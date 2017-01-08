using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.CoR.Command
{
    class SaveAsPDFCommand : BaseCommand
    {
        private MainWindow window;

        public void setWindow(MainWindow w)
        {
            window = w;
        }

        public void execute()
        {
            string lilypondLocation = @"C:\Program Files (x86)\LilyPond\usr\bin\lilypond.exe";

            System.Windows.Forms.SaveFileDialog s = new System.Windows.Forms.SaveFileDialog();
            s.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string sourceFolder = new FileInfo(window.txt_MidiFilePath.Text).Directory.FullName + "\\";
                string sourceFileName = window.txt_MidiFilePath.Text.Split('/').Last();
                sourceFileName = sourceFileName.Substring(0, sourceFileName.Length - 3);
                string targetFolder = new FileInfo(s.FileName).Directory.FullName + "\\";
                string targetFileName = s.FileName.Split('\\').Last();
                targetFileName = targetFileName.Substring(0, targetFileName.Length - 4);

                var process = new Process
                {
                    StartInfo =
                {
                    WorkingDirectory = sourceFolder,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Arguments = String.Format("--pdf \"{0}{1}\"", sourceFolder, sourceFileName + ".ly"),
                    FileName = lilypondLocation
                }
                };

                process.Start();
                while (!process.HasExited) { /* Wait for exit */ }

                try
                {
                    File.Copy(sourceFolder + sourceFileName + ".pdf", targetFolder + targetFileName + ".pdf", true);
                    File.Delete(sourceFolder + sourceFileName + ".pdf");
                }
                catch
                {
                    //something went wrong
                }

            }
        }
    }
}
