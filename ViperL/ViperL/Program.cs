using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ViperL
{
    class Program
    {
        public static StreamReader stream;
        public static WebClient client = new WebClient();
        public static BinaryWriter binary;
        static void Main(string[] args)
        {
            if (args[0].Length < 1)
            {
                Console.WriteLine("No .viper file found.");
            }
            else
            {
                if (args[0].Contains(".viper"))
                {
                    stream = new StreamReader(args[0]);
                    string content = stream.ReadToEnd();
                    if (content.Contains("vinclude <!viper_dev>"))
                    {
                        string[] parsed = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        foreach (var line in parsed)
                        {

                            // All Commands
                            /* WEB */
                            if (line.StartsWith("Download > "))
                            {
                                string url = line.Replace("Download > ", "");
                                client.DownloadFile(url, Environment.CurrentDirectory + "\\VIPER_OUTPUT.viperdata");
                            }
                            if (line.StartsWith("Upload > "))
                            {
                                string url = line.Replace("Upload > ", "");
                                client.UploadFile(url, "VIPER_OUTPUT_WEBUPLOAD.viperdata");
                            }
                            /* IO */
                            if (line.StartsWith("CreateFile > "))
                            {
                                string filename = line.Replace("CreateFile > ", "");
                                File.WriteAllText(filename, "");
                            }
                            if (line.StartsWith("CreateDirectory > "))
                            {
                                string dir = line.Replace("CreateDirectory > ", "");
                                Directory.CreateDirectory(dir);
                            }
                            if (line.StartsWith("DeleteFile > "))
                            {
                                string filename = line.Replace("DeleteFile > ", "");
                                File.Delete(filename);
                            }
                            if (line.StartsWith("DeleteDirectory > "))
                            {
                                string dir = line.Replace("DeleteDirectory > ", "");
                                Directory.Delete(dir, true);
                            }
                            if (line.StartsWith("WriteToFileWithBytes > "))
                            {
                                string file = File.ReadAllText("file.customfiledata");
                                string Replace = line.Replace("WriteToFileWithBytes > ", "");
                                string offset = File.ReadAllText("offset.customoffsetdata");
                                int intoffset = Convert.ToInt32(offset);
                                if (File.Exists(file))
                                {
                                    byte[] ReplaceByte = HexToByteArray(Replace);
                                    binary = new BinaryWriter((Stream)File.Open(file, FileMode.Open, FileAccess.ReadWrite));
                                    binary.BaseStream.Seek(intoffset, SeekOrigin.Begin);
                                    binary.Write(ReplaceByte);
                                    binary.Close();
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("ERROR: ViperWriterException: File Not Found.");
                                }
                            }
                            /* GENERAL SYS */
                            if (line.StartsWith("Print > "))
                            {
                                string msg = line.Replace("Print >", "");
                                Console.WriteLine(msg);
                            }
                            if (line.StartsWith("Delay > "))
                            {
                                string time = line.Replace("Delay > ", "");
                                string ms = time;
                                int timeI = Convert.ToInt32(ms);
                                Thread.Sleep(timeI);
                            }
                            if (line.StartsWith("PauseConsole"))
                            {
                                Console.ReadLine();
                            }
                            // UNFISHED
                            if (line.StartsWith("GetConsoleInput > "))
                            {
                                var input = Console.ReadLine();
                                var key = line.Replace("GetConsoleInput > ", "");
                                if (input == key)
                                {

                                }
                            }
                            // NOT ALL COLORS ARE CURRENTLY ADDED!
                            if (line.StartsWith("SetConsColorRed"))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            if (line.StartsWith("SetConsColorBlue"))
                            {
                                Console.ForegroundColor = ConsoleColor.Blue;
                            }
                            if (line.StartsWith("SetConsColorWhite"))
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            if (line.StartsWith("SetConsColorYellow"))
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                            }
                            if (line.StartsWith("SetConsColorGreen"))
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        FileInfo info = new FileInfo(args[0]);
                        Console.WriteLine($"ERROR: {info.Name} has no meaning of it's context. Try including the dev kit in the code: 'vinclude <!viper_dev>' ");
                        Console.ReadLine();
                    }
                }
            }
        }
        public static byte[] HexToByteArray(string input)
        {
            if (input.Contains(" "))
            {
                input = input.Replace(" ", "");
            }
            if (input.Length % 2 != 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "ERROR", input));
            }
            byte[] bytearray = new byte[input.Length / 2];
            for (int i = 0; i < bytearray.Length; i++)
            {
                string str = input.Substring(i * 2, 2);
                bytearray[i] = byte.Parse(str, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }
            return bytearray;
        }
    }
}
