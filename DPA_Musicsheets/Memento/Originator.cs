using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Memento
{
    class Originator
    {
        private String state;
        public void set(String state)
        {
            this.state = state;
        }

        public MementoObject saveToMemento()
        {
            return new MementoObject(state);
        }
        public void restoreFromMemento(MementoObject m)
        {
            state = m.getSavedState();
        }

        public String getState()
        {
            return state;
        }
    }
}
