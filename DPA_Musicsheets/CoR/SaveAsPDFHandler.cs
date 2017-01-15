using DPA_Musicsheets.CoR.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.CoR
{
    class SaveAsPDFHandler : ChainOfResponsability
    {
        private SaveAsPDFCommand pdfCommand = new SaveAsPDFCommand();
        protected override bool TryHandle(List<Key> keys, MainWindow window)
        {
            if (keys.Contains(Key.LeftCtrl) && keys.Contains(Key.S) && keys.Contains(Key.P))
            {
                pdfCommand.setWindow(window);
                pdfCommand.execute();
                return true;
            }
            return false;
        }
    }
}
