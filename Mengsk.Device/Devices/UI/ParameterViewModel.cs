using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Mengsk.Device.Devices.UI
{
    public class ParameterViewModel : DependencyObject
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(ParameterViewModel));

        public string Value { get { return (string)this.GetValue(ValueProperty); } set { this.SetValue(ValueProperty, value); } }

        public string Description { get; set; }

        public ParameterInfo Parameter { get; set; }
    }
}
