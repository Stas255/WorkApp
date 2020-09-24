using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
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
using Work.Tables;
using Work.Windows;

namespace Work
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static TableContext dbTable;
        public MainWindow()
        {
            try
            {
                InitializeComponent();

                dbTable = new TableContext();
                //dbTable.Invoices.Load(); // загружаем данные
                //var res = typeof(Invoice).GetProperties()
                //            .Select(property => property.Name)
                //            .ToArray();
                //CreateGridColems(res);
                //Grid.ItemsSource = dbTable.Invoices.Local.ToBindingList();
                this.Closing += MainWindow_Closing;
                this.SizeToContent = SizeToContent.WidthAndHeight;
                double width = this.Width;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            dbTable.Dispose();
        }

        private void CreateGridColems(Parameter[] s)
        {
            foreach (var text in s)
            {
                AddColum(text.name);
            }
        }

        private void AddColum(string name)
        {
            DataGridTextColumn c = new DataGridTextColumn();
            c.Header = name;
            c.Binding = new Binding(name);
            c.Width = 110;
            Grid.Columns.Add(c);
        }
        private void ButtonInvoices_Click(object sender, RoutedEventArgs e)
        {
            ShowInvoices(-1);
        }
        private void ButtonProviders_Click(object sender, RoutedEventArgs e)
        {
            ShowProviders(-1);
        }
        private void ButtonPayments_Click(object sender, RoutedEventArgs e)
        {
            ShowPayments(-1);
        }
        private void ButtonGoods_Click(object sender, RoutedEventArgs e)
        {
            ShowGoods(-1);
        }

        private void ShowGoods(int idPayment)
        {
            Grid.Columns.Clear();
            List<Goods> result = new List<Goods>();
            var res = Goods.GetProperty;
            CreateGridColems(res);
            dbTable.Goods.Load();
            if (idPayment != -1)
            {
                Grid.ItemsSource = dbTable.Payments.Local.ToBindingList().Where(x => x.Id == idPayment);
            }
            else
            {
                Grid.ItemsSource = dbTable.Goods.Local.ToBindingList();
            }
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void ShowPayments(int idPayment)
        {
            Grid.Columns.Clear();
            List<Payment> result = new List<Payment>();
            var res = Payment.GetProperty;
            CreateGridColems(res);
            dbTable.Payments.Load();
            if (idPayment != -1)
            {
                Grid.ItemsSource = dbTable.Payments.Local.ToBindingList().Where(x => x.Id == idPayment);
            }
            else
            {
                Grid.ItemsSource = dbTable.Payments.Local.ToBindingList();
            }
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }

        private void ShowInvoices(int idProvider)
        {
            Grid.Columns.Clear();
            List<Invoice> result = new List<Invoice>();
            var res = Invoice.GetProperty;
            CreateGridColems(res);
            dbTable.Invoices.Load();
            if (idProvider != -1)
            {
                Grid.ItemsSource = dbTable.Invoices.Local.ToBindingList().Where(x => x.ProviderId == idProvider);
            }
            else
            {
                Grid.ItemsSource = dbTable.Invoices.Local.ToBindingList();
            }
            this.SizeToContent = SizeToContent.WidthAndHeight;

        }
        public static bool CheckConnection()
        {
            try
            {
                dbTable.Database.Connection.Open();
                dbTable.Database.Connection.Close();
            }
            catch (SqlException)
            {
                return false;
            }
            return true;
        }
        private void ShowProviders(int id)
        {
            try
            {
                Grid.Columns.Clear();
                List<Provider> result = new List<Provider>();
                var res = Provider.GetProperty;
                CreateGridColems(res);
                if (id != -1)
                {
                    result.Add(dbTable.Providers.Find(id));
                    Grid.ItemsSource = result;
                }
                else
                {
                    dbTable.Providers.Load();
                    Grid.ItemsSource = dbTable.Providers.Local.ToBindingList();
                }
                this.SizeToContent = SizeToContent.WidthAndHeight;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }


        private void ButtonAddPayment_Click(object sender, RoutedEventArgs e)
        {
            Add ed = new Add();
            ed.CreateWindow(new Payment());
            ed.ShowDialog();
        }
        private void ButtonAddInvoice_Click(object sender, RoutedEventArgs e)
        {
            Add ed = new Add();
            ed.CreateWindow(new Invoice());
            ed.ShowDialog();
        }
        private void ButtonAddProvider_Click(object sender, RoutedEventArgs e)
        {
            Add ed = new Add();
            ed.CreateWindow(new Provider());
            ed.ShowDialog();
        }

        private void ButtonAddGoods_Click(object sender, RoutedEventArgs e)
        {
            Add ed = new Add();
            ed.CreateWindow(new Goods());
            ed.ShowDialog();
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            dbTable.SaveChanges();
        }

        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            //Get the clicked MenuItem
            var menuItem = (MenuItem)sender;

            //Get the ContextMenu to which the menuItem belongs
            var contextMenu = (ContextMenu)menuItem.Parent;

            //Find the placementTarget
            var item = (DataGrid)contextMenu.PlacementTarget;

            //Get the underlying item, that you cast to your object that is bound
            //to the DataGrid (and has subject and state as property)
            var toEditFromBindedList = (TableFirst)item.SelectedCells[0].Item;

            Edit ed = new Edit(toEditFromBindedList.GetType(), toEditFromBindedList.Id, dbTable);
            ed.CreateWindow(toEditFromBindedList);
            ed.EditC();
            ed.ShowDialog();
            Grid.Items.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Exsel ex = new Exsel();
            ex.ShowDialog();
        }










        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    var rand = new Random();
        //    using (TableContext db = new TableContext())
        //    {
        //        // создаем объект Provider
        //        Provider provider1 = new Provider { Id = rand.Next(101), Name = "Stas"};


        //        // добавляем их в бд
        //        db.Providers.Add(provider1);
        //        db.SaveChanges();

        //        Invoice invoice1 = new Invoice { Id = rand.Next(101), data = new DateTime(2018,12,21), Provider = provider1 };
        //        db.Invoices.Add(invoice1);
        //        db.SaveChanges();
        //        MessageBox.Show("OK");
        //    }
        //}
    }
}
