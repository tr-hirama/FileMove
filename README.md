# TSファイル移動ツール (fileMove)

録画したアニメの動画ファイル（`.ts` / `.mp4`）を、番組タイトルから「しょぼいカレンダー」の
タイトルID（tid）を判定し、コピー先のtid別フォルダへ自動仕分け・移動するWindowsデスクトップアプリ/
A Windows desktop tool that sorts recorded anime video files into destination folders by
their "Shobocal" title ID (tid), determined from the program title.

> 文字コードに関する注意: ソース（`Form1.cs` 等）は **Shift-JIS (CP932)** で保存されています。
> UTF-8前提のエディタ/ツールで開くと日本語が文字化けします。

---

## 概要

| 項目 | 内容 |
|------|------|
| 種別 | Windows Forms デスクトップアプリ |
| 言語 / FW | C# / .NET 10 (`net10.0-windows`) |
| ソリューション | `fileMove.sln` |
| プロジェクト | `WinFormsApp1/fileMove.csproj`（アセンブリ名 `fileMove`） |
| 主要パッケージ | [AngleSharp](https://anglesharp.github.io/) 1.0.1（HTMLスクレイピング） |
| ウィンドウタイトル | 「TSファイル移動ツール」 |
| バージョン管理 | なし（git未初期化） |

---

## 動作の流れ

1. **起動時** に `getAnimeList()` が「しょぼいカレンダー」(https://cal.syoboi.jp) の
   放送中リスト（`cat=1`, `cat=10`, `cat=4`）を AngleSharp でスクレイピングし、
   `タイトル → tid（4桁ゼロ埋め）` の一覧 `Animes` を構築する。
2. **コピー元フォルダ**（既定 `T:\`）から `*.ts.program.txt`（番組情報ファイル）を列挙。
3. 各ファイル名から **番組タイトルを抽出**：
   - 先頭19文字（日付プレフィックス）を除去
   - 大量の `Replace` チェーンで、放送局タグ・`[新]`等の記号・全角英数字記号を除去/半角化して正規化
4. 同じファイル名を持つ `.mp4` と `_cut.ts` を探す。
5. 抽出タイトルを、**ハードコードの対応表 `addList`** と **スクレイピング結果 `Animes`** に
   照合して tid を決定（タイトルを末尾から1文字ずつ短くして部分一致を試行）。
6. **コピー先フォルダ**（既定 `K:\`）から `tid*` で始まるフォルダを検索し、
   見つかれば `mp4` / `_cut.ts` / `program.txt` を `FileSystem.MoveFile`（上書き確認ダイアログ付き）で移動。
7. 処理ログは画面下部のテキストボックスに出力。

---

## 画面（UI）

| コントロール | 役割 |
|--------------|------|
| `textBox1` | コピー元フォルダ（既定 `T:\`） |
| `textBox3` | コピー先フォルダ（既定 `K:\`） |
| `checkBox1` | **テストモード** — ONのとき実ファイル移動を行わず判定ログのみ出力 |
| `button1` (Start) | 仕分け・移動処理を実行 |
| `button2` (TEST) | アニメ一覧（しょぼいカレンダー）を再取得 |
| `textBox2` | 処理ログ（読み取り専用） |

> 安全機構: コピー先（`textBox3`）に存在しないパスを入力すると、テストモードが自動的にONになる。

---

## ビルド & 実行

```powershell
# 必要: .NET 10 SDK / Windows
dotnet build fileMove.sln -c Release
dotnet run --project WinFormsApp1/fileMove.csproj
```

または Visual Studio 2026 で `fileMove.sln` を開いて F5。

---

## 主要ファイル

```
fileMove.sln                         ソリューション
WinFormsApp1/
├── fileMove.csproj                  プロジェクト定義（.NET10 / AngleSharp）
├── anime.json                       タイトル→tid 対応表（外部データ。exe隣へコピー）
├── Program.cs                       エントリポイント（Application.Run(new Form1())）
├── Form1.cs                         本体ロジック（タイトル抽出・tid判定・ファイル移動）
├── Form1.Designer.cs                画面定義（UTF-8。UI部品とイベント結線）
├── Form1.resx / Properties/         リソース
└── (bin/ obj/ .vs/                  ビルド成果物・IDEキャッシュ。VCS対象外推奨)
```

---

## データソースとロジックの要点

- **`Anime` 構造体**: `title`（番組名）と `tid`（4桁ID）のペア。
- **`addList`**: タイトル→tid の静的フォールバック表（233件）。スクレイピングで
  取れない/誤判定するタイトルを補う目的。**`anime.json`（外部ファイル）から起動時に読み込む**
  （`LoadAddList()`）。`anime.json` は UTF-8、`[{"title":"...","tid":"...."}, ...]` 形式で、
  ビルド時に exe と同じ出力フォルダへコピーされる（`CopyToOutputDirectory`）。
  → **再コンパイルなしで `anime.json` を編集するだけで対応表を更新できる。**
  `tid = "0000"` は「ID未確定だがフォルダ名 `0000_タイトル` に直接振り分ける」特例。
- **タイトル正規化**: ファイル名の表記ゆれ（局名・全角記号・各種タグ）を `Replace` の連鎖で吸収。
  ここが判定精度のキモであり、最も壊れやすい/メンテが必要な箇所。
- **部分一致照合**: タイトル全長から1文字ずつ削って `Contains` で照合するため、
  シリーズ表記（「Season2」等）の差異をある程度許容する。

---

## 既知の注意点・改善余地

- ソースが Shift-JIS のため、UTF-8環境での編集・diff・レビューで文字化けする。
- タイトル正規化が巨大な `Replace` チェーンで、可読性・保守性が低い（設定ファイル化の余地）。
- コピー元/先パス（`T:\` / `K:\`）やハードコード対応表がコードに直書き。
- 外部サイト（しょぼいカレンダー）のHTML構造変更に依存（壊れるとtid取得が失敗）。
- git未管理 — 履歴・バックアップが無い。
- `bin/`・`obj/`・`.vs/` がそのまま存在。`.gitignore` の追加を推奨。
```
