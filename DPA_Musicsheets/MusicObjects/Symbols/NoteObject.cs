﻿using DPA_Musicsheets.MusicObjects;
using PSAMControlLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    class NoteObject : ISymbol
    {
        private List<string> toonHoogtes;
        public int kruisMol { get; set; } // '1' is kruis, '-1' is mol

        public int octaaf { get; set; }

        public double nootduur { get; set; }

        public double lengte { get; set; }

        public int punt { get; set; }

        public string toonHoogte { get; set; }

        public int absoluteTicks { get; set; }

        public bool isMaatStreep { get; set; }

        public NoteObject()
        {
            toonHoogtes = new List<string>();
            toonHoogtes.Add("C");
            toonHoogtes.Add("C#");
            toonHoogtes.Add("D");
            toonHoogtes.Add("D#");
            toonHoogtes.Add("E");
            toonHoogtes.Add("F");
            toonHoogtes.Add("F#");
            toonHoogtes.Add("G");
            toonHoogtes.Add("G#");
            toonHoogtes.Add("A");
            toonHoogtes.Add("A#");
            toonHoogtes.Add("B");
        }

        public void setToonhoogte(int keyCode)
        {
            this.toonHoogte = toonHoogtes[keyCode];
            if (this.toonHoogte.Contains("#")) this.kruisMol = 1;
        }

        public MusicalSymbolDuration noteLengthToMusicalSymbolDuration(int length)
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

        public MusicalSymbol getSymbol()
        {
            return new Note(this.toonHoogte, this.kruisMol, this.octaaf - 1, noteLengthToMusicalSymbolDuration((int)this.lengte), NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single }) { NumberOfDots = this.punt };
        }
    }
}
