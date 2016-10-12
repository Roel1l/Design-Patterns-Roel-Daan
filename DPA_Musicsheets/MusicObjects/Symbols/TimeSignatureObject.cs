using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.MusicObjects.Symbols
{
    class TimeSignatureObject : Symbol
    {
        public int[] timeSignature { get; set; }
        public override MusicalSymbol getSymbol()
        {
            return new TimeSignature(TimeSignatureType.Numbers, (uint)timeSignature[0], (uint)timeSignature[1]);
        }
    }
}
