using DPA_Musicsheets.MusicObjects;
using Microsoft.Win32;
using PSAMControlLibrary;
using Sanford.Multimedia.Midi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DPA_Musicsheets
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MidiPlayer _player;
        public ObservableCollection<MidiTrack> MidiTracks { get; private set; }

        private DateTime _now;
        private Timer _timer = new Timer();
        private bool _typed = false;

        // De OutputDevice is een midi device of het midikanaal van je PC.
        // Hierop gaan we audio streamen.
        // DeviceID 0 is je audio van je PC zelf.
        private OutputDevice _outputDevice = new OutputDevice(0);

        public MainWindow()
        {
            this.MidiTracks = new ObservableCollection<MidiTrack>();
            InitializeComponent();
            DataContext = MidiTracks;
            FillPSAMViewer();
            textBox.Visibility = Visibility.Hidden;
            timer();
            //notenbalk.LoadFromXmlFile("Resources/example.xml");
        }

        private void FillPSAMViewer()
        {
            //staff.ClearMusicalIncipit();

            //// Clef = sleutel
            //staff.AddMusicalSymbol(new Clef(ClefType.GClef, 2));
            //staff.AddMusicalSymbol(new TimeSignature(TimeSignatureType.Numbers, 4, 4));
            ///* 
            //    The first argument of Note constructor is a string representing one of the following names of steps: A, B, C, D, E, F, G. 
            //    The second argument is number of sharps (positive number) or flats (negative number) where 0 means no alteration. 
            //    The third argument is the number of an octave. 
            //    The next arguments are: duration of the note, stem direction and type of tie (NoteTieType.None if the note is not tied). 
            //    The last argument is a list of beams. If the note doesn't have any beams, it must still have that list with just one 
            //        element NoteBeamType.Single (even if duration of the note is greater than eighth). 
            //        To make it clear how beamlists work, let's try to add a group of two beamed sixteenths and eighth:
            //            Note s1 = new Note("A", 0, 4, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Start, NoteBeamType.Start});
            //            Note s2 = new Note("C", 1, 5, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Continue, NoteBeamType.End });
            //            Note e = new Note("D", 0, 5, MusicalSymbolDuration.Eighth, NoteStemDirection.Down, NoteTieType.None,new List<NoteBeamType>() { NoteBeamType.End });
            //            viewer.AddMusicalSymbol(s1);
            //            viewer.AddMusicalSymbol(s2);
            //            viewer.AddMusicalSymbol(e); 
            //*/

            //staff.AddMusicalSymbol(new Note("A", 0, 4, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Start, NoteBeamType.Start }));
            //staff.AddMusicalSymbol(new Note("C", 1, 5, MusicalSymbolDuration.Sixteenth, NoteStemDirection.Down, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Continue, NoteBeamType.End }));
            //staff.AddMusicalSymbol(new Note("D", 0, 5, MusicalSymbolDuration.Eighth, NoteStemDirection.Down, NoteTieType.Start, new List<NoteBeamType>() { NoteBeamType.End }));
            //staff.AddMusicalSymbol(new Barline());

            //staff.AddMusicalSymbol(new Note("D", 0, 5, MusicalSymbolDuration.Whole, NoteStemDirection.Down, NoteTieType.Stop, new List<NoteBeamType>() { NoteBeamType.Single }));
            //staff.AddMusicalSymbol(new Note("E", 0, 4, MusicalSymbolDuration.Quarter, NoteStemDirection.Up, NoteTieType.Start, new List<NoteBeamType>() { NoteBeamType.Single }) { NumberOfDots = 1 });
            //staff.AddMusicalSymbol(new Barline());

            //staff.AddMusicalSymbol(new Note("C", 0, 4, MusicalSymbolDuration.Half, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single }));
            //staff.AddMusicalSymbol(
            //    new Note("E", 0, 4, MusicalSymbolDuration.Half, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single })
            //    { IsChordElement = true });
            //staff.AddMusicalSymbol(
            //    new Note("G", 0, 4, MusicalSymbolDuration.Half, NoteStemDirection.Up, NoteTieType.None, new List<NoteBeamType>() { NoteBeamType.Single })
            //    { IsChordElement = true });
            //staff.AddMusicalSymbol(new Barline());

        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if(_player != null)
            {
                _player.Dispose();
            }

            _player = new MidiPlayer(_outputDevice);
            _player.Play(txt_MidiFilePath.Text);
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "LilyPond Files (.ly)|*ly|Midi Files(.mid)|*.mid" };
            if (openFileDialog.ShowDialog() == true)
            {
                txt_MidiFilePath.Text = openFileDialog.FileName;
            }            
        }

        private void textBox_textChanged(object sender, RoutedEventArgs e)
        {
            _typed = true;
            _now = DateTime.Now;
        }

        private void timer()
        {
            _timer.Elapsed += new ElapsedEventHandler(UpdateStaff);
            _timer.Interval = 1500;
            _timer.Enabled = true;
        }

        private void UpdateStaff(object source, ElapsedEventArgs e)
        {
            TimeSpan timeElapsed = DateTime.Now - _now;
            if (timeElapsed.TotalMilliseconds > 1500 && _typed)
            {
                Console.WriteLine("Update!");
                _typed = false;

                Application.Current.Dispatcher.Invoke(new Action(() => {
                    LyToObject lyToObject = new LyToObject(textBox.Text);
                    drawTrack(lyToObject.getTrackObject());
                }));

            }     
        }

        private void drawTrack(TrackObject track)
        {
            staff.ClearMusicalIncipit();
            staff.Width = 500;
            staff.AddMusicalSymbol(new Clef(ClefType.GClef, 2));

            double maatvol = 0;

            if (track.notes.Count > 2)
            {
                foreach (Symbol symbol in track.notes)
                {
                    int a = symbol.absoluteTicks >= 16128 && track.timeSignature.Count > 1 ? 1 : 0;
                    if (maatvol >= track.timeSignature[a][1])
                    {
                        staff.AddMusicalSymbol(new Barline());
                        maatvol = 0;
                    }

                    maatvol += symbol.nootduur;
                    staff.AddMusicalSymbol(symbol.getSymbol());
                    staff.Width += 30;
                }
            }
        }

        private void btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            if (_player != null)
                _player.Dispose();
        }

        private void btn_ShowContent_Click(object sender, RoutedEventArgs e)
        {
            string extension = txt_MidiFilePath.Text.Split('.').Last();

            if (extension == "mid")
            {
                ShowMidiTracks(MidiReader.ReadMidi(txt_MidiFilePath.Text));
                MidiToObject midiToObject = new MidiToObject(txt_MidiFilePath.Text);
                drawTrack(midiToObject.getTrackObject());
                textBox.Visibility = Visibility.Hidden;
                tabCtrl_MidiContent.Visibility = Visibility.Visible;
            }
            else if(extension == "ly")
            {
                textBox.Visibility = Visibility.Visible;
                tabCtrl_MidiContent.Visibility = Visibility.Hidden;
                textBox.Text = File.ReadAllText(txt_MidiFilePath.Text);
                LyToObject lyToObject = new LyToObject(textBox.Text);
                drawTrack(lyToObject.getTrackObject());
            }          
        }

        private void ShowMidiTracks(IEnumerable<MidiTrack> midiTracks)
        {
            MidiTracks.Clear();
            foreach (var midiTrack in midiTracks)
            {
                MidiTracks.Add(midiTrack);
            }

            tabCtrl_MidiContent.SelectedIndex = 0;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _outputDevice.Close();
            if (_player != null)
            {
                _player.Dispose();
            }
            if (_timer != null) _timer.Dispose();
        }
    }
}
