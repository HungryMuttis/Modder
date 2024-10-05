using NLog;

namespace Modder
{
    internal class LogHandler
    {
        private readonly object Lock = new();
        private RichTextBox? Logs { get; set; }
        private string FilePath { get; }
        internal LogHandler(RichTextBox? logs = null, string path = @"Logs\", string cName = "")
        {
            this.Logs = logs;
            this.FilePath = $"{path}{DateTime.Now:yyyy-MM-dd HH.mm.ss}{(cName == "" ? "" : " " + cName)}.log";
            if (!Directory.Exists(string.Join('\\', this.FilePath.Split(@"\")[0..^1])))
                Directory.CreateDirectory(string.Join('\\', this.FilePath.Split(@"\")[0..^1]));
            File.Create(this.FilePath);
            
        }

        public void NewRichTextBox(RichTextBox? logs, bool addOldLogs = false)
        {
            this.Logs = logs;

            if (addOldLogs)
            {
                List<string> lines = [];
                /*using (StreamReader sr = new(this.FS))
                {
                    string? line;
                    while ((line = sr.ReadLine()) != null)
                        lines.Add(line);
                }*/
                foreach(string line in lines)
                {
                    string[] splitLine = line.Split(' ');
                    string p1 = string.Join(' ', splitLine[0..1]);
                    string p2 = splitLine[2][1..^1];
                    Console.WriteLine(p2);
                    string p3 = string.Join(' ', splitLine[3..]);
                    splitLine[^1] = p1;
                }
            }
        }
        public void NewRichTextBox(RichTextBox? logs, string path)
        {
            this.Logs = logs;
        }

        public void NewThread(LogType type, string message)
        {
            Thread thread = new(() => MakeLog(type, message));
            thread.Start();
        }
        public void New(LogType type, string message)
        {
            Console.WriteLine($"Logging {message}");
            MakeLog(type, message);
        }
        public Task NewAsyncTask(LogType type, string message)
        {
            return MakeLogAsync(type, message);
        }
        public async void NewAsync(LogType type, string message)
        {
            await MakeLogAsync(type, message);
        }
        private int AppendText(string text, Color color)
        {
            if (Logs == null)
            {
                return 1;
            }

            Logs.Invoke(new Action(() => Logs.SelectionStart = Logs.TextLength));
            Logs.Invoke(new Action(() => Logs.SelectionLength = 0));
            Logs.Invoke(new Action(() => Logs.SelectionColor = color));
            Logs.Invoke(new Action(() => Logs.AppendText(text)));
            Logs.Invoke(new Action(() => Logs.SelectionColor = Logs.ForeColor));
            return 0;
        }

        private int MakeLog(LogType type, string message)
        {
            try
            {
                string p1 = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ";
                string p2 = $"[{type,-8}] {message}";
                lock(Lock)
                {
                    /*using StreamWriter wr = new(this.FS);
                        wr.Write(p1 + p2);*/

                    int ret;
                    if ((ret = UseAppend(p1, p2, type)) != 0)
                        return ret;
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to log.\nMessage: {message}\nError: {ex}", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private async Task<int> MakeLogAsync(LogType type, string message)
        {
            try
            {
                string p1 = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}] ";
                string p2 = $"[{type,-8}] {message}";

                /*using StreamWriter wr = new(this.FS);
                    await wr.WriteAsync(p1 + p2);*/

                lock (Lock)
                {
                    int ret;
                    if ((ret = UseAppend(p1, p2, type)) != 0)
                        return ret;
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to log.\nMessage: {message}\nError: {ex}", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        
        private int UseAppend(string p1, string p2, LogType type)
        {
            int ret;
            if ((ret = AppendText(p1, Color.DarkGray)) != 0)
                return ret;

            Color color = type switch
            {
                LogType.None => Color.White,
                LogType.Info => Color.Blue,
                LogType.OK => Color.Green,
                LogType.Warning => Color.Yellow,
                LogType.Error => Color.DarkOrange,
                LogType.Critical => Color.Coral,
                LogType.Fatal => Color.Crimson,
                _ => Color.White,
            };

            if ((ret = AppendText(p2 + "\n", color)) != 0)
                return ret;
            return 0;
        }
    }
}
