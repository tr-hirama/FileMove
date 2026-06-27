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
using System.Text.Json;

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
        List<Anime> addList = new();

        public Form1()
        {
            InitializeComponent();

            textBox1.Text = fromFolder;
            textBox3.Text = toFolder;

            LoadAddList();
            getAnimeList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadAddList();
            string[] allFiles = Directory.GetFiles(textBox1.Text);

            string[] names = Directory.GetFiles(textBox1.Text, "*.ts.program.txt");

            foreach (string name in names)
            {
                string filePath = name.Replace(".ts.program.txt", "");
                string fileName = Path.GetFileName(filePath);

                if (fileName.Length < 19) { textBox2.AppendText("skip (name too short): " + fileName + Environment.NewLine); continue; }

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

                if (title.Length > 5 && title.Substring(0, 4) == "アニメ ")
                {
                    title = title.Substring(4);
                }

                title = title.Trim();

                textBox2.AppendText(title + Environment.NewLine);

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

                    else if (Path.GetExtension(str).ToLower() == ".ts" && str.IndexOf("_cut.ts") >= 0)
                    {
                        cutTSPath = str;
                    }
                }

                if (mp4Path == "" && cutTSPath == "") { textBox2.AppendText("×移動不可 " + title + " : 動画(.mp4/_cut.ts)が見つからない" + Environment.NewLine); continue; }

                if (mp4Path != "" || cutTSPath != "")
                {
                    textBox2.AppendText(fnCount.ToString() + " : " + mp4Path + " : " + cutTSPath + Environment.NewLine);

                    int cc = title.Length;

                    string tidF = TidFromProgramFile(name, title);
                    if (!string.IsNullOrEmpty(tidF))
                    {
                        textBox2.AppendText(" file->tid " + tidF + Environment.NewLine);
                        if (!checkBox1.Checked) MoveByTid(tidF, mp4Path, cutTSPath, name);
                        continue;
                    }


                    bool resolved = false;
                    for (int i = cc; i > 0; i--)
                    {
                        int rc = 0;
                        string prefix = title.Substring(0, i);

                        var result1 = addList.Where(x => x.title.Contains(prefix));
                        string tid = string.Empty;

                        foreach (Anime anime in result1)
                        {
                            tid = anime.tid;
                            textBox2.AppendText(" ※ " + anime.tid + Environment.NewLine);

                            rc = 1;
                        }

                        if (string.IsNullOrWhiteSpace(tid))
                        {
                            var result2 = Animes.Where(x => x.title.Contains(prefix));

                            foreach (Anime anime in result2)
                            {
                                tid = anime.tid;
                                textBox2.AppendText(" ※ " + anime.tid + Environment.NewLine);

                                rc = 1;
                            }

                        }

                        if (rc == 1)
                        {
                            resolved = true;
                            if (checkBox1.Checked)
                            {
                                textBox2.AppendText(tid + "_" + prefix + Environment.NewLine);
                                break;
                            }

                            string[] toFolders = Directory.GetDirectories(textBox3.Text, tid + "*");
                            if (toFolders.Length != 1 && tid != "0000") { textBox2.AppendText("×移動不可 " + title + " : 移動先フォルダ " + tid + "* が " + toFolders.Length + "件" + Environment.NewLine); }

                            if (toFolders.Length == 1)
                            {
                                textBox2.AppendText(toFolders[0] + Environment.NewLine);

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
                                toFolders = Directory.GetDirectories(textBox3.Text, tid + "_" + prefix);

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

                            textBox2.AppendText(tid + "_" + prefix + Environment.NewLine);
                            break;
                        }
                    }
                    if (!resolved) { textBox2.AppendText("×移動不可 " + title + " : tidを特定できない" + Environment.NewLine); }
                }
            }


        }

        // exe 隣の anime.json から タイトル→tid 対応表を読み込む(べた書きの置き換え)
        void LoadAddList()
        {
            try
            {
                string jsonPath = Path.Combine(AppContext.BaseDirectory, "anime.json");
                if (System.IO.File.Exists(jsonPath))
                {
                    string json = System.IO.File.ReadAllText(jsonPath);
                    var opts = new JsonSerializerOptions { IncludeFields = true };
                    var items = JsonSerializer.Deserialize<List<Anime>>(json, opts);
                    if (items != null) addList = items;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("anime.json 読み込みに失敗: " + ex.Message);
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
            try
            {
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
                int idx = tidA.IndexOf("\">");
                if (idx < 0) continue;
                string tidB = tidA.Substring(0, idx);
                string tid = int.Parse(tidB).ToString("0000");

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
                int idx = tidA.IndexOf("\">");
                if (idx < 0) continue;
                string tidB = tidA.Substring(0, idx);
                string tid = int.Parse(tidB).ToString("0000");

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
                int idx = tidA.IndexOf("\">");
                if (idx < 0) continue;
                string tidB = tidA.Substring(0, idx);
                string tid = int.Parse(tidB).ToString("0000");

                Anime anime = new Anime(title, tid);
                Animes.Add(anime);

                count++;
            }

            textBox2.AppendText("しょぼいカレンダータイトル一覧読込完了：" + count.ToString() + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show("getAnimeList failed: " + ex.Message);
            }

        }


        // anime.json を既定アプリで開いて編集できるようにする
        private void button3_Click(object sender, EventArgs e)
        {
            string jsonPath = Path.Combine(AppContext.BaseDirectory, "anime.json");
            if (!System.IO.File.Exists(jsonPath))
            {
                MessageBox.Show("anime.json が見つかりません: " + jsonPath);
                return;
            }
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo("notepad.exe", "\"" + jsonPath + "\"") { UseShellExecute = true };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("編集アプリの起動に失敗: " + ex.Message);
            }
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

        private void buttonBrowseFrom_Click(object sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog { SelectedPath = textBox1.Text };
            if (dlg.ShowDialog() == DialogResult.OK)
                textBox1.Text = dlg.SelectedPath;
        }

        // ---- added: resolve tid from program.txt broadcast time via syoboi ----
        static readonly System.Net.Http.HttpClient _http = MakeHttp();
        static System.Net.Http.HttpClient MakeHttp()
        {
            var h = new System.Net.Http.HttpClient();
            h.Timeout = TimeSpan.FromSeconds(12);
            h.DefaultRequestHeaders.Add("User-Agent", "FileMove/1.0");
            return h;
        }

        // Read the broadcast start time from program.txt line 1, query syoboi ProgLookup,
        // and return a 4-digit tid. If several programs share that minute, disambiguate by
        // normalizedTitle. Returns "" when undecidable (caller falls back to the fuzzy match).
        string TidFromProgramFile(string programTxtPath, string normalizedTitle)
        {
            try
            {
                var enc = System.Text.Encoding.GetEncoding(932);
                string content = enc.GetString(System.IO.File.ReadAllBytes(programTxtPath));
                string firstLine = content.Split((char)10)[0];
                var m = System.Text.RegularExpressions.Regex.Match(firstLine, "(\\d{4})/(\\d{2})/(\\d{2}).*?(\\d{1,2}):(\\d{2})");
                if (!m.Success) return "";
                var start = new DateTime(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value),
                                         int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value), 0);
                string range = start.AddMinutes(-1).ToString("yyyyMMdd_HHmmss") + "-" + start.AddMinutes(1).ToString("yyyyMMdd_HHmmss");
                string progXml = _http.GetStringAsync("https://cal.syoboi.jp/db.php?Command=ProgLookup&Range=" + range).GetAwaiter().GetResult();
                var doc = XDocument.Parse(progXml);
                string hhmm = start.ToString("HH:mm");
                var cands = doc.Descendants("ProgItem")
                    .Where(p => DateTime.TryParse((string?)p.Element("StTime"), out var st) && st.ToString("HH:mm") == hhmm)
                    .Select(p => (string?)p.Element("TID"))
                    .Where(t => !string.IsNullOrEmpty(t)).Select(t => t!).Distinct().ToList();
                if (cands.Count == 0) return "";
                if (cands.Count == 1) return int.Parse(cands[0]).ToString("0000");
                string titleXml = _http.GetStringAsync("https://cal.syoboi.jp/db.php?Command=TitleLookup&TID=" + string.Join(",", cands)).GetAwaiter().GetResult();
                var tdoc = XDocument.Parse(titleXml);
                string key = NormForMatch(normalizedTitle);
                var hits = tdoc.Descendants("TitleItem")
                    .Where(t => { string st = NormForMatch((string?)t.Element("Title")); return key.Length > 0 && st.Length > 0 && (st.Contains(key) || key.Contains(st)); })
                    .Select(t => (string?)t.Element("TID"))
                    .Where(t => !string.IsNullOrEmpty(t)).Select(t => t!).Distinct().ToList();
                if (hits.Count == 1) return int.Parse(hits[0]).ToString("0000");
                return "";
            }
            catch { return ""; }
        }

        // Light title-compare key: fullwidth digits/letters -> ascii, drop spaces, lowercase.
        static string NormForMatch(string? s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            var sb = new System.Text.StringBuilder(s.Length);
            foreach (char c in s)
            {
                char x = c;
                if (x >= (char)0xFF10 && x <= (char)0xFF19) x = (char)('0' + (x - 0xFF10));
                else if (x >= (char)0xFF21 && x <= (char)0xFF3A) x = (char)('A' + (x - 0xFF21));
                else if (x >= (char)0xFF41 && x <= (char)0xFF5A) x = (char)('a' + (x - 0xFF41));
                if (char.IsWhiteSpace(x)) continue;
                sb.Append(char.ToLowerInvariant(x));
            }
            return sb.ToString();
        }

        // Move using a syoboi-resolved tid (same rule as the main loop: only when exactly one tid* folder).
        void MoveByTid(string tid, string mp4Path, string cutTSPath, string name)
        {
            string[] toFolders = Directory.GetDirectories(textBox3.Text, tid + "*");
            if (toFolders.Length != 1) { textBox2.AppendText("×移動不可 tid=" + tid + " : 移動先フォルダ " + tid + "* が " + toFolders.Length + "件" + Environment.NewLine); return; }
            textBox2.AppendText(toFolders[0] + Environment.NewLine);
            if (mp4Path != "") FileSystem.MoveFile(mp4Path, Path.Combine(toFolders[0], Path.GetFileName(mp4Path)), UIOption.AllDialogs);
            if (cutTSPath != "") FileSystem.MoveFile(cutTSPath, Path.Combine(toFolders[0], Path.GetFileName(cutTSPath)), UIOption.AllDialogs);
            if (name != "") FileSystem.MoveFile(name, Path.Combine(toFolders[0], Path.GetFileName(name)), UIOption.AllDialogs);
        }

        private void buttonBrowseTo_Click(object sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog { SelectedPath = textBox3.Text };
            if (dlg.ShowDialog() == DialogResult.OK)
                textBox3.Text = dlg.SelectedPath;
        }
    }
}