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

namespace PHUP_Windykacje.MyControls
{
    /// <summary>
    /// Interaction logic for MyDataGridColumn.xaml
    /// </summary>
    public partial class MyDataGridColumn : DataGridTemplateColumn
    {
        public MyDataGridColumn()
        {
            InitializeComponent();
        }



        public Binding Binding
        {
            get { return (Binding)GetValue(BindingProperty); }
            set { SetValue(BindingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Binding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingProperty =
            DependencyProperty.Register("Binding", typeof(Binding), typeof(MyDataGridColumn), new PropertyMetadata(default));


    }
}
