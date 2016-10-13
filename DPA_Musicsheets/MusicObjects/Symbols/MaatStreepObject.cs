using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSAMControlLibrary;

namespace DPA_Musicsheets.MusicObjects.Symbols
{
    class MaatStreepObject : Symbol
    {
        public override MusicalSymbol getSymbol()
        {
            return new Barline();
        }
    }
}
