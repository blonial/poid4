using Microsoft.Win32;
using OxyPlot;
using poid.Commands;
using poid.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using static poid.Models.FourierWindows;

namespace poid.ViewModels
{
    public class MainViewModel : NotifyPropertyChangedEvent
    {
        #region Properties

        public ObservableCollection<WindowType> WindowTypes { get; } = new ObservableCollection<WindowType> { WindowType.Hamming, WindowType.Hanning, WindowType.Rectangular };

        private WindowType _SelectedWindowType;
        public WindowType SelectedWindowType
        {
            get
            {
                return _SelectedWindowType;
            }
            set
            {
                _SelectedWindowType = value;
                NotifyPropertyChanged("SelectedWindowType");
            }
        }

        private WavData _SignalData;
        public WavData SignalData
        {
            get
            {
                return _SignalData;
            }
            private set
            {
                _SignalData = value;
                NotifyPropertyChanged("SignalData");
            }
        }

        private string _FileName;
        public string FileName
        {
            get
            {
                return _FileName;
            }
            private set
            {
                _FileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        private List<DataPoint> _Signal;
        public List<DataPoint> Signal
        {
            get
            {
                return _Signal;
            }
            private set
            {
                _Signal = value;
                NotifyPropertyChanged("Signal");
            }
        }

        private string _WindowSize = "1024";
        public string WindowSize
        {
            get
            {
                return _WindowSize;
            }
            set
            {
                _WindowSize = value;
                NotifyPropertyChanged("WindowSize");
            }
        }

        #endregion

        #region Constuctors

        public MainViewModel()
        {
            this.InitializeCommands();
            this.InitializeWindowTypes();
            this.InitializeEventListeners();
        }

        #endregion

        #region Initializers

        private void InitializeCommands()
        {
            this._LoadFile = new RelayCommand(this.LoadFile);
        }

        private void InitializeWindowTypes()
        {
            this.SelectedWindowType = this.WindowTypes[0];
        }

        private void InitializeEventListeners()
        {
            this.PropertyChanged += this.HandleSignalDataChanged;
        }

        #endregion

        #region Event listeners

        private void HandleSignalDataChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "SignalData")
            {
                if (this.SignalData != null)
                {
                    List<DataPoint> signal = new List<DataPoint>();
                    for (int i = 0; i < this.SignalData.Samples.Length; i++)
                    {
                        signal.Add(new DataPoint(i, this.SignalData.Samples[i]));
                    }
                    this.Signal = signal;
                }
                else
                {
                    this.FileName = null;
                    this.Signal = null;
                }
            }
        }

        #endregion

        #region Commands

        public ICommand _LoadFile { get; private set; }

        #endregion

        #region Methods

        private void LoadFile(object o)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Browse WAV Files";
            openFileDialog.DefaultExt = "wav";
            openFileDialog.Filter = "WAV files (*.wav)|*.wav";

            if (openFileDialog.ShowDialog() == true)
            {
                this.SignalData = WavReader.ReadData(openFileDialog.FileName);
                this.FileName = openFileDialog.FileName;
                Notify.Info("WAV file loaded sucessfully!");
            }
        }

        #endregion
    }
}
