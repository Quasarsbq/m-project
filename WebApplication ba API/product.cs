using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication_ba_API
{
    public class product
    {
        public int Amount { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public int PayDate { get; set; }
        public string CustomField { get; set; }
        public string RefId { get; set; }
    }
}