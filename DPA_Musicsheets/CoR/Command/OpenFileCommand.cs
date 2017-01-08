using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.CoR.Command
{
    class OpenFileCommand : BaseCommand
    {
        private MainWindow window;

        public void setWindow(MainWindow w)
        {
            window = w;
        }
        public void execute()
        {
            // TODO
        }
    }
}
