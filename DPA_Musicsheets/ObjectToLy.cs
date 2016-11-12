using DPA_Musicsheets.MusicObjects;
using DPA_Musicsheets.MusicObjects.Symbols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    class ObjectToLy
    {
        int prevOctaaf = 5;
        public string convert(TrackObject track)
        {
            string lilyPond = "\\relative c' { \n \\clef treble \n";

            for (int i = 0; i < track.notes.Count; i++)
            {
                if(track.notes[i] is NoteSymbol)
                {
                    NoteSymbol note = (NoteSymbol)track.notes[i];
                    lilyPond += note.toonHoogte.ToLower();
                    if (note.kruisMol > 0) lilyPond += "is";
                    if (note.kruisMol < 0) lilyPond += "es";
                    for(int o = prevOctaaf; o < note.octaaf; o++) lilyPond += "'";
                    for (int o = prevOctaaf; o > note.octaaf; o--) lilyPond += ",";
                    lilyPond += note.lengte;
                    if (note.punt == 1) lilyPond += ".";
                    prevOctaaf = note.octaaf;
                    lilyPond += " ";
                }
                if (track.notes[i] is RestSymbol)
                {
                    RestSymbol rest = (RestSymbol)track.notes[i];
                    lilyPond += "r" + rest.lengte;
                    if (rest.punt == 1) lilyPond += ".";
                    lilyPond += " ";
                }
                if (track.notes[i] is MaatStreepSymbol)
                {
                    lilyPond += "|\n";
                }
                if(track.notes[i] is TimeSignatureSymbol)
                {
                    TimeSignatureSymbol ts = (TimeSignatureSymbol)track.notes[i];
                    lilyPond += "\\time " + ts.timeSignature[0] + "/" + ts.timeSignature[1] + " \n";
                }
                if (track.notes[i] is TempoSymbol)
                {
                    TempoSymbol tempoSymbol = (TempoSymbol)track.notes[i];
                    lilyPond += "\\tempo " + tempoSymbol.tempo[0] + "=" + tempoSymbol.tempo[1] + " \n";
                }
               // lilyPond += (i % 5) == 0 ? "\n" : string.Empty;
            }
            lilyPond += "\n}";
            return lilyPond;
        }
    }
}
