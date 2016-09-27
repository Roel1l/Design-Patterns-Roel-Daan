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
        public List<NoteObject> notes { get; set; }

        public TrackObject()
        {
            notes = new List<NoteObject>();
        }

        private int lastNoteData2 = 1;

        public void addNote(ChannelMessage message, MidiEvent midiEvent)
        {
            
            //rust bereken: vorige noot had message.data2 == 0 en huidige noot absoluteticks is later dan vorige noot

            //case: Rust
            if (lastNoteData2 == 0)
            {           
                if (midiEvent.AbsoluteTicks > notes[notes.Count - 1].absoluteTicks)
                {
                    NoteObject note = new NoteObject();
                    note.absoluteTicks = midiEvent.AbsoluteTicks;
                    note.octaaf = message.Data1 / 12;
                    note.setToonhoogte(message.Data1 % 12);
                    note.rust = true;
                    notes.Add(note);
                    lastNoteData2 = message.Data2;
                    return;
                }
            }

            lastNoteData2 = message.Data2;

            //case: normale Noot
            if (message.Data2 == 90)
            {
                NoteObject note = new NoteObject();
                note.absoluteTicks = midiEvent.AbsoluteTicks;
                note.octaaf = message.Data1 / 12;
                note.setToonhoogte(message.Data1 % 12);
                notes.Add(note);
            }
            else
            //case toonhoogte van vorige noot zetten
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
                        notes[notes.Count - 1].punt = true;
                    }     
                }
            }

           
        }
      
        public void setNoteDuur()
        {
            for (int i = 0; i < notes.Count - 1; i++)
            {
                notes[i].nootduur = ((double)notes[i + 1].absoluteTicks - (double)notes[i].absoluteTicks) / (double)ticksPerBeat;
            }
        }
    }
}
