using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.Memento
{
    class MementoObject
    {
        private String state;
        public MementoObject(String stateToSave) { state = stateToSave; }
        public String getSavedState() { return state; }
    }
}
