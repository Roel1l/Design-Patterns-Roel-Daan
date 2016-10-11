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

            foreach (Tuple<ChannelMessage, MidiEvent> c in notes)
            {
                track.addMidiNote(c.Item1, c.Item2);
            }

            tracks.Add(track);
        }

        public void buildLyToObjectTrack(List<Symbol> notes)
        {
            TrackObject track = new TrackObject();

            foreach(Symbol s in notes){
                track.addLyNote(s);
            }
        }
    }
}
