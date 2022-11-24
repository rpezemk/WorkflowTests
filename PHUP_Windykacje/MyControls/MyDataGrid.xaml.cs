using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MyDataGrid.xaml
    /// </summary>
    public partial class MyDataGrid : DataGrid
    {
        public MyDataGrid()
        {
            InitializeComponent();
            MyColumns = new ObservableCollection<MyDataGridColumn>();
        }



        public IEnumerable MyItemsSource
        {
            get { return (IEnumerable)GetValue(MyItemsSourceProperty); }
            set { SetValue(MyItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyItemsSourceProperty =
            DependencyProperty.Register("MyItemsSource", typeof(IEnumerable), typeof(MyDataGrid), new PropertyMetadata(default));


        public ObservableCollection<MyDataGridColumn> MyColumns
        {
            get { return (ObservableCollection<MyDataGridColumn>)GetValue(MyColumnsProperty); }
            set { SetValue(MyColumnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyColumns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyColumnsProperty =
            DependencyProperty.Register("MyColumns", typeof(ObservableCollection<MyDataGridColumn>), typeof(MyDataGrid), new PropertyMetadata());





        private void MyTemplateDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            foreach(var col in Columns)
            {

                BindingBase binding = null;
                if(col as DataGridBoundColumn != null)
                {
                    binding = (col as DataGridBoundColumn).Binding;
                }
                

                var resColumn = new MyDataGridColumn();

                TextBox textBox = new TextBox();
                if(binding != null)
                    textBox.SetBinding(TextBox.TextProperty, binding);// 

                DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();

                
            }
        }
    }
}
