using AutoNotify;
using System.ComponentModel;
using System.Threading;

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
