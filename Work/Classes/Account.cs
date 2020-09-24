using System;
using System.Collections.Generic;
using System.Text;

namespace Work.Classes
{
    public class Account
    {
        public int id;
        public string name { get; set; }
        public float debitStart { get; set; }
        public float creditStart { get; set; }
        public float debitPeroid { get; set; }
        public float creditPeriod { get; set; }

        public float debitEnd
        {
            get
            {
                var test = (debitStart + creditPeriod) - (creditStart + debitPeroid);
                return test > 0 ? test : 0;
            }
        }

        public float creditEnd
        {
            get
            {
                var test = (creditStart + debitPeroid) - (debitStart + creditPeriod);
                return test > 0 ? test : 0;
            }
        }


    }
}
