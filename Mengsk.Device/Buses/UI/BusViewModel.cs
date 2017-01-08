using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Mengsk.Device.Buses.UI
{
    public class BusViewModel : DependencyObject
    {
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message", typeof(string), typeof(BusViewModel));
        public static readonly DependencyProperty StateProperty = DependencyProperty.Register("State", typeof(BusState), typeof(BusViewModel));
        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(BusViewModel));

        public IBus Bus { get; private set; }

        public string Message { get { return (string)this.GetValue(MessageProperty); } set { this.SetValue(MessageProperty, value); } }

        public BusState State
        {
            get { return (BusState)this.GetValue(StateProperty); }
            set
            {
                if (this.State == value)
                {
                    return;
                }
                this.SetValue(StateProperty, value);
                if (value == BusState.Error)
                {
                    this.Background = Brushes.Red;
                }
                else if (value == BusState.Writing || value == BusState.Reading)
                {
                    this.Background = Brushes.Yellow;
                }
                else if (value == BusState.Writed || value == BusState.Readed)
                {
                    this.Background = Brushes.Green;
                }
                else
                {
                    this.Background = null;
                }
            }
        }

        private Brush Background { get { return (Brush)this.GetValue(BackgroundProperty); } set { this.SetValue(BackgroundProperty, value); } }

        public BusViewModel(IBus bus)
        {
            this.Bus = bus;
            this.State = bus.State;
            this.Message = "";
        }
    }
}
