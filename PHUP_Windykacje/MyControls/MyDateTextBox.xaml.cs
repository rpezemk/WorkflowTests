using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Prism.Commands;
namespace PHUP_Windykacje.MyControls
{
    /// <summary>
    /// Interaction logic for MyDateTextBox.xaml
    /// </summary>
    public partial class MyDateTextBox : UserControl
    {
        public MyDateTextBox()
        {
            InitializeComponent();
            TodayBtnCmd = new DelegateCommand(ResetDate);
        }

        private void ResetDate()
        {
            SetValue(MyDateProperty, DateTime.Now);
        }

        public DateTime MyDate
        {
            get { return (DateTime)GetValue(MyDateProperty); }
            set { SetValue(MyDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyDateProperty =
            DependencyProperty.Register("MyDate", typeof(DateTime), typeof(MyDateTextBox), new PropertyMetadata(null));




        public DelegateCommand TodayBtnCmd
        {
            get { return (DelegateCommand)GetValue(TodayBtnCmdProperty); }
            set { SetValue(TodayBtnCmdProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TodayBtnCmd.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TodayBtnCmdProperty =
            DependencyProperty.Register("TodayBtnCmd", typeof(ICommand), typeof(MyDateTextBox), new PropertyMetadata(null));





        private void CalendarControl_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
