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

        public string trackName { get; set; }

        public int[] timeSignature { get; set; }

        public int tempo { get; set; }

        public int ticksPerBeat { get; set; }
        public List<Symbol> notes { get; set; }

        public TrackObject()
        {
            notes = new List<Symbol>();
        }

        private int lastNoteData2 = 1;
        private int lastNoteAbsoluteTicks = 0;

        public void addNote(ChannelMessage message, MidiEvent midiEvent)
        {
            Symbol note;


            if (message.Data2 == 90 && midiEvent.DeltaTicks > 0) //Rust
            {
                note = new RestObject();
                note.absoluteTicks = midiEvent.AbsoluteTicks;
                note.octaaf = message.Data1 / 12;
                note.setToonhoogte(message.Data1 % 12);
                notes.Add(note);
            }
            else if (message.Data2 == 90)
            {
                note = new NoteObject();
                note.absoluteTicks = midiEvent.AbsoluteTicks;
                note.octaaf = message.Data1 / 12;
                note.setToonhoogte(message.Data1 % 12);
                notes.Add(note);
            }
            

            if (message.Data2 == 0)
            {
                notes[notes.Count - 1].nootduur = ((double)midiEvent.AbsoluteTicks - (double)notes[notes.Count - 1].absoluteTicks) / (double)ticksPerBeat;
                double percentageOfWholeNote = (1.0 / (double)timeSignature[1]) * notes[notes.Count - 1].nootduur;

                for (int noteLength = 32; noteLength >= 1; noteLength /= 2)
                {
                    double absoluteNoteLength = (1.0 / noteLength);

                    if (percentageOfWholeNote <= absoluteNoteLength)
                    {
                        notes[notes.Count - 1].lengte = noteLength;
                        return;
                    }
                    if (percentageOfWholeNote == 1.5 * absoluteNoteLength)
                    {
                        notes[notes.Count - 1].lengte = noteLength / 3 * 2;
                        notes[notes.Count - 1].punt = 1;
                    }     
                }
            }
        }
      
    }
}
