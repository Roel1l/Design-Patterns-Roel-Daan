using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.CoR.Command
{
    class TempoCommand : BaseCommand
    {
        private MainWindow window;

        public void setWindow(MainWindow w)
        {
            window = w;
        }
        public void execute()
        {
            window.textBox.Text = window.textBox.Text.Insert(window.textBox.SelectionStart, @"\tempo 4=120");
        }
    }
}
