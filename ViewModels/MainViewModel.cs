using Microsoft.Win32;
using OxyPlot;
using poid.Commands;
using poid.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using static poid.Models.FourierWindows;

namespace poid.ViewModels
{
    public class MainViewModel : NotifyPropertyChangedEvent
    {
        #region Properties

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

        private List<DataPoint> _FilteredSignalInTheTimeDomain;
        public List<DataPoint> FilteredSignalInTheTimeDomain
        {
            get
            {
                return _FilteredSignalInTheTimeDomain;
            }
            private set
            {
                _FilteredSignalInTheTimeDomain = value;
                NotifyPropertyChanged("FilteredSignalInTheTimeDomain");
            }
        }

        private List<DataPoint> _FilteredSignalInTheFrequencyDomain;
        public List<DataPoint> FilteredSignalInTheFrequencyDomain
        {
            get
            {
                return _FilteredSignalInTheFrequencyDomain;
            }
            private set
            {
                _FilteredSignalInTheFrequencyDomain = value;
                NotifyPropertyChanged("FilteredSignalInTheFrequencyDomain");
            }
        }

        private int _SampleRate = 0;
        public int SampleRate
        {
            get
            {
                return _SampleRate;
            }
            set
            {
                _SampleRate = value;
                NotifyPropertyChanged("SampleRate");
            }
        }

        public ObservableCollection<WindowType> WindowTypes { get; } = new ObservableCollection<WindowType> { WindowType.Hamming, WindowType.Hanning, WindowType.Rectangular };

        public ObservableCollection<ZeroFillingMethod> ZeroFillingMethods { get; } = new ObservableCollection<ZeroFillingMethod> { ZeroFillingMethod.CausalFilter, ZeroFillingMethod.CauselessFilter };

        private WindowType _SelectedWindowType = WindowType.Hamming;
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

        private ZeroFillingMethod _SelectedZeroFillingMethod = ZeroFillingMethod.CausalFilter;
        public ZeroFillingMethod SelectedZeroFillingMethod
        {
            get
            {
                return _SelectedZeroFillingMethod;
            }
            set
            {
                _SelectedZeroFillingMethod = value;
                NotifyPropertyChanged("SelectedZeroFillingMethod");
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

        private string _HopSize = "64";
        public string HopSize
        {
            get
            {
                return _HopSize;
            }
            set
            {
                _HopSize = value;
                NotifyPropertyChanged("HopSize");
            }
        }

        private string _FilterLength = "63";
        public string FilterLength
        {
            get
            {
                return _FilterLength;
            }
            set
            {
                _FilterLength = value;
                NotifyPropertyChanged("FilterLength");
            }
        }

        private string _CutoffFrequency = "128";
        public string CutoffFrequency
        {
            get
            {
                return _CutoffFrequency;
            }
            set
            {
                _CutoffFrequency = value;
                NotifyPropertyChanged("CutoffFrequency");
            }
        }

        private string _FilterTimeInTheTimeDomain = "";
        public string FilterTimeInTheTimeDomain
        {
            get
            {
                return _FilterTimeInTheTimeDomain;
            }
            set
            {
                _FilterTimeInTheTimeDomain = value;
                NotifyPropertyChanged("FilterTimeInTheTimeDomain");
            }
        }

        private string _FilterTimeInTheFrequencyDomain = "";
        public string FilterTimeInTheFrequencyDomain
        {
            get
            {
                return _FilterTimeInTheFrequencyDomain;
            }
            set
            {
                _FilterTimeInTheFrequencyDomain = value;
                NotifyPropertyChanged("FilterTimeInTheFrequencyDomain");
            }
        }

        private double[] _FilterResultInTheTimeDomain;
        public double[] FilterResultInTheTimeDomain
        {
            get
            {
                return _FilterResultInTheTimeDomain;
            }
            set
            {
                _FilterResultInTheTimeDomain = value;
                NotifyPropertyChanged("FilterResultInTheTimeDomain");
            }
        }

        private double[] _FilterResultInTheFrequencyDomain;
        public double[] FilterResultInTheFrequencyDomain
        {
            get
            {
                return _FilterResultInTheFrequencyDomain;
            }
            set
            {
                _FilterResultInTheFrequencyDomain = value;
                NotifyPropertyChanged("FilterResultInTheFrequencyDomain");
            }
        }

        private bool _ShowResultInTheTimeDomain = true;
        public bool ShowResultInTheTimeDomain
        {
            get
            {
                return _ShowResultInTheTimeDomain;
            }
            set
            {
                _ShowResultInTheTimeDomain = value;
                NotifyPropertyChanged("ShowResultInTheTimeDomain");
            }
        }

        private bool _ShowResultInTheFrequencyDomain = false;
        public bool ShowResultInTheFrequencyDomain
        {
            get
            {
                return _ShowResultInTheFrequencyDomain;
            }
            set
            {
                _ShowResultInTheFrequencyDomain = value;
                NotifyPropertyChanged("ShowResultInTheFrequencyDomain");
            }
        }

        #endregion

        #region Constuctors

        public MainViewModel()
        {
            this.InitializeCommands();
            this.InitializeEventListeners();
        }

        #endregion

        #region Initializers

        private void InitializeCommands()
        {
            this._LoadFile = new RelayCommand(this.LoadFile);
            this._FilterWithFiniteImpluseResponseInTheTimeDomain = new RelayCommand(o => this.SignalData != null, this.FilterWithFiniteImpluseResponseInTheTimeDomain);
            this._FilterWithFiniteImpluseResponseInTheFrequencyDomain = new RelayCommand(o => this.SignalData != null, this.FilterWithFiniteImpluseResponseInTheFrequencyDomain);
            this._ShowResultInTimeDomain = new RelayCommand(this.ShowResultInTimeDomain);
            this._ShowResultInFrequencyDomain = new RelayCommand(this.ShowResultInFrequencyDomain);
        }

        private void InitializeEventListeners()
        {
            this.PropertyChanged += this.HandleSignalDataChanged;
            this.PropertyChanged += this.HandleFilterResultInTheTimeDomainChanged;
            this.PropertyChanged += this.HandleFilterResultInTheFrequencyDomainChanged;
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
                    this.SampleRate = SignalData.FormatChunk.SampleRate;
                    this.FilterTimeInTheTimeDomain = null;
                    this.FilterTimeInTheFrequencyDomain = null;
                    this.FilterResultInTheTimeDomain = null;
                    this.FilterResultInTheFrequencyDomain = null;
                }
                else
                {
                    this.FileName = null;
                    this.Signal = null;
                    this.SampleRate = 0;
                }
            }
        }

        private void HandleFilterResultInTheTimeDomainChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "FilterResultInTheTimeDomain")
            {
                if (this.FilterResultInTheTimeDomain != null)
                {
                    List<DataPoint> signal = new List<DataPoint>();
                    for (int i = 0; i < this.FilterResultInTheTimeDomain.Length; i++)
                    {
                        signal.Add(new DataPoint(i, this.FilterResultInTheTimeDomain[i]));
                    }
                    this.FilteredSignalInTheTimeDomain = signal;
                }
                else
                {
                    this.FilteredSignalInTheTimeDomain = null;
                }
            }
        }

        private void HandleFilterResultInTheFrequencyDomainChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "FilterResultInTheFrequencyDomain")
            {
                if (this.FilterResultInTheFrequencyDomain != null)
                {
                    List<DataPoint> signal = new List<DataPoint>();
                    for (int i = 0; i < this.FilterResultInTheFrequencyDomain.Length; i++)
                    {
                        signal.Add(new DataPoint(i, this.FilterResultInTheFrequencyDomain[i]));
                    }
                    this.FilteredSignalInTheFrequencyDomain = signal;
                }
                else
                {
                    this.FilteredSignalInTheFrequencyDomain = null;
                }
            }
        }

        #endregion

        #region Commands

        public ICommand _LoadFile { get; private set; }

        public ICommand _FilterWithFiniteImpluseResponseInTheTimeDomain { get; private set; }

        public ICommand _FilterWithFiniteImpluseResponseInTheFrequencyDomain { get; private set; }

        public ICommand _ShowResultInTimeDomain { get; private set; }

        public ICommand _ShowResultInFrequencyDomain { get; private set; }

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

        #region Operations

        private void FilterWithFiniteImpluseResponseInTheTimeDomain(object o)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                int L = int.Parse(this.FilterLength);
                int Fc = int.Parse(this.CutoffFrequency);
                int Fs = this.SampleRate;
                double[] filter = FilterWithFiniteImpulseResponse.GetFilterValues(Fc, Fs, L);
                double[] smoothedFilter = FourierWindows.MultiplyByWindowFunction(filter, this.SelectedWindowType);
                double[] result = FilterWithFiniteImpulseResponse.FilterInTheTimeDomain(this.SignalData.Samples, smoothedFilter);
                double[] clearResult = FilterWithFiniteImpulseResponse.GetResultInTheTimeDomain(result, L);
                this.FilterResultInTheTimeDomain = result;
            }
            catch (Exception e)
            {
                Notify.Error(e.Message);
            }

            sw.Stop();
            this.FilterTimeInTheTimeDomain = sw.ElapsedMilliseconds.ToString();
        }

        private void FilterWithFiniteImpluseResponseInTheFrequencyDomain(object o)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                int L = int.Parse(this.FilterLength);
                int Fc = int.Parse(this.CutoffFrequency);
                int Fs = this.SampleRate;
                double[] filter = FilterWithFiniteImpulseResponse.GetFilterValues(Fc, Fs, L);
                double[] smoothedFilter = FourierWindows.MultiplyByWindowFunction(filter, this.SelectedWindowType);

                int M = int.Parse(this.WindowSize);
                double[][] sampleWindows = FourierWindows.SplitSamplesToWindows(this.SignalData.Samples, M);

                int N = M + L - 1;
                double[] filterWithZeros = FourierWindows.FillWithZeros(smoothedFilter, N, this.SelectedZeroFillingMethod);
                List<Complex> filterFFT = FourierTransform.FFT(filterWithZeros);

                double[] result = new double[this.SignalData.Samples.Length];
                for (int i = 0; i < sampleWindows.Length; i++)
                {
                    double[] samples = sampleWindows[i];
                    double[] samplesWithZeros = FourierWindows.FillWithZeros(samples, N, this.SelectedZeroFillingMethod);
                    List<Complex> samplesFFT = FourierTransform.FFT(samplesWithZeros);

                    for (int j = 0; j < samplesFFT.Count; j++)
                    {
                        samplesFFT[j] = Complex.Multiply(samplesFFT[j], filterFFT[j]);
                    }

                    double[] samplesAfterFilter = FourierTransform.IFFT(samplesFFT);
                    double[] clearResult = FourierWindows.GetResultWithoutZeros(samplesAfterFilter, M, this.SelectedZeroFillingMethod);
                    Array.Copy(clearResult, 0, result, i * M, M);
                }

                this.FilterResultInTheFrequencyDomain = result;
            }
            catch (Exception e)
            {
                Notify.Error(e.Message);
            }

            sw.Stop();
            this.FilterTimeInTheFrequencyDomain = sw.ElapsedMilliseconds.ToString();
        }

        private void ShowResultInTimeDomain(object o)
        {
            this.ShowResultInTheTimeDomain = true;
            this.ShowResultInTheFrequencyDomain = false;
        }

        private void ShowResultInFrequencyDomain(object o)
        {
            this.ShowResultInTheTimeDomain = false;
            this.ShowResultInTheFrequencyDomain = true;
        }

        #endregion
    }
}
