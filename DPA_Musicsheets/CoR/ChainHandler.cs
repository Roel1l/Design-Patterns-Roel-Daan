using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DPA_Musicsheets.CoR
{
    class ChainHandler
    {
        private MainHandler _actionHandler { get; set; }

        public bool IncomingCommand(List<Key> keys, MainWindow window) {
            return _actionHandler.Handle(keys, window);
        }

        public void AddFirstHandler(MainHandler handler)
        {
            if (_actionHandler != null)
            {
                handler.Next = _actionHandler;
            }

            _actionHandler = handler;
        }

        public void AddLastHandler(MainHandler handler)
        {
            if (_actionHandler == null)
            {
                _actionHandler = handler;
            }
            else
            {
                var prev = _actionHandler;
                while (prev.Next != null)
                {
                    prev = prev.Next;
                }
                prev.Next = handler;
            }
        }

        public void RemoveHandler(MainHandler handler)
        {
            var current = _actionHandler;
            MainHandler prev = null;

            while (current != null && current != handler)
            {
                prev = current;
                current = current.Next;
            }

            if (current != null)
            {
                if (prev == null)
                {
                    _actionHandler = current.Next;
                }
                else
                {
                    prev.Next = current.Next;
                }
            }
        }
    }
}
