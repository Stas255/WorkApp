using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Work.Tables;

namespace Work.Classes
{
    class ButtonClass
    {
        public static TextBox CreteTextBox(Type type)
        {
            TextBox textB = new TextBox();
            if (type == typeof(int))
            {
                textB.KeyDown += TextBox_KeyDown_OnlyNumber;
            }
            else if (type == typeof(float))
            {
                textB.KeyDown += TextBox_KeyDown_OnlyNumberAndComma;
            }
            return textB;
        }

        public static ComboBox CreteComboBox(Type type)
        {
            ComboBox comboBox = new ComboBox();
            TableContext dbTable;
            dbTable = new TableContext();
            if (type == typeof(Provider))
            {
                dbTable.Providers.Load();
                var providers = dbTable.Providers.Local.ToBindingList();
                comboBox.DisplayMemberPath = "Name";
                comboBox.SelectedValuePath = "Id";
                comboBox.ItemsSource = providers;
            }else if (type == typeof(Invoice))
            {
                dbTable.Invoices.Load();
                var providers = dbTable.Invoices.Local.ToBindingList();
                comboBox.DisplayMemberPath = "data";
                comboBox.SelectedValuePath = "Id";
                comboBox.ItemsSource = providers;
            }
            return comboBox;
        }

        public static void TextBox_KeyDown_OnlyNumber(object sender, KeyEventArgs e)
        {

            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Back || e.Key == Key.Delete))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        public static void TextBox_KeyDown_OnlyNumberAndComma(object sender, KeyEventArgs e)
        {
            e.Handled = true;
            if (e.Key == Key.OemComma || (e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Back || e.Key == Key.Delete))
            {
                TextBox tb = (TextBox)sender;
                string text = tb.Text;
                if (e.Key == Key.OemComma)
                {
                    if (text.Length != 0 && !text.Contains(','))
                    {
                        e.Handled = false;
                    }
                }
                else
                {
                    e.Handled = false;
                }
            }

        }


        public class ComboBoxPairs
        {
            public string _Key { get; set; }
            public string _Value { get; set; }

            public ComboBoxPairs(string _key, string _value)
            {
                _Key = _key;
                _Value = _value;
            }
        }
    }
}
