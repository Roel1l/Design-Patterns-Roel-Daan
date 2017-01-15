using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPA_Musicsheets.CoR
{
    class TempoHandler : ChainOfResponsability
    {
        protected override bool TryHandle(List<Key> keys, MainWindow window)
        {
            if (keys.Contains(Key.LeftAlt) && keys.Contains(Key.S))
            {
                window.textBox.Text = window.textBox.Text.Insert(window.textBox.SelectionStart, @"\tempo 4=120");
                return true;
            }

            return false;
        }
    }
}
