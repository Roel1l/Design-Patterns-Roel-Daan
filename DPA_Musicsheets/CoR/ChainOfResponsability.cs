using DPA_Musicsheets.CoR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Windows.Documents;
using System.Windows.Input;

namespace DPA_Musicsheets
{
    abstract class ChainOfResponsability : MainHandler
    {
        public MainHandler Next
        {
            get;
            set;
        }

        virtual public bool Handle(List<System.Windows.Input.Key> keys, MainWindow window)
        {
            if (TryHandle(keys, window))
            {
                return true;
            }
            else
            {
                return Next != null ? Next.Handle(keys, window) : false;
            }

        }

        protected abstract bool TryHandle(List<Key> keys, MainWindow window);
    }
}