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
        public List<Symbol> notes { get; set; }

        public TrackObject()
        {
            notes = new List<Symbol>();
        }

        public void addMidiNote(ChannelMessage message, MidiEvent midiEvent)
        {
            Symbol note;

            if (message.Data2 == 90 && midiEvent.DeltaTicks > 0) //Rust
            {
                note = new RestObject();
                note.absoluteTicks = midiEvent.AbsoluteTicks;
                note.octaaf = message.Data1 / 12;
                note.setToonhoogte(message.Data1 % 12);
                notes.Add(note);

                calculateNoteLength(midiEvent);
            }
            if (message.Data2 == 90) //Noot
            {
                note = new NoteObject();
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

        public void addLyNote(Symbol note)
        {
            notes.Add(note);
        }

        private void calculateNoteLength(MidiEvent midiEvent)
        {
            notes[notes.Count - 1].nootduur = (midiEvent.DeltaTicks) / (double)ticksPerBeat[0];
            double percentageOfWholeNote = (1.0 / (double)timeSignature[0][1]) * notes[notes.Count - 1].nootduur;

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
                    notes[notes.Count - 1].lengte = noteLength;
                    notes[notes.Count - 1].punt = 1;
                    return;
                }
            }
        }
      
    }
}
