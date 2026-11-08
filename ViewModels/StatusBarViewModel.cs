using loadpdf3.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace loadpdf3.ViewModels
{
    public class StatusBarViewModel : ViewModelBase
    {
        private string _statusText = "ready???";
        private int _current;
        private int _max;
        public string StatusText 
        { 
            get => _statusText; 
            set { 
                _statusText = value; 
                OnPropertyChanged(nameof(StatusText)); 
            } 
        }
        public int Current { get => _current; set { _current = value; OnPropertyChanged(nameof(Current)); } }
        public int Max { get => _max; set { _max = value; OnPropertyChanged(nameof(Max)); } }

        public StatusBarViewModel(IPdfMessenger messenger)
        {
            messenger.ProgressChanged += message =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    StatusText = message.StatusText;
                    Current = message.CurrentPage;
                    Max = message.TotalPages;
                }));
            };
        }
    }
}
