using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows;
using System.Collections.ObjectModel;
using PHUP_Windykacje.ViewModel;


namespace PHUP_Windykacje.View
{

    public class DecimalToStringConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(decimal))
                return null;
            if (!value.TryCast<string>(out _))
                return null;

            var res = "";
            CultureInfo daDK = CultureInfo.CreateSpecificCulture("pl-PL");
            res = String.Format(daDK, "{0:0.00}", UConv.ConvertTo<decimal>(value));
            return res;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value.GetType() != typeof(string))
                return 0;

            string str = value.ToString();
            if (str.Trim().In(new[] { ",", "." }))
            {
                str = "0,0";
            }

            if (str.TryCast<decimal>(out var res1))
            {
                return res1;
            }

            var arr = str.ToArray();
            var res = "";
            var szPNowackiHallmarkFound = false;
            var szPNowackiHallmarks = new[] { ',', '.' };
            foreach (var c in arr)
            {
                if (c.In(szPNowackiHallmarks))
                {
                    if (!szPNowackiHallmarkFound)
                    {
                        res += c;
                    }

                    szPNowackiHallmarkFound = true;
                }
                else
                {
                    if (c.In("0123456789".ToArray()))
                        res += c;
                }

            }

            var resDec = UConv.ConvertTo<decimal>(res);
            return resDec;
        }
    }






    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            DateTime d = (DateTime)value;
            string s = d.ToString("yyyy-MM-dd");
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = (string)value;
            validateDateString(ref s);
            try
            {
                DateTime d = System.Convert.ToDateTime(s);
                return d;
            }
            catch
            {
                MessageBox.Show("Podano złą datę", "Błąd daty", MessageBoxButton.OK, MessageBoxImage.Error);
                return new DateTime();
            }

        }

        private void validateDateString(ref string s)
        {
            s = s.Replace('/', '-');
            s = Regex.Replace(s, @"[^\-0-9]", "");
            if (s.Length != 10)
            {
                MessageBox.Show("Podano złą datę", "Błąd daty", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }




    public class YetAnotherDateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            DateTime d = (DateTime)value;
            string s = d == default(DateTime)? "" : d.ToString("yyyy-MM-dd");
            return s;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = (string)value;
            //validateDateString(ref s);
            try
            {
                if(string.IsNullOrEmpty(s))
                    return default(DateTime);
                DateTime d = System.Convert.ToDateTime(s);
                return d;
            }
            catch
            {
                //MessageBox.Show("Podano złą datę", "Błąd daty", MessageBoxButton.OK, MessageBoxImage.Error);
                return new DateTime();
            }

        }

        //private void validateDateString(ref string s)
        //{
        //    s = s.Replace('/', '-');
        //    s = Regex.Replace(s, @"[^\-0-9]", "");
        //    if (s.Length != 10)
        //    {
        //        MessageBox.Show("Podano złą datę", "Błąd daty", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}
    }




    //public class DFList2StringConv : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if(value == null)
    //            return string.Empty;

    //        if (value is ObservableCollection<VM_DzialanieDF>)
    //        {
    //            return string.Join(';', (value as ObservableCollection<VM_DzialanieDF>).Select(oc => oc.Opis));
    //        }
    //        return "";
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value == null)
    //            return new ObservableCollection<VM_DzialanieDF>();
    //        if (value is string)
    //        {
    //            var list = new List<VM_DzialanieDF>();

    //            list = (value as string).Split(";").Select(s => new VM_DzialanieDF() { Opis = s, VM_WindykacjaRow = parameter as VM_WindykacjaRow }).ToList();
    //            return new ObservableCollection<VM_DzialanieDF>(list);
    //        }
    //        return "";
    //    }
    //}
}
