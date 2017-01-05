using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Memento
{
    class Caretaker
    {
        private List<MementoObject> savedStates = new List<MementoObject>();

        public void addMemento(MementoObject m) { savedStates.Add(m); }
        public MementoObject getMemento(int index) { return savedStates[index]; }
    }
}
