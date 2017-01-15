using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPA_Musicsheets.CoR
{
    class MissingBarlinesHandler : ChainOfResponsability
    {
        protected override bool TryHandle(List<Key> keys, MainWindow window)
        {
            if (keys.Contains(Key.LeftAlt) && keys.Contains(Key.B))
            {
                window.textBox.Text = window.textBox.Text.Insert(window.textBox.SelectionStart, @"\clef treble");
                return true;
            }

            return false;
        }
    }
}
