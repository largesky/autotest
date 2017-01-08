using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Mengsk.Device.Devices.UI
{
    /// <summary>
    /// Interaction logic for DeviceMethodCaller.xaml
    /// </summary>
    public partial class DeviceMethodCaller : Window
    {
        public DeviceViewModel DeviceViewModel { get; set; }

        public DeviceMethodCaller()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MethodInfo[] mis = this.DeviceViewModel.Device.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(mi => mi.Name.StartsWith("get_") == false && mi.Name.StartsWith("set_") == false).ToArray();
            this.cbbMethods.ItemsSource = mis;
            this.cbbMethods.SelectedIndex = mis.Length > 0 ? 0 : -1;
        }

        private void cbbMethods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MethodInfo mi = this.cbbMethods.SelectedItem as MethodInfo;

            if (mi == null)
            {
                return;
            }

            var parameters = mi.GetParameters().Select(p => new ParameterViewModel() { Value = "", Description = "This is description", Parameter = p }).ToArray();
            this.dgParameters.ItemsSource = parameters;
        }

        private void btnExecute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MethodInfo mi = this.cbbMethods.SelectedItem as MethodInfo;
                if (mi == null)
                {
                    MessageBox.Show("请选择一个函数，以进行测试");
                    return;
                }
                ParameterViewModel[] parameters = this.dgParameters.ItemsSource as ParameterViewModel[];
                if (parameters == null)
                {
                    MessageBox.Show("函数参数为空");
                    return;
                }

                object[] parameter = ParseParameterValue(parameters);
                DateTime start = DateTime.Now;
                object ret = mi.Invoke(this.DeviceViewModel.Device, parameter);

                this.tbMessage.AppendText(string.Format("{0}: {1} 返回值: {2}\n", start, mi.Name, (ret == null ? "无返回值" : ret.ToString())));
            }
            catch (Exception ex)
            {
                this.tbMessage.AppendText(ex.Message + Environment.NewLine + ex.StackTrace);
            }
        }

        private object ParseValue(string value, TypeCode tc)
        {
            if (tc == TypeCode.Boolean)
            {
                return Boolean.Parse(value);
            }
            else if (tc == TypeCode.Byte)
            {
                return byte.Parse(value);
            }
            else if (tc == TypeCode.Char)
            {
                return value[0];
            }
            else if (tc == TypeCode.DateTime)
            {
                return DateTime.Parse(value);
            }
            else if (tc == TypeCode.DBNull)
            {
                throw new NotImplementedException(tc.ToString());
            }
            else if (tc == TypeCode.Decimal)
            {
                return decimal.Parse(value);
            }
            else if (tc == TypeCode.Double)
            {
                return double.Parse(value);
            }
            else if (tc == TypeCode.Empty)
            {
                throw new NotImplementedException(tc.ToString());
            }
            else if (tc == TypeCode.Int16)
            {
                return Int16.Parse(value);
            }
            else if (tc == TypeCode.Int32)
            {
                return Int32.Parse(value);
            }
            else if (tc == TypeCode.Int64)
            {
                return Int64.Parse(value);
            }
            else if (tc == TypeCode.Object)
            {
                throw new NotImplementedException(tc.ToString());
            }
            else if (tc == TypeCode.SByte)
            {
                return sbyte.Parse(value);
            }
            else if (tc == TypeCode.Single)
            {
                return Single.Parse(value);
            }
            else if (tc == TypeCode.String)
            {
                return value;
            }
            else if (tc == TypeCode.UInt16)
            {
                return UInt16.Parse(value);
            }
            else if (tc == TypeCode.UInt32)
            {
                return UInt32.Parse(value);
            }
            else if (tc == TypeCode.UInt64)
            {
                return UInt64.Parse(value);
            }
            else
            {
                throw new NotImplementedException(tc.ToString());
            }
        }

        private object ParseParameterValue(string value, Type type)
        {
            TypeCode tc = Type.GetTypeCode(type);

            if (tc != TypeCode.Object)
            {
                return ParseValue(value, tc);
            }

            if (type.IsArray == false)
            {
                throw new NotImplementedException(type.FullName);
            }

            string[] values = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Array array = Array.CreateInstance(type, values.Length);

            for (int i = 0; i < array.Length; i++)
            {
                array.SetValue(ParseValue(value, tc), i);
            }

            return array;
        }

        private object[] ParseParameterValue(ParameterViewModel[] parameters)
        {
            return parameters.Select(p => ParseParameterValue(p.Value, p.Parameter.ParameterType)).ToArray();
        }


    }
}
