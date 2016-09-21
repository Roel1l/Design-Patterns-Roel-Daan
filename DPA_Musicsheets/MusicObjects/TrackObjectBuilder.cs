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

        public void buildMidiToObjectTrack(string trackName, int[] timeSignature, string tempo, int ticksperBeat, List<Tuple<ChannelMessage, MidiEvent>> notes)
        {
            TrackObject track = new TrackObject();
            track.trackName = trackName;
            track.timeSignature = timeSignature;
            track.tempo = Int32.Parse(tempo.Substring(7));
            track.ticksPerBeat = ticksperBeat;

            foreach (Tuple<ChannelMessage, MidiEvent> c in notes)
            {
                track.addNote(c.Item1, c.Item2);
            }

            tracks.Add(track);
        }

        public void buildLyToObjectTrack()
        {

        }
    }
}
