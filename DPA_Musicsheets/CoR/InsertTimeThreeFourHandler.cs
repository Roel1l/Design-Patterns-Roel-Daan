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
    class InsertTimeThreeFourHandler : ChainOfResponsability
    {
        private InsertTimeThreeFourCommand threeCommand = new InsertTimeThreeFourCommand();
        protected override bool TryHandle(List<Key> keys, MainWindow window)
        {
            if (keys.Contains(Key.LeftAlt) && keys.Contains(Key.T) && keys.Contains(Key.D3))
            {
                threeCommand.setWindow(window);
                threeCommand.execute();
                return true;
            }

            return false;
        }
    }
}
