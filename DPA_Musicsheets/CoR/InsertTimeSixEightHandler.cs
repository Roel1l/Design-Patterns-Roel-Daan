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
    class InsertTimeSixEightHandler : ChainOfResponsability
    {
        private InsertTimeSixEightCommand sixCommand = new InsertTimeSixEightCommand();
        protected override bool TryHandle(List<Key> keys, MainWindow window)
        {
            if (keys.Contains(Key.LeftAlt) && keys.Contains(Key.T) && keys.Contains(Key.D6))
            {
                sixCommand.setWindow(window);
                sixCommand.execute();
                return true;
            }

            return false;
        }
    }
}
