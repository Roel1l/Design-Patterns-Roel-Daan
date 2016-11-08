using DPA_Musicsheets.MusicObjects;
using DPA_Musicsheets.MusicObjects.Symbols;
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

        private Symbol latestNote;

        private TrackObjectBuilder trackObjectBuilder;

        private List<int[]> maatSoort;

        private enum contentType
        {
            none,
            alternativeBlok,
            alternative
        }

        public LyToObject(string content)
        {
            // relative c = octaaf 5
            latestNote = new NoteObject();
            latestNote.octaaf = 5;
            lilyPondContents = content.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            for (int i = 0; i < lilyPondContents.Length; i++)
            {
                lilyPondContents[i] = lilyPondContents[i].Replace("\r\n", string.Empty);
                lilyPondContents[i] = lilyPondContents[i].Replace(" ", string.Empty);
            }

            maatSoort = new List<int[]>();

            trackObjectBuilder = new TrackObjectBuilder();
            trackObjectBuilder.buildLyToObjectTrack(maatSoort, readContent());
        }

        internal TrackObjectBuilder TrackObjectBuilder
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        private List<Symbol> readContent()
        {
            int alternativeNr = 0;

            string tempo = "error";
            string maatsoort = "error";
            string sleutel = "error";

            contentType type = contentType.none;

            List<Symbol> notes = new List<Symbol>();                        // alle noten die geen onderdeel zijn van alternative
            List<List<Symbol>> alternatives = new List<List<Symbol>>();     // alle alternatives + noten per alternative

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
                        notes.Add(getProperMaatsoort(maatsoort));
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
                            alternatives.Add(new List<Symbol>());
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
                        if (lilyPondContents[i] != string.Empty)
                        {
                            if (type == contentType.alternative) alternatives[alternativeNr].Add(createNote(lilyPondContents[i]));
                            else notes.Add(createNote(lilyPondContents[i]));
                        }

                        break;
                }
            }
            return notes;

        }



        private TimeSignatureObject getProperMaatsoort(string maatsoort)
        {
            string s1 = maatsoort.Substring(0, maatsoort.IndexOf("/"));
            string s2 = maatsoort.Substring(maatsoort.IndexOf("/") + 1);


            int[] i = { Int32.Parse(s1), Int32.Parse(s2) };

            maatSoort.Add(i);

            return new TimeSignatureObject() { timeSignature = i};
        }



        private Symbol createNote(string note)
        {
            Symbol newNote;
            string toonhoogte = note.Substring(0, 1);       // a, b, c etc..

            if (toonhoogte == "r")
            {
                newNote = new RestObject();
                newNote.octaaf = latestNote.octaaf;
            }

            else
            {
               newNote = new NoteObject();
                newNote.toonHoogte = toonhoogte.ToUpper();
                newNote.octaaf = setCurrentOctaaf(note);
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



        private Symbol createMaatStreep()
        {
            MaatStreepObject maatStreep = new MaatStreepObject();

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

            string latestToonHoogte;



            if (latestNote.toonHoogte != null)

            {

                latestToonHoogte = latestNote.toonHoogte.ToUpper();

            }

            else

            {

                latestToonHoogte = "C";

            }



            string nieuwToonHoogte = note.Substring(0, 1).ToUpper();

            List<string> toonHoogtes = new List<string> { "A", "B", "C", "D", "E", "F", "G" };



            if (!toonHoogtes.Contains(nieuwToonHoogte) || !toonHoogtes.Contains(latestToonHoogte))
            {

                return nieuwOctaaf;

            }



            int rechtsOm = toonHoogtes.IndexOf(latestToonHoogte);

            bool rechtsWrap = false;

            int linksOm = toonHoogtes.IndexOf(latestToonHoogte);

            bool linksWrap = false;



            while (toonHoogtes[rechtsOm] != nieuwToonHoogte)

            {

                if (toonHoogtes[rechtsOm] == "G")

                {

                    rechtsOm = 0;

                    rechtsWrap = true;

                }

                else

                {

                    rechtsOm++;

                }

            }



            while (toonHoogtes[linksOm] != nieuwToonHoogte)

            {

                if (toonHoogtes[linksOm] == "A")

                {

                    linksOm = toonHoogtes.Count - 1;

                    linksWrap = true;

                }

                else

                {

                    linksOm--;

                }

            }



            if (rechtsOm < linksOm)

            {

                if (rechtsWrap)

                {

                    if (nieuwOctaaf <= 10)

                    {

                        nieuwOctaaf++;

                    }

                }

            }

            else if (linksOm < rechtsOm)

            {

                if (linksWrap)

                {

                    if (nieuwOctaaf >= 0)

                    {

                        nieuwOctaaf--;

                    }

                }

            }



            // handel de comma/apostrophe af

            if (note.Contains(","))

            {

                if (nieuwOctaaf >= 0)

                {

                    nieuwOctaaf--;

                }

            }

            else if (note.Contains("'"))

            {

                if (nieuwOctaaf <= 10)

                {

                    nieuwOctaaf++;

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