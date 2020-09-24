using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Windows.Documents;




namespace Work.Tables
{
    public class TableContext : DbContext
    {
        public TableContext()
            : base("DefaultConnection")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>()
                .HasRequired<Provider>(b => b.Provider)
                .WithMany(a => a.Invoices)
                .HasForeignKey<int>(b => b.ProviderId);
            modelBuilder.Entity<Goods>()
                .HasRequired<Invoice>(b => b.Invoice)
                .WithMany(a => a.Goods)
                .HasForeignKey<int>(b => b.InvoiceId);
            modelBuilder.Entity<Payment>()
                .HasRequired<Invoice>(b => b.Invoice)
                .WithMany(a => a.Payments)
                .HasForeignKey<int>(b => b.InvoiceId);
        }
        public System.Data.Entity.DbSet<Provider> Providers { get; set; }
        public System.Data.Entity.DbSet<Invoice> Invoices { get; set; }
        public System.Data.Entity.DbSet<Goods> Goods { get; set; }
        public System.Data.Entity.DbSet<Payment> Payments { get; set; }

        
    }

    public abstract class TableFirst
    {
        public virtual int Id { get; set; }
        public static Parameter[] GetProperty;
        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }

    public class Provider : TableFirst //Постачальник
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; } //ЄДРПОУ

        [Required]
        public string Name { get; set; }
        public ICollection<Invoice> Invoices { get; set; }
        public Provider()
        {
            Invoices = new List<Invoice>();
        }

        public static new Parameter[] GetProperty
        {
            get { return new Parameter[] { new Parameter("Id", typeof(int)), new Parameter("Name", typeof(string)) }; }
        }

        public void Update(Provider provider)
        {
            this.Name = provider.Name;
        }

        public float GetResult(DateTime endTime, List<Invoice> InvoicesT, TableContext dbTable)
        {
            float start = 0;
            ICollection<Invoice> InvoicesTest = InvoicesT.Where(i =>(i.data <= endTime)).ToList();
            foreach (var invoice in InvoicesTest)
            {
                dbTable.Goods.Load();
                dbTable.Payments.Load();
                var goodsCost = dbTable.Goods.Local.Where(g => g.InvoiceId == invoice.Id).Sum(g => g.Price);
                var payments = dbTable.Payments.Local.Where(g => g.InvoiceId == invoice.Id).Sum(g => g.Price);
                start += goodsCost - payments;
            }

            return start;
        }
        public float[] GetResult(DateTime startTime, DateTime endTime, List<Invoice> InvoicesT, TableContext dbTable)
        {
            float[] start = new float[2];
            ICollection<Invoice> InvoicesTest = InvoicesT.Where(i => (i.data >= startTime && i.data <= endTime)).ToList();
            foreach (var invoice in InvoicesTest)
            {
                dbTable.Goods.Load();
                dbTable.Payments.Load();
                var goodsCost = dbTable.Goods.Local.Where(g => g.InvoiceId == invoice.Id).Sum(g => g.Price);
                var payments = dbTable.Payments.Local.Where(g => g.InvoiceId == invoice.Id).Sum(g => g.Price);
                start[0] += goodsCost;
                start[1] += payments;
            }

            return start;
        }
    }

    public class Invoice : TableFirst //Накладна
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; } //Номер Накладної

        [Required]
        public DateTime data { get; set; }

        [Required]
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

        public ICollection<Goods> Goods { get; set; }
        public ICollection<Payment> Payments { get; set; }

        public Invoice()
        {
            Goods = new List<Goods>();
            Payments = new List<Payment>();
        }

        public static new Parameter[] GetProperty
        {
            get
            {
                return new Parameter[] { new Parameter("Id", typeof(int)), new Parameter("data", typeof(DateTime)), new Parameter("ProviderId", typeof(Provider)) };
            }
        }

        public void Update(Invoice invoice)
        {
            this.data = invoice.data;
            this.ProviderId = invoice.ProviderId;
        }
    }

    public class Goods : TableFirst //Товар
    {
        public override int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float Price { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public static new Parameter[] GetProperty
        {
            get
            {
                return new Parameter[] { new Parameter("Id", typeof(int)), new Parameter("Name", typeof(string)), new Parameter("Price", typeof(float)), new Parameter("InvoiceId", typeof(Invoice)) };
            }
        }
        public void Update(Goods invoice)
        {
            this.Name = invoice.Name;
            this.Price = invoice.Price;
            this.InvoiceId = invoice.InvoiceId;
        }
    }

    public class Payment : TableFirst //Платіж
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        [Required]
        public DateTime data { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public float Price { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public static new Parameter[] GetProperty
        {
            get
            {
                return new Parameter[] { new Parameter("Id", typeof(int)), new Parameter("data", typeof(DateTime)), new Parameter("Type", typeof(string)), new Parameter("Price", typeof(float)), new Parameter("InvoiceId", typeof(Invoice)) };
            }
        }

        public void Update(Payment payment)
        {
            this.data = payment.data;
            this.Type = payment.Type;
            this.Price = payment.Price;
            this.InvoiceId = payment.InvoiceId;
        }

    }

    public class Parameter
    {
        public string name { get; set; }
        public Type type { get; set; }
        public Parameter(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }
    }
}
