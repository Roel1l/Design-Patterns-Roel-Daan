using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DPA_Musicsheets.MusicObjects;
using PSAMControlLibrary;
using PSAMWPFControlLibrary;
using DPA_Musicsheets.MusicObjects.Symbols;
using System.Windows.Controls;

namespace DPA_Musicsheets
{
    interface IWrapper
    {
        void draw(ScrollViewer scrollViewer, TrackObject track);
    }
    class Wrapper : IWrapper
    {
        private IncipitViewerWPF staff;

        private void createStaff()
        {
            staff = new IncipitViewerWPF();
            staff.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            staff.Width = 500;
            staff.Height = 57;
        }
        public void draw(ScrollViewer scrollViewer, TrackObject track)
        {
            createStaff();  

            staff.ClearMusicalIncipit();
            staff.Width = 500;
            staff.AddMusicalSymbol(new Clef(ClefType.GClef, 2));

            double maatvol = 0;
            int a = 0;

            if (track.notes.Count > 2)
            {
                foreach (ISymbol symbol in track.notes)
                {
                    if(symbol is NoteSymbol)
                    {
                        NoteSymbol note = (NoteSymbol)symbol;
                        a = note.absoluteTicks >= 16128 && track.timeSignature.Count > 1 ? 1 : 0;
                        if (maatvol >= track.timeSignature[a][1])
                        {
                            staff.AddMusicalSymbol(new Barline());
                            maatvol = 0;
                        }

                        maatvol += note.nootduur;
                    }

                    staff.AddMusicalSymbol(getSymbol(symbol));
                    staff.Width += 30;
                }
            }

            scrollViewer.Content = staff;
        }

        private MusicalSymbol getSymbol(ISymbol symbol)
        {
            if(symbol is MaatStreepSymbol)
            {
                return new Barline();
            }
            else if (symbol is TimeSignatureSymbol)
            {
                TimeSignatureSymbol timeSignatureObject = (TimeSignatureSymbol)symbol;
                return new TimeSignature(TimeSignatureType.Numbers, (uint)timeSignatureObject.timeSignature[0], (uint)timeSignatureObject.timeSignature[1]);
            }
            else if (symbol is RestSymbol)
            {
                RestSymbol restObject = (RestSymbol)symbol;
                return new Rest(noteLengthToMusicalSymbolDuration((int)restObject.lengte)) { NumberOfDots = restObject.punt };
            }
            else 
            {
                NoteSymbol noteObject = (NoteSymbol)symbol;
                return new Note(noteObject.toonHoogte, noteObject.kruisMol, noteObject.octaaf - 1, noteLengthToMusicalSymbolDuration((int)noteObject.lengte), NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single }) { NumberOfDots = noteObject.punt };
            }
        }

        private MusicalSymbolDuration noteLengthToMusicalSymbolDuration(int length)
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
