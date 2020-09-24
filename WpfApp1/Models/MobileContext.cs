using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace WpfApp1.Models
{
    public class MobileContext : DbContext
    {
        public  MobileContext() : base("DefaultConnection")
        {

        }
        public DbSet<Phone> Phones { get; set; }
    }

}
