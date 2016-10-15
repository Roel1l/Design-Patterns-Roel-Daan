using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPA_Musicsheets
{
    class ChainOfResponsability
    {
       
        virtual public bool Handle(List<System.Windows.Input.Key> keys, MainWindow window) {
            SaveHandler _saveHandler = new SaveHandler();
            TextHandler _textHandler = new TextHandler();

            if (keys.Contains(System.Windows.Input.Key.LeftCtrl) && keys.Contains(System.Windows.Input.Key.LeftAlt)) {
                // als zowel ctrl als alt zijn ingedrukt mag er niets gebeuren
                return true;
            }
            else if (keys.Contains(System.Windows.Input.Key.LeftCtrl))
            {
                return _saveHandler.Handle(keys, window);
            }
            else if (keys.Contains(System.Windows.Input.Key.LeftAlt)) {
                return _textHandler.Handle(keys, window);
            }

            return true;
        }
    }
}