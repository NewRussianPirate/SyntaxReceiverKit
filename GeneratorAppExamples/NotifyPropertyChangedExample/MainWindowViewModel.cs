using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoNotify;

namespace WpfApp1
{
    internal partial class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        [AutoNotify]
        private uint _counter = 0;

        public MainWindowViewModel()
        {
            new Thread(() =>
            {
                while(true)
                {
                    Thread.Sleep(1500);
                    Counter++;
                }
            }).Start();
        }
    }
}
