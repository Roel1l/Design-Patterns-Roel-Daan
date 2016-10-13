using DPA_Musicsheets.MusicObjects.Symbols;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets.MusicObjects
{
    class TrackObjectBuilder
    {
        public List<TrackObject> tracks = new List<TrackObject>();

        public void buildMidiToObjectTrack(List<int[]> timeSignature, List<int> ticksperBeat, List<Tuple<ChannelMessage, MidiEvent>> notes)
        {
            TrackObject track = new TrackObject();
            track.timeSignature = timeSignature;
            track.ticksPerBeat = ticksperBeat;
            track.currTimeSignature = 0;

            bool addedFirstTimeSignature = false;
            bool addedSecondTimeSignature = timeSignature.Count > 1 ? false : true;

            foreach (Tuple<ChannelMessage, MidiEvent> c in notes)
            {
                if (c.Item2.AbsoluteTicks >= 16128 && !addedSecondTimeSignature)
                {
                    track.addSymbol(new TimeSignatureObject() { timeSignature = new int[] { timeSignature[1][0], timeSignature[1][1] } });
                    addedSecondTimeSignature = true;
                    track.currTimeSignature = 1;
                }
                else if(!addedFirstTimeSignature)
                {
                    track.addSymbol(new TimeSignatureObject() { timeSignature = new int[] { timeSignature[0][0], timeSignature[0][1] } });
                    addedFirstTimeSignature = true;
                }
                track.addMidiNote(c.Item1, c.Item2);
            }

            tracks.Add(track);
        }

        public void buildLyToObjectTrack(List<int[]> timeSignature, List<Symbol> notes)
        {
            TrackObject track = new TrackObject();
            track.timeSignature = timeSignature;

            foreach (Symbol s in notes)
            {
                track.addLyNote(s);
            }
            tracks.Add(track);
        }
    }
}
