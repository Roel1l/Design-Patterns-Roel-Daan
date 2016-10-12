using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.MusicObjects
{
    class RestObject : Symbol
    {
        public override MusicalSymbol getSymbol()
        {
            return new Rest(noteLengthToMusicalSymbolDuration((int)this.lengte)) { NumberOfDots = this.punt };
        }
    }
}

