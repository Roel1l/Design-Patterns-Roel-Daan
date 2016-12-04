﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace DPA_Musicsheets.CoR
{
    class InsertTimeThreeFourHandler : ChainOfResponsability
    {

        protected override bool TryHandle(List<Key> keys, MainWindow window)
        {
            if (keys.Contains(Key.LeftAlt) && keys.Contains(Key.T) && keys.Contains(Key.D3))
            {
                window.textBox.Text = window.textBox.Text.Insert(window.textBox.SelectionStart, @"\time 3/4");
                return true;
            }

            return false;
        }
    }
}