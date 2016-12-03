using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.CoR
{
    class OpenFileHandler : ChainOfResponsability
    {
        protected override bool TryHandle(List<Key> keys, MainWindow window)
        {
            if (keys.Contains(Key.LeftCtrl) && keys.Contains(Key.O)) {

                // TODO : open file

                return true;
            }

            return false;
        }
    }

}
