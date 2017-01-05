using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPA_Musicsheets.CoR
{
    class UndoHandler : ChainOfResponsability
    {

        DPA_Musicsheets.Memento.Caretaker caretaker = new DPA_Musicsheets.Memento.Caretaker();

        DPA_Musicsheets.Memento.Originator originator = new DPA_Musicsheets.Memento.Originator();

        protected override bool TryHandle(List<Key> keys, MainWindow window)
        {
            if (keys.Contains(Key.LeftCtrl) && keys.Contains(Key.Z))
            {
                window.textBox.Text = window.textBox.Text;
                return true;
            }

            return false;
        }
    }
}
