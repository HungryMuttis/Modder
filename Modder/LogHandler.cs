namespace Modder
{
    internal class LogHandler
    {
        private readonly object Lock = new();
        private RichTextBox? Logs { get; set; }
        private DateTime Creation { get; } = DateTime.Now;
        internal string LogsFolder { get; } = @"Logs\";
        internal LogHandler(RichTextBox logs)
        {
            Logs = logs;
        }

        internal void New(LogType type, string message)
        {
            Thread thread = new(() => MakeLog(type, message));
            thread.Start();
        }
        private void AppendText(string text, Color color)
        {
            Logs?.Invoke(new Action(() => Logs.SelectionStart = Logs.TextLength));
            Logs?.Invoke(new Action(() => Logs.SelectionLength = 0));
            Logs?.Invoke(new Action(() => Logs.SelectionColor = color));
            Logs?.Invoke(new Action(() => Logs.AppendText(text)));
            Logs?.Invoke(new Action(() => Logs.SelectionColor = Logs.ForeColor));
        }

        private void MakeLog(LogType type, string message)
        {
            try
            {
                lock (Lock)
                {
                    string part1 = $"[{DateTime.Now}] ";
                    string part2 = $"[{type,-8}] ";
                    string part3 = $"{message}";
                    using (StreamWriter writer = new($"{LogsFolder}{Creation:dd-MM-yyyy HH.mm.ss}.txt", append: true))
                        writer.WriteLine(part1 + part2 + part3);
                    AppendText(part1, Color.DarkGray);
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
                    AppendText(part2 + part3 + "\n", color);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to log.\nMessage: {message}\nError: {ex}", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
