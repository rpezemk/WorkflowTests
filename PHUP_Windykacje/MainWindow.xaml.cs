using PHUP_Windykacje.ViewModel;

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;

namespace PHUP_Windykacje
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Events.LoadRaportDataCmd.Subscribe(OnRaportDataLoad);
        }

        private void OnRaportDataLoad(VM_TabRaport vm)
        {
            Binding b = new Binding("VM_RaportRows");
            //MyDataGrid13.SetBinding(DataGrid.ItemsSourceProperty, b);
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = sender.GetType();
            var dg = sender as DataGrid;
            var sel = dg.SelectedValue;
            if (sel == null)
                return;
            if (sel.GetType() != typeof(VM_RaportRow))
                return;
            Events.RaportRowSelectionChanged.Publish(sel as VM_RaportRow);
        }

        private void DataGridTemplateColumn_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void DataGridTemplateColumn_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void TextBlock_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void TextBlock_TargetUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void DataGridRow_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void DzialaniaDFDataGrid_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {

        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null)
                return;
            tb.IsReadOnly = false;
            tb.Select(tb.Text.Length, 0);
            tb.Cursor = Cursors.IBeam;

        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = sender as TextBox;
            if (tb == null)
                return;
            tb.IsReadOnly = true;
            tb.Cursor = Cursors.Arrow;
        }


        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                var hyp = sender as Hyperlink;
                var test = hyp.NavigateUri.OriginalString;
                var mailto = $"mailto:{test}?subject=Windykacja&amp;body=Dzień dobry";
                Process.Start(new ProcessStartInfo(mailto) { UseShellExecute = true });
                e.Handled = true;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
