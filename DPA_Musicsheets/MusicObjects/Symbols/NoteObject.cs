using DPA_Musicsheets.MusicObjects;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    class NoteObject : Symbol
    {
        public override MusicalSymbol getSymbol()
        {
            return new Note(this.toonHoogte, this.kruisMol, this.octaaf - 1, noteLengthToMusicalSymbolDuration((int)this.lengte), NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single }) { NumberOfDots = this.punt };
        }

        public override MusicalSymbolDuration noteLengthToMusicalSymbolDuration(int length)
        {
            switch (length)
            {
                case 1:
                    return MusicalSymbolDuration.Whole;
                case 2:
                    return MusicalSymbolDuration.Half;
                case 4:
                    return MusicalSymbolDuration.Quarter;
                case 8:
                    return MusicalSymbolDuration.Eighth;
                case 16:
                    return MusicalSymbolDuration.Sixteenth;
                default:
                    return MusicalSymbolDuration.Unknown;
            }
        }
    }
}
