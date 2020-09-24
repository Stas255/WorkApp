using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
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
using Work.Classes;
using Work.Tables;

namespace Work.Windows
{
    /// <summary>
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        protected TableContext dbTable;
        protected Type typeMain;
        protected Button buttonSave;
        public Add()
        {
            dbTable = new TableContext();
            InitializeComponent();
        }

        private void CreateLine(Parameter[] list)
        {
            //for (int i = 1; i < 1; i++)
            //{
            //    ColumnDefinition c1 = new ColumnDefinition();
            //    c1.Width = new GridLength(280, GridUnitType.Star);
            //    GridN.ColumnDefinitions.Add(c1);
            //}
            //for (int i = 0; i < list.Length + 1; i++)
            //{
            //    RowDefinition rowDefinition = new RowDefinition();
            //    rowDefinition.Height = new GridLength(50* list.Length, GridUnitType.Star);
            //    GridN.RowDefinitions.Add(rowDefinition);
            //}
        }

        public void CreateWindow(TableFirst table)
        {
            typeMain = table.GetType();
            var type = table.GetType().GetProperty("GetProperty").GetValue(null);
            if (type is Parameter[] list)
            { /*type.GetType() == typeof(string[]))*/
                CreateLine(list);
                Point position = new Point(0, 0);
                foreach (var t in list)
                {
                    AddInput(t.type, t.name, out position);
                }
                AddButtonSave(position);
            }
            this.SizeToContent = SizeToContent.WidthAndHeight;
            //this.ResizeMode = ResizeMode.NoResize;
        }

        private void AddButtonSave(Point p)
        {
            buttonSave = new Button();
            buttonSave.Content = "Save";
            buttonSave.Height = 30;
            buttonSave.Width = 120;
            buttonSave.Margin = new Thickness(10);
            buttonSave.Click += ButtonSave_Click;
            //Grid.SetRow(buttonSave, (int)p.Y);
            //Grid.SetColumn(buttonSave, (int)p.X);
            StackP.Children.Add(buttonSave);
        }

        public void AddInput(Type type, string name, out Point p)
        {
            TextBox textB;
            ComboBox comboBox;
            Label label = new Label();
            label.Content = name;
            Binding binding = new Binding();
            binding.ElementName = name;
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.String:
                    textB = new TextBox();
                    //textB.Width = 120;
                    textB.Height = 17;
                    textB.Name = name;
                    //Grid.SetRow(textB, (int)p.Y);
                    //Grid.SetColumn(textB, (int)p.X);
                    label.SetBinding(TextBlock.TextProperty, binding);
                    StackP.Children.Add(label);
                    StackP.Children.Add(textB);
                    break;
                case TypeCode.DateTime:
                    DatePicker datePicker = new DatePicker();
                    datePicker.Name = name;
                    //Grid.SetRow(datePicker, (int)p.Y);
                    //Grid.SetColumn(datePicker, (int)p.X);
                    label.SetBinding(TextBlock.TextProperty, binding);
                    StackP.Children.Add(label);
                    StackP.Children.Add(datePicker);
                    break;
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                    textB = ButtonClass.CreteTextBox(type);
                    textB.Name = name;
                    //Grid.SetRow(textB, (int)p.Y);
                    //Grid.SetColumn(textB, (int)p.X);
                    label.SetBinding(TextBlock.TextProperty, binding);
                    StackP.Children.Add(label);
                    StackP.Children.Add(textB);
                    break;
                case TypeCode.Object:
                    if (type == typeof(Provider) || type == typeof(Invoice) || type == typeof(Goods))
                    {
                        comboBox = ButtonClass.CreteComboBox(type);
                        comboBox.Name = name;
                        //Grid.SetRow(comboBox, (int)p.Y);
                        //Grid.SetColumn(comboBox, (int)p.X);
                        label.SetBinding(TextBlock.TextProperty, binding);
                        StackP.Children.Add(label);
                        StackP.Children.Add(comboBox);
                    }
                    break;
                default:
                    MessageBox.Show("Тип не опреділився (Error521).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw new IndexOutOfRangeException();
            }
            p.Y += 1;
        }

        public void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (typeMain == typeof(Invoice))
            {
                Invoice newInvoice = (Invoice)Save();
                dbTable.Invoices.Add(newInvoice);
            }
            else if (typeMain == typeof(Provider))
            {
                Provider newProvider = (Provider)Save();
                dbTable.Providers.Add(newProvider);
            }
            else if (typeMain == typeof(Payment))
            {
                Payment newPayment = (Payment)Save();
                dbTable.Payments.Add(newPayment);
            }
            else if (typeMain == typeof(Goods))
            {
                Goods newPayment = (Goods)Save();
                dbTable.Goods.Add(newPayment);
            }
            dbTable.SaveChanges();
            this.Close();
        }

        protected TableFirst Save()
        {
            TableFirst tableFirst = new Invoice();
            if (typeMain == typeof(Invoice))
            {
                Parameter[] property = Invoice.GetProperty;
                tableFirst = new Invoice();
                foreach (Parameter p in property)
                {
                    PropertyInfo f = tableFirst.GetType().GetProperty(p.name);
                    f.SetValue(tableFirst, GetValueByName(p));
                }

            }
            else if(typeMain == typeof(Provider))
            {
                Parameter[] property = Provider.GetProperty;
                tableFirst = new Provider(); ;
                foreach (Parameter p in property)
                {
                    PropertyInfo f = tableFirst.GetType().GetProperty(p.name);
                    f.SetValue(tableFirst, GetValueByName(p));
                }
            }
            else if (typeMain == typeof(Payment))
            {
                Parameter[] property = Payment.GetProperty;
                tableFirst = new Payment();
                foreach (Parameter p in property)
                {
                    PropertyInfo f = tableFirst.GetType().GetProperty(p.name);
                    f.SetValue(tableFirst, GetValueByName(p));
                }
            }
            else if (typeMain == typeof(Goods))
            {
                Parameter[] property = Goods.GetProperty;
                tableFirst = new Goods();
                foreach (Parameter p in property)
                {
                    PropertyInfo f = tableFirst.GetType().GetProperty(p.name);
                    f.SetValue(tableFirst, GetValueByName(p));
                }
            }
            return tableFirst;
        }

        public object GetValueByName(Parameter p)
        {
            TextBox textB;
            DatePicker datePicker;
            ComboBox comboBox;
            switch (Type.GetTypeCode(p.type))
            {
                case TypeCode.String:
                    textB = (TextBox)StackP.Children.OfType<TextBox>().Where(t => t.Name == p.name).FirstOrDefault();
                    return textB.Text;
                    break;
                case TypeCode.DateTime:
                    datePicker = (DatePicker)StackP.Children.OfType<DatePicker>().Where(t => t.Name == p.name).FirstOrDefault();
                    return DateTime.Parse(datePicker.Text);
                    break;
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                    textB = (TextBox)StackP.Children.OfType<TextBox>().Where(t => t.Name == p.name).FirstOrDefault();
                    return int.Parse(textB.Text);
                    break;
                case TypeCode.Object:
                    if (p.type == typeof(Provider) || p.type == typeof(Invoice) || p.type == typeof(Payment))
                    {
                        comboBox = (ComboBox)StackP.Children.OfType<ComboBox>().Where(t => t.Name == p.name).FirstOrDefault();
                        return int.Parse(comboBox.SelectedValue.ToString());
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
}
