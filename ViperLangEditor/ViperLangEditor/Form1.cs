using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViperLangEditor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (Directory.Exists("Source"))
            {
                codeBox.Text = File.ReadAllText("source.viper");
            }
            else
            {
                Directory.CreateDirectory("Source");
            }
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str = string.Format(codeBox.Text); 
            File.WriteAllText("source.viper", str);
            if (File.Exists("ViperL.exe"))
            {
                MessageBox.Show("Check source.viper. If the code is in 1 line you gotta format it. Just Put A Space Everywhere so its in different lines. Then drag the source.viper in ViperL.exe and enjoy!");
            }
            else
            {
                MessageBox.Show("Error: compiler not found");
            }
        }
        
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string keywords = @"\b(vinclude|<|>|DeleteDirectory|CreateFile|DeleteFile|CreateDirectory|WriteToFileWithBytes|Print|PauseConsole)\b";
            string writtenCode = codeBox.Text;
            string comment = @"\b(#|//)\b";
            MatchCollection keyWordsCollection = Regex.Matches(writtenCode, keywords);
            string types = @"\b(Console)\b";
            MatchCollection typesCollection = Regex.Matches(writtenCode, types);
            string comments = @"\/\/.+?$|\/\*.+?\*\/";
            MatchCollection commentsCollection = Regex.Matches(writtenCode, comments, RegexOptions.Multiline);
            string strings = "\".+?\"";
            MatchCollection stringCollection = Regex.Matches(writtenCode, strings);

            int originalIndex = codeBox.SelectionStart;
            int originalLength = codeBox.SelectionLength;
            Color originalColor = Color.White;

            codeBox.SelectionStart = 0;
            codeBox.SelectionLength = codeBox.Text.Length;
            codeBox.SelectionColor = originalColor;

            foreach (Match match in keyWordsCollection)
            {
                codeBox.SelectionStart = match.Index;
                codeBox.SelectionLength = match.Length;
                codeBox.SelectionColor = Color.Magenta;
            }
            foreach (Match match in commentsCollection)
            {
                codeBox.SelectionStart = match.Index;
                codeBox.SelectionLength = match.Length;
                codeBox.SelectionColor = Color.DarkGreen;
            }

            codeBox.SelectionStart = originalIndex;
            codeBox.SelectionLength = originalLength;
            codeBox.SelectionColor = originalColor;
            codeBox.Focus();
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void codeBox_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}
