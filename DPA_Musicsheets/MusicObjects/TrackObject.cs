using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.MusicObjects
{
    class TrackObject
    {
        public List<int[]> timeSignature { get; set; }
        public List<int> ticksPerBeat { get; set; }
        public List<ISymbol> notes { get; set; }
        public int currTimeSignature { get; set; }

        public TrackObject()
        {
            notes = new List<ISymbol>();
        }

        public void addMidiNote(ChannelMessage message, MidiEvent midiEvent)
        {
            if (message.Data2 == 90 && midiEvent.DeltaTicks > 0) //Rust
            {
                RestSymbol rest = new RestSymbol();
                rest.absoluteTicks = midiEvent.AbsoluteTicks;
                rest.octaaf = message.Data1 / 12;
                rest.setToonhoogte(message.Data1 % 12);
                notes.Add(rest);

                calculateNoteLength(midiEvent);
            }
            if (message.Data2 == 90) //Noot
            {
                NoteSymbol note = new NoteSymbol();
                note.absoluteTicks = midiEvent.AbsoluteTicks;
                note.octaaf = message.Data1 / 12;
                note.setToonhoogte(message.Data1 % 12);
                notes.Add(note);
            }


            if (message.Data2 == 0)
            {
                calculateNoteLength(midiEvent);
            }
        }

        public void addLyNote(ISymbol note)
        {
            notes.Add(note);
        }

        public void addSymbol(ISymbol symbol)
        {
            notes.Add(symbol);
        }

        private void calculateNoteLength(MidiEvent midiEvent)
        {
            if (notes[notes.Count - 1] is NoteSymbol)
            {
                NoteSymbol note = (NoteSymbol)notes[notes.Count - 1];

                note.nootduur = (midiEvent.DeltaTicks) / (double)ticksPerBeat[currTimeSignature];
                double percentageOfWholeNote = (1.0 / (double)timeSignature[0][1]) * note.nootduur;

                for (int noteLength = 32; noteLength >= 1; noteLength /= 2)
                {
                    double absoluteNoteLength = (1.0 / noteLength);

                    if (percentageOfWholeNote <= absoluteNoteLength)
                    {
                        note.lengte = noteLength;
                        notes[notes.Count - 1] = note;
                        return;
                    }
                    if (percentageOfWholeNote == 1.5 * absoluteNoteLength)
                    {
                        note.lengte = noteLength;
                        note.punt = 1;
                        notes[notes.Count - 1] = note;
                        return;
                    }
                }
            }
            else if (notes[notes.Count - 1] is RestSymbol)
            {
                RestSymbol note = (RestSymbol)notes[notes.Count - 1];

                note.nootduur = (midiEvent.DeltaTicks) / (double)ticksPerBeat[currTimeSignature];
                double percentageOfWholeNote = (1.0 / (double)timeSignature[0][1]) * note.nootduur;

                for (int noteLength = 32; noteLength >= 1; noteLength /= 2)
                {
                    double absoluteNoteLength = (1.0 / noteLength);

                    if (percentageOfWholeNote <= absoluteNoteLength)
                    {
                        note.lengte = noteLength;
                        notes[notes.Count - 1] = note;
                        return;
                    }
                    if (percentageOfWholeNote == 1.5 * absoluteNoteLength)
                    {
                        note.lengte = noteLength;
                        note.punt = 1;
                        notes[notes.Count - 1] = note;
                        return;
                    }
                }
            }
        }

    }
}
