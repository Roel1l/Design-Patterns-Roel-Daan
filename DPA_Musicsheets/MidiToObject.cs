using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sanford.Multimedia.Midi;
using DPA_Musicsheets.MusicObjects;

namespace DPA_Musicsheets
{
    class MidiToObject
    {
        private TrackObjectBuilder trackBuilder = new TrackObjectBuilder();

        public MidiToObject(String path)
        {
            var sequence = new Sequence();
            sequence.Load(path);

            List<Track> tracks = new List<Track>();

            for (int i = 0; i < sequence.Count; i++)
            {
                tracks.Add(sequence[i]);
            }

            List<int[]> timeSignatures = new List<int[]>();
            List<int> ticksPerBeats = new List<int>();
            List<Tuple<ChannelMessage, MidiEvent>> notes = new List<Tuple<ChannelMessage, MidiEvent>>();
            double noteLength;

            foreach (Track i in tracks)
            {
                #region
                foreach (MidiEvent midiEvent in i.Iterator())
                {
                    // ChannelMessages zijn de inhoudelijke messages.
                    if (midiEvent.MidiMessage.MessageType == MessageType.Channel)
                    {
                        var channelMessage = midiEvent.MidiMessage as ChannelMessage;
                        
                        if (channelMessage.Command == ChannelCommand.NoteOn || channelMessage.Command == ChannelCommand.NoteOff)
                        {
                            notes.Add(new Tuple<ChannelMessage, MidiEvent>(channelMessage, midiEvent));
                        }
                    }
                    // Meta zegt iets over de track zelf.
                    if (midiEvent.MidiMessage.MessageType == MessageType.Meta)
                    {
                        var metaMessage = midiEvent.MidiMessage as MetaMessage;
                        if (metaMessage.MetaType == MetaType.TimeSignature)
                        {
                            byte[] bytes = metaMessage.GetBytes();
                            int[] timesignature = { bytes[0], (int)Math.Pow(2, bytes[1]) };
                            timeSignatures.Add(timesignature);
                            noteLength = 1.0/ timesignature[1];
                            ticksPerBeats.Add((int)(sequence.Division * (noteLength / 0.25)));
                        }
                    }
                }
                #endregion
            }


            trackBuilder.buildMidiToObjectTrack(timeSignatures, ticksPerBeats, notes);
            //Je slaat nu beide timesignatures met hun tpb op. Hoe weet je wanneer de volgende timesignature ingaat?
        }

        public TrackObject getTrackObject()
        {
            return trackBuilder.tracks[0];
        }
    }
}
