using Microsoft.VisualBasic.Logging;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace Modder
{
    public enum LogsRestoreMethod
    {
        TextBox,
        LogFile,
        ToDisplay
    }
    internal class LogHandler : IDisposable
    {
        private bool _disposed;
        private delegate void EmptyEventHandler();
        private event EmptyEventHandler textBoxChanged;

        private readonly object Lock = new();
        private RichTextBox? Logs { get; set; }
        private FileStream FS { get; set; }
        private StringBuilder ToDisplay { get; }
        private StreamWriter Writer { get; set; }
        private string FilePath { get; set; }
        private bool TextBoxReady { get; set; } = false;

        public bool Usable { get; private set; } = false;

        public bool WriteToFile { get; set; } = true;
        public bool WriteToTextBox { get; set; } = true;
        public bool WriteEnabled { get; set; } = true;

        internal LogHandler(RichTextBox? logs = null, string path = @"Logs\", string cName = "")
        {
            this.Logs = logs;
            this.FilePath = $"{path}{DateTime.Now:yyyy-MM-dd HH.mm.ss}{(cName == "" ? "" : " " + cName)}.log";
            if (!Directory.Exists(string.Join('\\', this.FilePath.Split(@"\")[0..^1])))
                Directory.CreateDirectory(string.Join('\\', this.FilePath.Split(@"\")[0..^1]));
            this.FS = File.Create(this.FilePath);
            this.Writer = new(this.FS)
            {
                AutoFlush = true
            };
            this.FS.Flush();
            this.ToDisplay = new();
            this.Usable = true;
            if (this.Logs != null)
                this.Logs.HandleCreated += (object? s, EventArgs e) => { this.TextBoxReady = true; };
            textBoxChanged += Display;
        }
        private void test() { }
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                this.Usable = false;
                this.Writer.Dispose();
                this.FS.Dispose();
                GC.SuppressFinalize(this);
            }
        }
        public void NewFolder(string path, string cName = "", bool delete = true, params LogsRestoreMethod[] restoreMethods)
        {
            string[] lines = this.Restore(restoreMethods);

            this.Writer.Dispose();
            this.FS.Dispose();
            if (delete)
            {
                File.Delete(this.FilePath);

                string? dir = Path.GetDirectoryName(this.FilePath);
                if (dir != null)
                {
                    using IEnumerator<string> en = Directory.EnumerateFileSystemEntries(dir).GetEnumerator();

                    if (!en.MoveNext())
                    {
                        en.Dispose();
                        Directory.Delete(dir);
                    }
                    else
                        en.Dispose();
                }
            }

            this.FilePath = $"{path}{(cName == "" ? Path.GetFileName(this.FilePath) : DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss") + cName)}";
            if (!Directory.Exists(string.Join('\\', this.FilePath.Split(@"\")[0..^1])))
                Directory.CreateDirectory(string.Join('\\', this.FilePath.Split(@"\")[0..^1]));
            this.FS = File.Create(this.FilePath);
            this.Writer = new(this.FS)
            {
                AutoFlush = true
            };
            this.FS.Flush();

            Write(LogHandler.UnpackLines(lines));
        }
        public void NewRichTextBox(RichTextBox? logs)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            this.TextBoxReady = false;
            this.Logs = logs;
            if (this.Logs != null)
                this.Logs.HandleCreated += (object? s, EventArgs e) =>
                {
                    this.TextBoxReady = true;
                    textBoxChanged.Invoke();
                };
        }
        public void NewRichTextBox(RichTextBox? logs, params LogsRestoreMethod[] restoreMethods)
        {
            string[] lines = this.Restore(restoreMethods);

            NewRichTextBox(logs);

            Display(LogHandler.UnpackLines(lines));
        }
        private string[] Restore(LogsRestoreMethod[] restoreMethods)
        {
            string[] lines = [];

            foreach (LogsRestoreMethod restoreMethod in restoreMethods)
            {
                bool restored = false;

                switch (restoreMethod)
                {
                    case LogsRestoreMethod.LogFile:
                        {
                            try
                            {
                                // Closing alreay open FileStream that doesn't support reading
                                this.Writer.Dispose();
                                this.FS.Dispose();

                                // Reading
                                lines = File.ReadLines(this.FilePath).ToArray();

                                // Reopening a FileStream to be able to write to the file
                                this.FS = File.Open(this.FilePath, FileMode.Open);
                                this.Writer = new StreamWriter(this.FS);
                                restored = true;
                            }
                            catch { }
                            break;
                        }
                    case LogsRestoreMethod.TextBox:
                        {
                            if (this.Logs == null)
                                break;

                            lines = this.Logs.Text.Split('\n');
                            restored = true;
                            break;
                        }
                    case LogsRestoreMethod.ToDisplay:
                        {
                            if (this.ToDisplay.Length < 1)
                                break;

                            lines = this.ToDisplay.ToString().Split('\n');
                            restored = true;
                            break;
                        }
                }

                if (restored)
                    break;
            }

            return lines;
        }
        private static Tuple<int, string, LogType, string, string>[] UnpackLines(string data)
        {
            string[] lines = Utils.SplitFull(data, '\n');

            return LogHandler.UnpackLines(lines);
        }
        private static Tuple<int, string, LogType, string, string>[] UnpackLines(string[] lines)
        {
            List<Tuple<int, string, LogType, string, string>> unpacked = [];
            
            foreach (string line in lines)
            {
                unpacked.Add(LogHandler.UnpackLine(line));
            }

            return [.. unpacked];
        }
        private static Tuple<int, string, LogType, string, string> UnpackLine(string line)
        {
            try
            {
                if (line.Length < 1)
                    return new(1, "", LogType.None, "", "");

                string[] splitLine = line.Split(']');
                string p1 = string.Join(' ', splitLine[0][1..]);
                Console.WriteLine(string.Join(", ", splitLine));
                Console.WriteLine(splitLine.Length);
                string asd = splitLine[1];
                string[] p2s = asd[2..].Split('/');
                string th = p2s[0];
                string p2 = p2s[1];
                string p3 = string.Join(' ', splitLine[2..]);
                splitLine[^1] = p1;

                LogType type = p2 switch
                {
                    "None    " => LogType.None,
                    "Info    " => LogType.Info,
                    "OK      " => LogType.OK,
                    "Warning " => LogType.Warning,
                    "Error   " => LogType.Error,
                    "Critical" => LogType.Critical,
                    "Fatal   " => LogType.Fatal,
                    _ => LogType.None,
                };

                return new(0, th, type, p3, p1);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unpacking failed\nError: {ex}", "Unpacking Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new(-1, "", LogType.None, "", "");
            }
        }
        private Tuple<string, string> FormatMessage(LogType type, string message, string time, string thread)
        {
            string p1 = $"[{time}] ";
            string p2 = $"[{thread}/{type,-8}] {message}";

            return new(p1, p2);
        }
        private int Write(Tuple<int, string, LogType, string, string>[] lines)
        {
            foreach (Tuple<int, string, LogType, string, string> line in lines)
            {
                if (line.Item1 != 0)
                    continue;

                (string p1, string p2) = this.FormatMessage(line.Item3, line.Item4[1..], line.Item5, line.Item2);

                if (p1 != "")
                    Write(p1 + p2 + '\n');
            }
            return 0;
        }
        private int Write(string txt)
        {
            if (!WriteEnabled || !WriteToFile)
                return 0;

            try
            {
                lock (Lock)
                {
                    this.Writer.Write(txt);
                }
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to log.\nText: {txt}\nError: {ex}", "Log Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private void Display()
        {
            Tuple<int, string, LogType, string, string>[] unpacked = LogHandler.UnpackLines(this.ToDisplay.ToString());

            foreach (Tuple<int, string, LogType, string, string> line in unpacked)
            {
                if (line.Item1 != 0)
                    continue;

                Display(line.Item3, line.Item4, line.Item5, line.Item2);
            }
        }
        private int Display(Tuple<int, string, LogType, string, string>[] lines)
        {
            foreach (Tuple<int, string, LogType, string, string> line in lines)
            {
                if (line.Item1 != 0)
                    continue;

                Display(line.Item3, line.Item4, line.Item5, line.Item2);
            }
            return 0;
        }
        private int Display(LogType type, string message, string time, string thread)
        {
            (string p1, string p2) = this.FormatMessage(type, message, time, thread);

            if (p1 == "")
                return 0;

            try
            {
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
                MessageBox.Show($"Failed to log.\nMessage: {message}\nError: {ex}", "Log Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        public void NewRichTextBox(RichTextBox? logs, string path)
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            this.Logs = logs;
        }
        public int New(LogType type, string message, string thread = "Main")
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            if (!message.EndsWith('\n'))
                message += '\n';

            return MakeLog(type, message, thread);
        }
        private int AppendText(string text, Color color)
        {
            if (this.Logs == null)
                return 1;
            else if (!WriteEnabled || !WriteToTextBox)
                return 0;

            this.Logs.Invoke(new Action(() =>
            {
                this.Logs.SelectionStart = this.Logs.TextLength;
                this.Logs.SelectionLength = 0;
                this.Logs.SelectionColor = color;
                this.Logs.AppendText(text);
                this.Logs.SelectionColor = this.Logs.ForeColor;
            }));
            return 0;
        }

        private int MakeLog(LogType type, string message, string thread)
        {
            (string p1, string p2) = this.FormatMessage(type, message, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), thread);

            if (p1 == "")
                return 0;

            try
            {
                lock (Lock)
                {
                    Task tk = Task.Run(async () => { await this.Writer.WriteAsync(p1 + p2); });

                    int ret = UseAppend(p1, p2, type);

                    tk.Wait();
                    return ret;
                }
            }
            catch (Exception ex)
            {
                if (this.Logs != null)
                    MessageBox.Show($"Failed to log.\nMessage: {message}\nError: {ex}", "Log Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
        private int UseAppend(string p1, string p2, LogType type)
        {
            if (!TextBoxReady)
            {
                this.ToDisplay.Append(p1);
                this.ToDisplay.Append(p2);
                //this.ToDisplay.Append('\n');
                return 0;
            }

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

            if ((ret = AppendText(p2, color)) != 0)
                return ret;
            return 0;
        }
    }
}
