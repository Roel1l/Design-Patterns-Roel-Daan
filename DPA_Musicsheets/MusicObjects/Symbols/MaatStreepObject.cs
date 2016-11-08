using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;

namespace DPA_Musicsheets.MusicObjects.Symbols
{
    class MaatStreepObject : ISymbol
    {
        public MusicalSymbol getSymbol()
        {
            return new Barline();
        }
    }
}
