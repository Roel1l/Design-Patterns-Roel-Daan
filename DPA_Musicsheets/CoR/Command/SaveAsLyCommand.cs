using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.CoR.Command
{
    class SaveAsLyCommand : BaseCommand
    {
        private MainWindow window;

        public void setWindow(MainWindow w)
        {
            window = w;
        }
        public void execute()
        {
            System.Windows.Forms.SaveFileDialog s = new System.Windows.Forms.SaveFileDialog();

            s.FileName = "sheetmusic1.ly";
            s.Filter = "Lilypond files (*.ly)|*.ly|All files (*.*)|*.*";

            if (s.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(s.FileName, window.textBox.Text);
            }
        }
    }
}
