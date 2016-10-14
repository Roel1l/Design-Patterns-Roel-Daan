using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    class TextHandler : ChainOfResponsability
    {
        public override bool Handle(List<System.Windows.Input.Key> keys, MainWindow window)
        {
            return false;
        }
    }
}
