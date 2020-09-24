using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Work.Tables;

namespace Work.Windows
{
    class Edit : Add
    {
        TableContext dbTable;
        Type type;
        int id;
        public Edit(Type type, int id, TableContext dbTable) : base()
        {
            this.type = type;
            this.id = id;
            this.dbTable = dbTable;
        }

        public void EditC()
        {
            buttonSave.Click -= ButtonSave_Click;
            buttonSave.Click += ButtonEdit_Click;
            if (type == typeof(Invoice))
            {
                dbTable.Invoices.Load();
                Invoice invoice = dbTable.Invoices.Find(id);
                Parameter[] property = Invoice.GetProperty;
                Type fieldsType = new Invoice().GetType();
                EditInTable(invoice, property, fieldsType);
            }
            else if (type == typeof(Provider))
            {
                dbTable.Providers.Load();
                Provider invoice = dbTable.Providers.Find(id);
                Parameter[] property = Provider.GetProperty;
                Type fieldsType = new Provider().GetType();
                EditInTable(invoice, property, fieldsType);
            }
            else if (type == typeof(Payment))
            {
                dbTable.Payments.Load();
                Payment invoice = dbTable.Payments.Find(id);
                Parameter[] property = Payment.GetProperty;
                Type fieldsType = new Payment().GetType();
                EditInTable(invoice, property, fieldsType);
            }
            else if (type == typeof(Goods))
            {
                dbTable.Goods.Load();
                Goods invoice = dbTable.Goods.Find(id);
                Parameter[] property = Goods.GetProperty;
                Type fieldsType = new Goods().GetType();
                EditInTable(invoice, property, fieldsType);
            }
        }

        public void EditInTable(TableFirst invoice, Parameter[] property, Type fieldsType)
        {
            TextBox textB;
            DatePicker datePicker;
            ComboBox comboBox;
            string text;
            FieldInfo[] fields = fieldsType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (Parameter p in property)
            {
                FieldInfo f = fields.Where(f => f.Name.Contains(p.name) == true).FirstOrDefault();
                text = f.GetValue(invoice).ToString();
                switch (Type.GetTypeCode(p.type))
                {
                    case TypeCode.String:
                        textB = (TextBox)StackP.Children.OfType<TextBox>().Where(t => t.Name == p.name).FirstOrDefault();
                        textB.Text = text;
                        break;
                    case TypeCode.DateTime:
                        datePicker = (DatePicker)StackP.Children.OfType<DatePicker>().Where(t => t.Name == p.name).FirstOrDefault();
                        datePicker.Text = text;
                        break;
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.Single:
                        textB = (TextBox)StackP.Children.OfType<TextBox>().Where(t => t.Name == p.name).FirstOrDefault();
                        textB.Text = text;
                        break;
                    case TypeCode.Object:
                        if (type == typeof(Provider) || type == typeof(Invoice) || type == typeof(Payment) || type == typeof(Goods))
                        {
                            comboBox = (ComboBox)StackP.Children.OfType<ComboBox>().Where(t => t.Name == p.name).FirstOrDefault();
                            comboBox.SelectedValue = int.Parse(text);
                        }
                        else
                        {
                            MessageBox.Show("Тип не опреділився (Error521).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            throw new IndexOutOfRangeException();
                        }
                        break;
                    default:
                        MessageBox.Show("Тип не опреділився (Error521).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        throw new IndexOutOfRangeException();
                }
            }
        }

        public void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            if (typeMain == typeof(Invoice))
            {
                Invoice newInvoice = (Invoice)Save();
                var editInvoice = dbTable.Invoices.Find(id);
                editInvoice.Update(newInvoice);
            }
            else if (typeMain == typeof(Provider))
            {
                Provider newProvider = (Provider)Save();
                var editProvider = dbTable.Providers.Find(id);
                editProvider.Update(newProvider);
            }
            else if (typeMain == typeof(Payment))
            {
                Payment newPayment = (Payment)Save();
                var editPayment = dbTable.Payments.Find(id);
                editPayment.Update(newPayment);
            }
            else if (typeMain == typeof(Goods))
            {
                Goods newPayment = (Goods)Save();
                var editPayment = dbTable.Goods.Find(id);
                editPayment.Update(newPayment);
            }
            dbTable.SaveChanges();
            this.Close();
        }
    }
}

