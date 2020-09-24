using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Work.Classes;
using Work.Tables;
using ClassLibraryExsel;

namespace Work.Windows
{
    /// <summary>
    /// Логика взаимодействия для Exsel.xaml
    /// </summary>
    public partial class Exsel : Window
    {
        TableContext dbTable;
        private ICollection<Account> accounts;
        public Exsel()
        {
            InitializeComponent();
            dbTable = new TableContext();
            this.Closing += MainWindow_Closing;
        }
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            dbTable.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTimeStart = DatePickerStart.SelectedDate.Value.Date;
            DateTime dateTimeEnd = DatePickerEnd.SelectedDate.Value.Date;
            dbTable.Providers.Load();
            dbTable.Invoices.Load();
            List<Provider> providers = dbTable.Providers.ToList();
            accounts = new List<Account>();
            foreach (var provider in providers)
            {
                List<Invoice> InvoicesT = dbTable.Invoices.Local.Where(i => i.ProviderId == provider.Id).ToList();
                Account account = new Account();
                float test = provider.GetResult(dateTimeStart, InvoicesT, dbTable);
                if (test < 0)
                {
                    account.debitStart = 0;
                    account.creditStart = -test; 
                }
                else
                {
                    account.debitStart = test;
                    account.creditStart = 0;
                }
                float[] result = provider.GetResult(dateTimeStart, dateTimeEnd, InvoicesT, dbTable);
                account.debitPeroid = result[0];
                account.creditPeriod = result[1];
                account.id = provider.Id;
                account.name = provider.Name;
                accounts.Add(account);
            }

            Grid.ItemsSource = accounts;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ClassExsel cl = new ClassExsel(accounts);
        }
    }
}
