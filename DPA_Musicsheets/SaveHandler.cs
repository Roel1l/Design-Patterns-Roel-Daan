using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    class SaveHandler : ChainOfResponsability
    {
        public override bool Handle(List<System.Windows.Input.Key> keys, MainWindow window)
        {
            if (keys.Contains(System.Windows.Input.Key.LeftCtrl))
            {
                if (keys.Contains(System.Windows.Input.Key.S))
                {
                    if (keys.Contains(System.Windows.Input.Key.P))
                    {
                        return SaveAsPDF(window);
                    }
                    return SaveAsLY(window);
                }
                else if (keys.Contains(System.Windows.Input.Key.O)) {
                    return OpenFile(window);
                }
            }
            return false;
        }

        private bool OpenFile(MainWindow window)
        {
            // make ding
            return true;
        }

        private bool SaveAsLY(MainWindow window)
        {
            System.Windows.Forms.SaveFileDialog s = new System.Windows.Forms.SaveFileDialog();

            s.FileName = "sheetmusic1.ly";
            s.Filter = "Lilypond files (*.ly)|*.ly|All files (*.*)|*.*";

            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(s.FileName, window.textBox.Text);
            }
            return true;
        }

        private bool SaveAsPDF(MainWindow window)
        {
            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";

            string sourceFolder = @"c:\temp\";
            string targetFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string sourceFileName = string.Empty;
            string targetFileName = string.Empty;

            System.Windows.Forms.SaveFileDialog s = new System.Windows.Forms.SaveFileDialog();
            s.FileName = "sheetmusic1.pdf";
            s.Filter = "PDF (*.pdf)|*.pdf|All files (*.*)|*.*";

            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
            }

            sourceFileName = s.FileName;
            targetFileName = s.FileName;

            //var process = new Process
            //{
            //    StartInfo =
            //    {
            //        WorkingDirectory = sourceFolder,
            //        WindowStyle = ProcessWindowStyle.Hidden,
            //        // Arguments = String.Format("--pdf \"{0}{1}\"", sourceFolder, sourceFileName + ".ly"),
            //        Arguments = String.Format("--pdf \"{0}{1}\"", sourceFolder, sourceFileName),
            //        FileName = lilypondLocation
            //    }
            //};

            //process.Start();
            //while (!process.HasExited)
            //{
            //    /* Wait for exit */
            //}

            //File.Copy(sourceFolder + sourceFileName + ".pdf", targetFolder + targetFileName + ".pdf", true);
            return true;
        }
    }
}
