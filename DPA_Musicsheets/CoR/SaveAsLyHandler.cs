using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.CoR
{
    class SaveAsLyHandler : ChainOfResponsability
    {
        protected override bool TryHandle(List<Key> keys, MainWindow window)
        {
            if (keys.Contains(Key.LeftCtrl) && keys.Contains(Key.S) && !keys.Contains(Key.P))
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
            return false;
        }
    }
}
