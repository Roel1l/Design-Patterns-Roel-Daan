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

        private int latestNoteOctaaf;

        private string latestNoteToonhoogte;

        private TrackObjectBuilder trackObjectBuilder;

        private List<int[]> maatSoort;

        private int repeatTimes;

        private enum contentType
        {
            none,
            alternativeBlok,
            alternative
        }

        public LyToObject(string content)
        {
            // relative c = octaaf 5
            latestNoteOctaaf = 5;
            content =content.Replace("\n", " ");
            content = content.Replace("\r", " ");

            lilyPondContents = content.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            for (int i = 0; i < lilyPondContents.Length; i++)
            {
                lilyPondContents[i] = lilyPondContents[i].Replace(" ", string.Empty);
            }

            maatSoort = new List<int[]>();

            trackObjectBuilder = new TrackObjectBuilder();
            trackObjectBuilder.buildLyToObjectTrack(maatSoort, readContent());
        }

        private List<ISymbol> readContent()
        {
            int alternativeNr = 0;

            string sleutel = "error";

            contentType type = contentType.none;

            List<ISymbol> notes = new List<ISymbol>();                        // alle noten die geen onderdeel zijn van alternative
            List<List<ISymbol>> alternatives = new List<List<ISymbol>>();     // alle alternatives + noten per alternative

            for (int i = 2; i < lilyPondContents.Length; i++)
            {
                switch (lilyPondContents[i])
                {
                   case "\\tempo":
                        string[] sTempo = lilyPondContents[i + 1].Split('=');
                        int[] iTempo = { Int32.Parse(sTempo[0]), Int32.Parse(sTempo[1]) };
                        notes.Add(new TempoSymbol { tempo = iTempo });
                        i++;
                        break;
                    case "\\time":
                        notes.Add(getProperMaatsoort(lilyPondContents[i + 1]));
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
                            alternatives.Add(new List<ISymbol>());
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
                    case "volta":
                        try
                        {
                            repeatTimes = Int32.Parse(lilyPondContents[i + 1]);
                        }
                        catch { }
                        break;

                    default:
                        // gebruik deze voor de (cashew)noten
                        if (lilyPondContents[i] != string.Empty && Regex.IsMatch(lilyPondContents[i], @"[a-zA-Z]") && Regex.IsMatch(lilyPondContents[i], @"[0-9]"))
                        {
                            if (type == contentType.alternative) alternatives[alternativeNr].Add(createNote(lilyPondContents[i]));
                            else notes.Add(createNote(lilyPondContents[i]));
                        }

                        break;
                }
            }
            return notes;

        }

        private TimeSignatureSymbol getProperMaatsoort(string maatsoort)
        {
            string s1 = maatsoort.Substring(0, maatsoort.IndexOf("/"));
            string s2 = maatsoort.Substring(maatsoort.IndexOf("/") + 1);

            int[] i = { Int32.Parse(s1), Int32.Parse(s2) };
            maatSoort.Add(i);
            return new TimeSignatureSymbol() { timeSignature = i};
        }

        private ISymbol createNote(string note)
        {
            string toonhoogte = note.Substring(0, 1);       // a, b, c etc..

            if (toonhoogte == "r")
            {
                RestSymbol newNote = new RestSymbol();
                newNote.octaaf = latestNoteOctaaf;

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
                latestNoteOctaaf = newNote.octaaf;
                latestNoteToonhoogte = newNote.toonHoogte;
                return newNote;
            }
            else
            {
                NoteSymbol newNote = new NoteSymbol();
                newNote.toonHoogte = toonhoogte.ToUpper();
                newNote.octaaf = setCurrentOctaaf(note);
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
                latestNoteOctaaf = newNote.octaaf;
                latestNoteToonhoogte = newNote.toonHoogte;
                return newNote;
            }

        }



        private ISymbol createMaatStreep()
        {
            MaatStreepSymbol maatStreep = new MaatStreepSymbol();
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
            int nieuwOctaaf = latestNoteOctaaf;
            string latestToonHoogte;

            if (latestNoteToonhoogte != null && latestNoteToonhoogte != string.Empty)
            {
                latestToonHoogte = latestNoteToonhoogte.ToUpper();
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

            int highcommas = note.Count(x => x == '\'');
            nieuwOctaaf = nieuwOctaaf + highcommas;

            int commas = note.Count(x => x == ',');
            nieuwOctaaf = nieuwOctaaf - commas;

            return nieuwOctaaf;
        }



        public TrackObject getTrackObject()
        {
            return trackObjectBuilder.tracks[0];
        }

    }

}