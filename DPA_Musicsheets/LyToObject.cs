using DPA_Musicsheets.MusicObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    class LyToObject
    {
        private string[] lilyPondContents;
        private NoteObject latestNote;
        private TrackObjectBuilder trackObjectBuilder;

        private enum contentType
        {
            none,
            alternativeBlok,
            alternative
        }


        public LyToObject(string path)
        {
            // relative c = octaaf 5
            latestNote = new NoteObject();
            latestNote.octaaf = 5;

            lilyPondContents = System.IO.File.ReadAllText(path).Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            for (int i = 0; i < lilyPondContents.Length; i++)
            {
                lilyPondContents[i] = lilyPondContents[i].Replace("\r\n", string.Empty);
            }
            readContent();

           
        }

        private void readContent()
        {
            int alternativeNr = 0;

            string tempo = "error";
            string maatsoort = "error";
            string sleutel = "error";

            contentType type = contentType.none;

            List<NoteObject> notes = new List<NoteObject>();                        // alle noten die geen onderdeel zijn van alternative
            List<List<NoteObject>> alternatives = new List<List<NoteObject>>();     // alle alternatives + noten per alternative

            for (int i = 2; i < lilyPondContents.Length; i++)
            {
                switch (lilyPondContents[i])
                {
                    case "\\tempo":
                        tempo = lilyPondContents[i + 1];
                        i++;
                        break;
                    case "\\time":
                        maatsoort = lilyPondContents[i + 1];
                        i++;
                        break;
                    case "\\repeat":
                        break;
                    case "\\alternative":
                        type = contentType.alternativeBlok;
                        break;
                    case "\\clef":
                        sleutel = lilyPondContents[i + 1];
                        i++;
                        break;
                    case "|":
                        if (type == contentType.alternative)
                        {
                            alternatives[alternativeNr].Add(createMaatStreep());
                        }
                        else
                        {
                            notes.Add(createMaatStreep());
                        }
                        break;
                    case "{":
                        if (type == contentType.alternativeBlok)
                        {
                            type = contentType.alternative;
                            alternatives.Add(new List<NoteObject>());
                        }
                        break;
                    case "}":
                        if (type == contentType.alternative)
                        {
                            type = contentType.alternativeBlok;
                            alternativeNr++;
                        }
                        else
                        {
                            type = contentType.none;
                        }
                        break;
                    default:
                        // gebruik deze voor de (cashew)noten
                        if (type == contentType.alternative)
                        {
                            alternatives[alternativeNr].Add(createNote(lilyPondContents[i]));
                        }
                        else
                        {
                            notes.Add(createNote(lilyPondContents[i]));
                        }
                        break;
                }
            }
        }

        private NoteObject createNote(string note)
        {
            NoteObject newNote = new NoteObject();

            newNote.toonHoogte = note.Substring(0, 1);       // a, b, c etc..

            if (newNote.toonHoogte != "r")
            {
                newNote.octaaf = setCurrentOctaaf(note);
            }
            else
            {
                newNote.octaaf = latestNote.octaaf;
            }

            newNote.kruisMol = addKruisMol(note);

            if (note.Substring(note.Length - 1, 1) == ".")
            {
                newNote.punt = 1;
            }

            try
            {
                newNote.lengte = Int32.Parse(Regex.Match(note, @"\d+").Value);
            }
            catch
            {
                newNote.lengte = 1;
            }

            latestNote = newNote;
            return newNote;
        }

        private NoteObject createMaatStreep()
        {
            NoteObject maatStreep = new NoteObject();

            maatStreep.isMaatStreep = true;

            return maatStreep;
        }

        private int addKruisMol(string note)
        {
            if (note.Contains("is"))
            {
                return 1;
            }
            else if (note.Contains("es") || note.Contains("s"))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        private int setCurrentOctaaf(string note)
        {
            int nieuwOctaaf = latestNote.octaaf;
            List<char> toonHoogtes = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' };
            char toonHoogte = note.Substring(0, 1).First();

            int octaafOmhoog = 0;
            int octaafOmlaag = toonHoogtes.Count - 1;

            // TODO : zoek uit hoe de juiste octaaf berekend kan worden
            // zoek dichtsbijzijnde toonhoogte en pas octaaf daarop aan
            while (toonHoogtes[octaafOmhoog] != toonHoogte)
            {
                octaafOmhoog++;
            }

            while (toonHoogtes[octaafOmlaag] != toonHoogte)
            {
                octaafOmlaag--;
            }

            if (octaafOmhoog <= octaafOmlaag)
            {

            }

            // handel de comma/apostrophe af
            if (note.Contains(","))
            {
                if (nieuwOctaaf >= 1)
                {
                    nieuwOctaaf = nieuwOctaaf - 1;
                }
            }
            else if (note.Contains("'"))
            {
                if (nieuwOctaaf < 8)
                {
                    nieuwOctaaf = nieuwOctaaf + 1;
                }
            }

            return nieuwOctaaf;
        }

        public TrackObject getTrackObject()
        {
            return trackObjectBuilder.tracks[0];
        }
    }
}