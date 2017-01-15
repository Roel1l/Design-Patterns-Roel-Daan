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
    class InsertClefTrebleHandler : ChainOfResponsability
    {
        private InsertClefTrebleCommand trebleCommand = new InsertClefTrebleCommand();
        protected override bool TryHandle(List<Key> keys, MainWindow window)
        {
            if (keys.Contains(Key.LeftAlt) && keys.Contains(Key.C))
            {
                trebleCommand.setWindow(window);
                trebleCommand.execute();
                return true;
            }

            return false;
        }
    }
}
