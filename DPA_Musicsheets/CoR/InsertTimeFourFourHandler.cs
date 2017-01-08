using DPA_Musicsheets.CoR.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPA_Musicsheets.CoR
{
    class InsertTimeFourFourHandler : ChainOfResponsability
    {
        private InsertTimeFourFourCommand fourCommand = new InsertTimeFourFourCommand();
        protected override bool TryHandle(List<Key> keys, MainWindow window)
        {
            if (keys.Contains(Key.LeftAlt) && keys.Contains(Key.T) || keys.Contains(Key.LeftAlt) && keys.Contains(Key.T) && keys.Contains(Key.D4))
            {
                if (!keys.Contains(Key.D6) && !keys.Contains(Key.D3))
                {
                    fourCommand.setWindow(window);
                    fourCommand.execute();
                    return true;
                }
            }

            return false;
        }
    }
}
