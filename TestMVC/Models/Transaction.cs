using System;

namespace TestMVC.Models
{
    public class Transaction
    {

        public string id { get; set; }

        public string acc_from { get; set; }

        public string acc_to { get; set; }

        public string trans_type { get; set; }

        public string date_done { get; set; }

        public string date_completed { get; set; }

        public string status { get; set; }

        public string done_by { get; set; }

        public string done_to { get; set; }

        public string description { get; set; }

        public string bank { get; set; }

        public string amount { get; set; }
    }
}
