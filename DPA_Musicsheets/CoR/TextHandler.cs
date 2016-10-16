using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DPA_Musicsheets
{
    class TextHandler : ChainOfResponsability
    {
        public override bool Handle(List<System.Windows.Input.Key> keys, MainWindow window)
        {
            if (keys.Contains(System.Windows.Input.Key.LeftAlt))
            {
                if (keys.Contains(System.Windows.Input.Key.C))
                {
                    return insertClefTreble(window.textBox);
                }
                else if (keys.Contains(System.Windows.Input.Key.S))
                {
                    return tempo(window.textBox);
                }
                else if (keys.Contains(System.Windows.Input.Key.B))
                {
                    return insertMissingBarlines(window.textBox);
                }
                else if (keys.Contains(System.Windows.Input.Key.T))
                {
                    if (keys.Contains(System.Windows.Input.Key.D4))
                    {
                        return insertTimeFourFour(window.textBox);
                    }
                    else if (keys.Contains(System.Windows.Input.Key.D3))
                    {
                        return insertTimeThreeFour(window.textBox);
                    }
                    else if (keys.Contains(System.Windows.Input.Key.D6))
                    {
                        return insertTimeSixEight(window.textBox);
                    }
                    return insertTimeFourFour(window.textBox);
                }
            }
            return false;
        }

        private bool insertClefTreble(TextBox textBox)
        {
            textBox.Text = textBox.Text.Insert(textBox.SelectionStart, @"\clef treble");
            return true;
        }

        private bool tempo(TextBox textBox)
        {
            textBox.Text = textBox.Text.Insert(textBox.SelectionStart, @"\tempo 4=120");
            return true;
        }

        private bool insertTimeFourFour(TextBox textBox)
        {
            textBox.Text = textBox.Text.Insert(textBox.SelectionStart, @"\time 4/4");
            return true;
        }

        private bool insertTimeThreeFour(TextBox textBox)
        {
            textBox.Text = textBox.Text.Insert(textBox.SelectionStart, @"\time 3/4");
            return true;
        }

        private bool insertTimeSixEight(TextBox textBox)
        {
            textBox.Text = textBox.Text.Insert(textBox.SelectionStart, @"\time 6/8");
            return true;
        }

        private bool insertMissingBarlines(TextBox textBox)
        {
            return true;
        }
    }
}
