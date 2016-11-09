using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.MusicObjects
{
    class RestSymbol : ISymbol
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

        public RestSymbol()
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
    }
}

