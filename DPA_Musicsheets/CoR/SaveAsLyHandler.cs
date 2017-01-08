using DPA_Musicsheets.CoR.Command;
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
        private SaveAsLyCommand lyCommand = new SaveAsLyCommand();
        protected override bool TryHandle(List<Key> keys, MainWindow window)
        {
            if (keys.Contains(Key.LeftCtrl) && keys.Contains(Key.S) && !keys.Contains(Key.P))
            {
                lyCommand.setWindow(window);
                lyCommand.execute();
                return true;
            }
            return false;
        }
    }
}
