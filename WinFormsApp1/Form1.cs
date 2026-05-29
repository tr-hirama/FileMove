using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io;

using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;
using Microsoft.VisualBasic.FileIO;

namespace WinFormsApp1
{
    public struct Anime
    {
        public string title;
        public string tid;

        public Anime(string title, string tid)
        {
            this.title = title;
            this.tid = tid;
        }

    }





    public partial class Form1 : Form
    {
        string fromFolder = @"T:\";
        string toFolder = @"K:\";

        List<Anime> Animes = new();
        Anime[] addList = {
            new Anime{title = "百姓貴族", tid = "6726"},
            new Anime{title = "百姓貴族", tid = "6726"},
            new Anime{title = "百姓貴族", tid = "6726"},
            new Anime{title = "百姓貴族", tid = "6726"},
            new Anime{title = "百姓貴族", tid = "6726"},
            new Anime{title = "百姓貴族", tid = "6726"},
            new Anime{title = "魔入りました！入間くん４", tid = "5443"},
            new Anime{title = "鉄腕アトム", tid = "7429"},
            new Anime{title = "魔法のプリンセス ミンキーモモ ", tid = "2968"},
            new Anime{title = "正反対な君と僕 ", tid = "7691"},
            new Anime{title = "聖女なのに国を追い出されたので、崩壊寸前の隣国へ来ました シーズン2", tid = "7345"},
            new Anime{title = "魔都精兵のスレイブ2", tid = "6906"},
            new Anime{title = "Fate/strange Fake ", tid = "7663"},
            new Anime{title = "MFゴースト 3rd Season", tid = "6726"},
            new Anime{title = "嘆きの亡霊は引退したい 第2", tid = "7218"},
            new Anime{title = "嘆きの亡霊は引退したい 第", tid = "7218"},
            new Anime{title = "元祖！バンドリちゃん", tid = "4411"},
            new Anime{title = "姫様“拷問”の時間です(第2期)", tid = "6893"},
            new Anime{title = "スクールランブル二学期", tid = "0447"},
            new Anime{title = "ヴァイスクロイツ　グリーエン", tid = "0000"},
            new Anime{title = "鬼灯の冷徹 第弐期その弐", tid = "3266"},
            new Anime{title = "BanG Dream! 3rd Season", tid = "4411"},
            new Anime{title = "百姓貴族", tid = "6726"},
            new Anime{title = "青のオーケストラ シーズン", tid = "6626"},
            new Anime{title = "とんでもスキルで異世界放浪メシ2", tid = "6568"},
            new Anime{title = "キングダム　第", tid = "2554"},
            new Anime{title = "結婚指輪物語Ⅱ", tid = "6898"},
            new Anime{title = "かくりよの宿飯 弐", tid = "6726"},
            new Anime{title = "百姓貴族 3rd Season", tid = "6726"},
            new Anime{title = "本好きの下剋上 司書になるためには手段を選んでいられません 第", tid = "5420"},
            new Anime{title = "異世界かるてっと3", tid = "5244"},
            new Anime{title = "Fate/EXTRA Last Encore", tid = "3188"},
            new Anime{title = "ガンダムビルドファイターズ ", tid = "3188"},
            new Anime{title = "ウマ娘 シンデレラグレイ(第2クール)", tid = "4851"},
            new Anime{title = "ケンガンアシュラ Season2 Part.2", tid = "5546"},
            new Anime{title = "本好きの下剋上 司書になるためには手段を選んでいられません ", tid = "5420"},
            new Anime{title = "ゴールデンカムイ(第三期)", tid = "4881"},
            new Anime{title = "自動販売機に生まれ変わった俺は迷宮を彷徨う 2nd season", tid = "6723"},
            new Anime{title = "ぐらんぶる ", tid = "4967"},
            new Anime{title = "カッコウの許嫁 Season2", tid = "6330"},
            new Anime{title = "ぐらんぶる Season 2", tid = "4967"},
            new Anime{title = "SAKAMOTO DAYS ", tid = "7287"},
            new Anime{title = "盾の勇者の成り上がり Season 4　", tid = "5149"},
            new Anime{title = "その着せ替え人形は恋をする ", tid = "6200"},
            new Anime{title = "ぐらんぶる ", tid = "4967"},
            new Anime{title = "魔法少女リリカルなのはA", tid = "0508"},
            new Anime{title = "阿波連さんははかれない season2-", tid = "6286"},
            new Anime{title = "スライム倒して300年、知らないうちにレベルMAXになってました ～そのに～", tid = "5935"},
            new Anime{title = "黒執事 -緑の魔女編-", tid = "1477"},
            new Anime{title = "炎炎ノ消防隊 参ノ章", tid = "5359"},
            new Anime{title = "WIND BREAKER ", tid = "6990"},
            new Anime{title = "WIND BREAKER Season", tid = "6990"},
            new Anime{title = "ウマ娘 シンデレラグレイ", tid = "4851"},
            new Anime{title = "僕のヒーローアカデミア ", tid = "4133"},
            new Anime{title = "戦隊大失格 ", tid = "6988"},
            new Anime{title = "戦隊大失格 2nd", tid = "6988"},
            new Anime{title = "スライム倒して300年、知らないうちにレベルMAXになってました ", tid = "5935"},
            new Anime{title = "キン肉マン 完璧超人始祖編 Season", tid = "7096"},
            new Anime{title = "魔神英雄伝ワタル2", tid = "1535"},
            new Anime{title = "君のことが大大大大大好きな100人の彼女 第2期", tid = "6842"},
            new Anime{title = "魔神英雄伝ワタル2", tid = "1535"},
            new Anime{title = "地縛少年花子くん2 ", tid = "5545"},
            new Anime{title = "小林さんちのメイドラゴン ", tid = "4418"},
            new Anime{title = "Dr.STONE SCIENCE FUTURE", tid = "5361"},
            new Anime{title = "青の祓魔師 終夜篇", tid = "2123"},
            new Anime{title = "異修羅 ", tid = "6953"},
            new Anime{title = "俺だけレベルアップな件 S", tid = "6931"},
            new Anime{title = "わたしの幸せな結婚 第", tid = "6738"},
            new Anime{title = "わたしの幸せな結婚 ", tid = "6738"},
            new Anime{title = "夏目友人帳 漆", tid = "1414"},
            new Anime{title = "蒼穹のファフナー THE BEYOND", tid = "0405"},
            new Anime{title = "MFゴースト 2nd Season", tid = "6809"},
            new Anime{title = "神之塔 -Tower of God- 工房戦", tid = "5635"},
            new Anime{title = "ありふれた職業で世界最強 season3", tid = "5350"},
            new Anime{title = "ありふれた職業で世界最強 season 3", tid = "5350"},
            new Anime{title = "鴨乃橋ロンの禁断推理 2nd Season", tid = "6840"},
            new Anime{title = "ブルーロック VS. U-20 JAPAN", tid = "6477"},
            new Anime{title = "BLEACH 千年血戦篇-相剋譚-", tid = "0491"},
            new Anime{title = "青の祓魔師 雪ノ果篇", tid = "2123"},
            new Anime{title = "精霊幻想記2", tid = "6055"},
            new Anime{title = "美少女戦士セーラームーンS", tid = "1707"},
            new Anime{title = "アイドルマスター シャイニーカラーズ 2nd season", tid = "7012"},
            new Anime{title = "ケンガンアシュラ Season2 Part.1", tid = "5546"},
            new Anime{title = "ダンジョンに出会いを求めるのは間違っているだろうかⅤ 豊穣の女神篇", tid = "3695"},
            new Anime{title = "ソードアート・オンライン オルタナティブ ガンゲイル・オンラインⅡ", tid = "4872"},
            new Anime{title = "百姓貴族 2nd Season", tid = "6726"},
            new Anime{title = "齢5000年の草食ドラゴン、いわれなき邪竜認定 season2", tid = "6557"},
            new Anime{title = "るろうに剣心 -明治剣客浪漫譚- 京都動乱", tid = "1560"},
            new Anime{title = "侵略！", tid = "2042"},
            new Anime{title = "キン肉マン 完璧超人始祖編", tid = "7096"},
            new Anime{title = "Re:ゼロから始める異世界生活", tid = "4114"},
            new Anime{title = "THEビッグオー", tid = "0102"},
            new Anime{title = "戦国妖狐 千魔混沌編", tid = "6923"},
            new Anime{title = "キミと僕の最後の戦場、あるいは世界が始まる聖戦 Season Ⅱ", tid = "5769"},
            new Anime{title = "キン肉マン", tid = "0000"},
            new Anime{title = "Re：ゼロから始める異世界生活　一部再放送", tid = "4114"},
            new Anime{title = "神之塔 -Tower of God- 王子の帰還(", tid = "5635"},
            new Anime{title = "アイドルマスター シンデレラガールズ", tid = "3588"},
            new Anime{title = "アイドルマスター シンデレラガールズ 2nd S", tid = "3588"},
            new Anime{title = "FAIRY TAIL 100年クエスト", tid = "1755"},
            new Anime{title = "響け！ユーフォニアム3", tid = "3687"},
            new Anime{title = "シンカリオン チェンジ ザ ワールド", tid = "4801"},
            new Anime{title = "無職転生Ⅱ ～異世界行ったら本気だす～", tid = "5851"},
            new Anime{title = "にじよん あにめーしょん2", tid = "6585"},
            new Anime{title = "この素晴らしい世界に祝福を！3", tid = "4016"},
            new Anime{title = "黒執事 -寄宿学校編-", tid = "1477"},
            new Anime{title = "魔法科高校の劣等生 第3シーズン", tid = "3334"},
            new Anime{title = "スペース☆ダンディ ", tid = "3241"},
            new Anime{title = "転生したらスライムだった件 第3期", tid = "5048"},
            new Anime{title = "ハイスピードエトワール", tid = "7019"},
            new Anime{title = "バンステ！", tid = "5187"},
            new Anime{title = "ラディアン(", tid = "5079"},
            new Anime{title = "不機嫌なモノノケ庵 續", tid = "4227"},
            new Anime{title = "せいぜいがんばれ！魔法少女くるみ", tid = "5190"},
            new Anime{title = "青の祓魔師 島根啓明結社篇", tid = "2123"},
            new Anime{title = "Fate／Zero", tid = "0663"},
            new Anime{title = "Fate／Zero ", tid = "0663"},
            new Anime{title = "ようこそ実力至上主義の教室へ 3rd Season", tid = "4612"},
            new Anime{title = "弱キャラ友崎くん 2nd STAGE", tid = "5840"},
            new Anime{title = "冬のゆるキャン△まつり", tid = "4783"},
            new Anime{title = "HIGH CARD season 2", tid = "6550"},
            new Anime{title = "SHAMAN KING FLOWERS", tid = "0232"},
            new Anime{title = "姫様“拷問”の時間です", tid = "6893"},
            new Anime{title = "月が導く異世界道中 第二幕", tid = "6034"},
            new Anime{title = "真の仲間じゃないと勇者のパーティー", tid = "6126"},
            new Anime{title = "Fate/Apocrypha ", tid = "0663"},
            new Anime{title = "カノジョも彼女 ", tid = "6030"},
            new Anime{title = "最果てのパラディン 鉄錆の山の王", tid = "6123"},
            new Anime{title = "進撃の巨人 The Final Season 完結編", tid = "5827"},
            new Anime{title = "東京リベンジャーズ 天竺編", tid = "5939"},
            new Anime{title = "アイドルマスター ミリオンライブ！", tid = "6836"},
            new Anime{title = "しーくれっとみっしょん～潜入捜査官は絶対に負けない！～ ＃", tid = "6843"},
            new Anime{title = "キャッツ・アイ", tid = "1060"},
            new Anime{title = "魔法使いの嫁 SEASON2", tid = "4697"},
            new Anime{title = "魔法使いの嫁 SEASON2 ", tid = "4697"},
            new Anime{title = "アークナイツ【冬隠帰路/PERISH IN FROST】", tid = "6500"},
            new Anime{title = "ゴブリンスレイヤー Ⅱ", tid = "5052"},
            new Anime{title = "ゴブリンスレイヤーⅡ", tid = "5052"},
            new Anime{title = "22/7計算中 シーズン5", tid = "5904"},
            new Anime{title = "盾の勇者の成り上がり Season 3", tid = "5149"},
            new Anime{title = "聖女の魔力は万能です Season2", tid = "5933"},
            new Anime{title = "キャプテン翼シーズン2 ジュニアユース編", tid = "0932"},
            new Anime{title = "ウマ娘 プリティーダービー Season 3", tid = "4851"},
            new Anime{title = "陰の実力者になりたくて！ 2nd season", tid = "6452"},
            new Anime{title = "ようこそ実力至上主義の教室へ 2nd Season", tid = "4612"},
            new Anime{title = "範馬刃牙", tid = "4971"},
            new Anime{title = "機動戦士ガンダムSEED ", tid = "0019"},
            new Anime{title = "響け！ユーフォニアム2", tid = "3687"},
            new Anime{title = "スパイ教室", tid = "6584"},
            new Anime{title = "呪術廻戦 ＃", tid = "5764"},
            new Anime{title = "キャプテン翼シーズン2", tid = "4899"},
            new Anime{title = "ゆるキャン△ SEASON2", tid = "4783"},
            new Anime{title = "デジモンアドベンチャー02", tid = "1292"},
            new Anime{title = "ようこそ実力至上主義の教室へ", tid = "4612"},
            new Anime{title = "無職転生Ⅱ", tid = "5851"},
            new Anime{title = "ゲゲゲの鬼太郎(第6期)", tid = "4945"},
            new Anime{title = "D4DJ All Mix", tid = "5690"},
            new Anime{title = "異世界かるてっと2", tid = "5244"},
            new Anime{title = "ハイキュー！！セカンドシーズン", tid = "3380"},
            new Anime{title = "ダークギャザリング", tid = "6744"},
            new Anime{title = "ゴールデンカムイ(第四期)", tid = "4881"},
            new Anime{title = "ゴールデンカムイ 4期", tid = "4881"},
            new Anime{title = "無職転生Ⅱ", tid = "5851"},
            new Anime{title = "機動戦士ガンダムSEED", tid = "0019"},
            new Anime{title = "ギヴン", tid = "5674"},
            new Anime{title = "ワンピース ", tid = "0350"},
            new Anime{title = "ようこそ実力至上主義の教室へ", tid = "4612"},
            new Anime{title = "るろうに剣心", tid = "1560"},
            new Anime{title = "七つの大罪", tid = "3542"},
            new Anime{title = "SPY×FAMILY", tid = "6309"},
            new Anime{title = "シュガーアップル・フェアリーテイル", tid = "6548"},
            new Anime{title = "アークナイツ", tid = "6500"},
            new Anime{title = "STEINS；GATE", tid = "2142"},
            new Anime{title = "邪神ちゃんドロップキック", tid = "4974"},
            new Anime{title = "彼女、お借りします", tid = "5693"},
            new Anime{title = "MIX～二度目の夏、空の向こうへ～", tid = "5287"},
            new Anime{title = "BLEACH 千年血戦篇-訣別譚", tid = "0491"},
            new Anime{title = "ホリミヤ -piece-", tid = "5853"},
            new Anime{title = "EDENS ZERO", tid = "5985"},
            new Anime{title = "魔王学院の不適合者 Ⅱ", tid = "5694"},
            new Anime{title = "BanG Dream！ It’s MyGO", tid = "6716"},
            new Anime{title = "TIGER ＆ BUNNY 2", tid = "6349"},
            new Anime{title = "D4DJTV side:nova", tid = "6707"},
            new Anime{title = "東京ミュウミュウにゅ～", tid = "6640"},
            new Anime{title = "日曜アニメ劇場", tid = "0000"},
            new Anime{title = "ダンジョンに出会いを求めるのは間違っているだろうかⅣ 深章 厄災篇", tid = "6535"},
            new Anime{title = "ルパン三世 PART2", tid = "3574"},
            new Anime{title = "アメトーーク", tid = "0000"},
            new Anime{title = "うる星やつら＜ノイタミナ＞", tid = "6480"},
            new Anime{title = "とある魔術の禁書目録Ⅱ", tid = "2029"},
            new Anime{title = "「文豪ストレイドッグス」第4シーズン", tid = "6575"},
            new Anime{title = "バンドリ！ガールズバンドパーティ！", tid = "0000"},
            new Anime{title = "水曜どうでしょう", tid = "0000"},
            new Anime{title = "おにぎりあたためますか", tid = "0000"},
            new Anime{title = "囲碁フォーカス", tid = "0000"}
        };

        public Form1()
        {
            InitializeComponent();

            textBox1.Text = fromFolder;
            textBox3.Text = toFolder;

            getAnimeList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] allFiles = Directory.GetFiles(fromFolder);

            string[] names = Directory.GetFiles(textBox1.Text, "*.ts.program.txt");

            foreach (string name in names)
            {
                string filePath = name.Replace(".ts.program.txt", "");
                string fileName = Path.GetFileName(filePath);

                string title = fileName.Substring(19).Replace("[Ｓ]", "").Replace("[字]", "").Replace("[デ]", "").Replace("[終]", "").Replace("[再]", "").Replace("[新]", "").Replace("[解]", "").Replace("[多]", "").Replace("アニメ・", "").Replace("アニメＡ・", "").Replace("アニメ『", "")
                    .Replace("[SS]", "").Replace("【新作】", "").Replace("【新作映画公開記念】", "").Replace("アニメA・", "").Replace("日５『", "").Replace("　", " ").Replace("日５「", "").Replace("　", " ")
                    .Replace("＜アニメギルド＞", "").Replace("＜アニおび＞", "").Replace("アニメの神様『", "").Replace("アニメA・", "").Replace("日５『", "").Replace("　", " ")
                    .Replace("０", "0").Replace("１", "1").Replace("２", "2").Replace("３", "3").Replace("４", "4").Replace("５", "5").Replace("６", "6").Replace("７", "7").Replace("８", "8").Replace("９", "9")
                    .Replace("Ａ", "A").Replace("Ｂ", "B").Replace("Ｃ", "C").Replace("Ｄ", "D").Replace("Ｅ", "E").Replace("Ｆ", "F").Replace("Ｇ", "G").Replace("Ｈ", "H").Replace("Ｉ", "I").Replace("Ｊ", "J")
                    .Replace("Ｋ", "K").Replace("Ｌ", "L").Replace("Ｍ", "M").Replace("Ｎ", "N").Replace("Ｏ", "O").Replace("Ｐ", "P").Replace("Ｑ", "Q").Replace("Ｒ", "R").Replace("Ｓ", "S").Replace("Ｔ", "T")
                    .Replace("Ｕ", "U").Replace("Ｖ", "V").Replace("Ｗ", "W").Replace("Ｘ", "X").Replace("Ｙ", "Y").Replace("Ｚ", "Z").Replace("／", "/").Replace("：", ":").Replace("ｅ", "e").Replace("（", "(")
                    .Replace("ａ", "a").Replace("ｂ", "b").Replace("ｃ", "c").Replace("ｄ", "d").Replace("ｅ", "e").Replace("ｆ", "f").Replace("ｇ", "g").Replace("ｈ", "h").Replace("ｉ", "i").Replace("ｊ", "j")
                    .Replace("ｋ", "k").Replace("ｌ", "l").Replace("ｍ", "m").Replace("ｎ", "n").Replace("ｏ", "o").Replace("ｐ", "p").Replace("ｑ", "q").Replace("ｒ", "r").Replace("ｓ", "s").Replace("ｔ", "t")
                    .Replace("ｕ", "u").Replace("ｖ", "v").Replace("ｗ", "w").Replace("ｘ", "x").Replace("ｙ", "y").Replace("ｚ", "z").Replace("）", ")").Replace("．", ".").Replace("BS11ガンダムアワー ", "")
                    .Replace("新アニメ", "").Replace("冬のゆるキャン△まつり", "").Replace("映画", "").Replace("－", "-").Replace("テレビアニメ「", "").Replace("アニメ「", "").Replace("）", ")").Replace("．", ".").Replace("劇場版", "")
                    .Replace("［新］", "").Replace(" AnichU", "").Replace("映画", "").Replace("－", "-").Replace("テレビアニメ「", "").Replace("アニメ「", "").Replace("）", ")").Replace("．", ".").Replace("劇場版", "")
                    .Replace("『", "").Replace("』", "").Replace("テレビアニメ", "").Replace("「", "").Replace("」", "").Replace("TVアニメ", "").Replace("映画公開記念", "").Replace("．", ".").Replace("アニメ「", "").Replace("TVシリーズ", "");

                if (title.Count() > 5 && title.Substring(0, 4) == "アニメ ")
                {
                    title = title.Substring(4);
                }

                title = title.Trim();

                textBox2.Text += title + Environment.NewLine;

                int fnCount = 0;
                string mp4Path = string.Empty;
                string cutTSPath = string.Empty;



                ///配列の中から、部分一致する要素だけ出力する
                foreach (string str in Array.FindAll(allFiles, delegate (string s) { return s.IndexOf(fileName) != -1; }))
                {
                    fnCount++;

                    if (Path.GetExtension(str).ToLower() == ".mp4")
                    {
                        mp4Path = str;
                    }

                    else if (Path.GetExtension(str).ToLower() == ".ts" && str.IndexOf("_cut.ts") > 0)
                    {
                        cutTSPath = str;
                    }
                }

                if (mp4Path != "" || cutTSPath != "")
                {
                    textBox2.Text += fnCount.ToString() + " : " + mp4Path + " : " + cutTSPath + Environment.NewLine;

                    int cc = title.Length;

                    for (int i = cc; i > 0; i--)
                    {
                        int rc = 0;

                        var result1 = addList.Where(x => x.title.Contains(title.Substring(0, i)));
                        string tid = string.Empty;

                        foreach (Anime anime in result1)
                        {
                            tid = anime.tid;
                            textBox2.Text += " ※ " + anime.tid + Environment.NewLine;

                            rc = 1;
                        }

                        if (string.IsNullOrWhiteSpace(tid))
                        {
                            var result2 = Animes.Where(x => x.title.Contains(title.Substring(0, i)));

                            foreach (Anime anime in result2)
                            {
                                tid = anime.tid;
                                textBox2.Text += " ※ " + anime.tid + Environment.NewLine;

                                rc = 1;
                            }

                        }

                        if (rc == 1)
                        {
                            if (checkBox1.Checked)
                            {
                                textBox2.Text += tid + "_" + title.Substring(0, i) + Environment.NewLine;
                                break;
                            }

                            string[] toFolders = Directory.GetDirectories(textBox3.Text, tid + "*");

                            if (toFolders.Length == 1)
                            {
                                textBox2.Text += toFolders[0] + Environment.NewLine;

                                if (mp4Path != "")
                                {
                                    var toPath = Path.Combine(toFolders[0], Path.GetFileName(mp4Path));

                                    // ファイルの移動（同じ名前のファイルがある場合は上書き）
                                    FileSystem.MoveFile(mp4Path, toPath, UIOption.AllDialogs);

                                }

                                if (cutTSPath != "")
                                {
                                    var toPath = Path.Combine(toFolders[0], Path.GetFileName(cutTSPath));

                                    // ファイルの移動（同じ名前のファイルがある場合は上書き）
                                    FileSystem.MoveFile(cutTSPath, toPath, UIOption.AllDialogs);

                                }


                                if (name != "")
                                {
                                    var toPath = Path.Combine(toFolders[0], Path.GetFileName(name));

                                    // ファイルの移動（同じ名前のファイルがある場合は上書き）
                                    FileSystem.MoveFile(name, toPath, UIOption.AllDialogs);

                                }

                            }
                            else if (tid == "0000")
                            {
                                toFolders = Directory.GetDirectories(textBox3.Text, tid + "_" + title.Substring(0, i));

                                if(toFolders.Length == 0) { break; }

                                if (mp4Path != "")
                                {

                                    var toPath = Path.Combine(toFolders[0], Path.GetFileName(mp4Path));


                                    // ファイルの移動（同じ名前のファイルがある場合は上書き）
                                    FileSystem.MoveFile(mp4Path, toPath, UIOption.AllDialogs);

                                }
                                else if (cutTSPath != ""){ 

                                    var toPath = Path.Combine(toFolders[0], Path.GetFileName(cutTSPath));


                                    // ファイルの移動（同じ名前のファイルがある場合は上書き）
                                    FileSystem.MoveFile(cutTSPath, toPath, UIOption.AllDialogs);

                                }


                                if (name != "")
                                {
                                    var toPath = Path.Combine(toFolders[0], Path.GetFileName(name));


                                    // ファイルの移動（同じ名前のファイルがある場合は上書き）
                                    FileSystem.MoveFile(name, toPath, UIOption.AllDialogs);

                                }
                            }

                            textBox2.Text += tid + "_" + title.Substring(0, i) + Environment.NewLine;
                            break;
                        }
                        else if (rc > 0)
                        {
                            textBox2.Text += "×" + title.Substring(0, i) + Environment.NewLine;
                            break;
                        }
                    }
                }
            }


        }

        async void getAnimeList()
        {
            //foreach (Anime anime in addList)
            //{
            //    Anime addAnime = new Anime(anime.title, anime.tid);
            //    Animes.Add(anime);
            //}


            // この辺で色々設定する
            var config = Configuration.Default
                .WithDefaultLoader(); // LoaderはデフォではいないのでOpenAsyncする場合につける

            // Headless Browser的なものを作る
            using var context = BrowsingContext.New(config);


            var doc = await context.OpenAsync("https://cal.syoboi.jp/list?cat=1");

            // すべての<td>要素の中からclass属性が'title'の要素を取得する
            var items = doc.QuerySelectorAll("td[class='title']");

            int count = 0;

            foreach (var item in items)
            {
                string title = item.QuerySelector("a")?.InnerHtml ?? "";
                string tidA = item.InnerHtml.Replace("<a href=\"/tid/", "");
                string tidB = tidA.Substring(0, tidA.IndexOf("\">"));
                string tid = Int16.Parse(tidB).ToString("0000");

                Anime anime = new Anime(title, tid);
                Animes.Add(anime);


                count++;
            }

            doc = await context.OpenAsync("https://cal.syoboi.jp/list?cat=10");

            // すべての<td>要素の中からclass属性が'title'の要素を取得する
            items = doc.QuerySelectorAll("td[class='title']");

            foreach (var item in items)
            {
                string title = item.QuerySelector("a")?.InnerHtml ?? "";
                string tidA = item.InnerHtml.Replace("<a href=\"/tid/", "");
                string tidB = tidA.Substring(0, tidA.IndexOf("\">"));
                string tid = Int16.Parse(tidB).ToString("0000");

                Anime anime = new Anime(title, tid);
                Animes.Add(anime);

                count++;
            }

            doc = await context.OpenAsync("https://cal.syoboi.jp/list?cat=4");

            // すべての<td>要素の中からclass属性が'title'の要素を取得する
            items = doc.QuerySelectorAll("td[class='title']");

            foreach (var item in items)
            {
                string title = item.QuerySelector("a")?.InnerHtml ?? "";
                string tidA = item.InnerHtml.Replace("<a href=\"/tid/", "");
                string tidB = tidA.Substring(0, tidA.IndexOf("\">"));
                string tid = Int16.Parse(tidB).ToString("0000");

                Anime anime = new Anime(title, tid);
                Animes.Add(anime);

                count++;
            }

            textBox2.Text += "しょぼいカレンダータイトル一覧読込完了：" + count.ToString() + Environment.NewLine;

        }


        private void Button2_Click(object sender, EventArgs e)
        {
            getAnimeList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if(System.IO.Directory .Exists(textBox3.Text))
            {
                checkBox1 .Checked = false;
            }
            else
            {
                checkBox1 .Checked = true;
            }
        }
    }
}