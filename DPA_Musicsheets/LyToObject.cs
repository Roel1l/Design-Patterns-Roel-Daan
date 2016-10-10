using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    class LyToObject
    {
        private string[] lilyPondContents;
        private enum contentType
        {
            none,
            repeat,
            alternativeBlok,
            alternative
        }

        public LyToObject(string path)
        {

            lilyPondContents = System.IO.File.ReadAllText(path).Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();

            for (int i = 0; i < lilyPondContents.Length; i++)
            {
                lilyPondContents[i] = lilyPondContents[i].Replace("\r\n", string.Empty);
            }
            readContent();
        }

        private void readContent()
        {
            string tempo = "error";
            string maatsoort = "error";
            string sleutel = "error";

            contentType type = contentType.none;

            // komen alle noten + maatstrepen in te staan
            List<NoteObject> notes = new List<NoteObject>();
            List<List<NoteObject>> repeats;
            List<List<NoteObject>> alternatives;

            for (int i = 0; i < lilyPondContents.Length; i++)
            {
                switch (lilyPondContents[i])
                {
                    case "\\tempo":
                        tempo = lilyPondContents[i + 1];
                        break;
                    case "\\time":
                        maatsoort = lilyPondContents[i + 1];
                        break;
                    case "\\repeat":
                        type = contentType.repeat;
                        repeats = new List<List<NoteObject>>();
                        break;
                    case "\\alternative":
                        type = contentType.alternativeBlok;
                        alternatives = new List<List<NoteObject>>();
                        break;
                    case "\\clef":
                        sleutel = lilyPondContents[i + 1];
                        break;
                    case "|":
                        notes.Add(createMaatStreep());
                        break;
                    case "{":
                        if (type == contentType.alternativeBlok)
                        {
                            type = contentType.alternative;
                        }
                        break;
                    case "}":
                        if (type == contentType.alternative)
                        {
                            type = contentType.alternativeBlok;
                        }
                        else
                        {
                            type = contentType.none;
                        }
                        break;
                    default:
                        // gebruik deze voor de (cashew)noten
                        if (type == contentType.repeat)
                        {

                        }
                        else if (type == contentType.alternative)
                        {

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
            return null;
        }

        private NoteObject createMaatStreep()
        {
            NoteObject maatStreep = new NoteObject();

            // maatstrepen zijn saai en hebben maar 1 veld dat ze belangrijk maakt
           // maatStreep.isMaatStreep = true;

            return maatStreep;
        }
    }
}
