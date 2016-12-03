using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.CoR
{
    interface MainHandler
    {
        MainHandler Next { get; set; }

        bool Handle(List<System.Windows.Input.Key> keys, MainWindow window);
    }
}
