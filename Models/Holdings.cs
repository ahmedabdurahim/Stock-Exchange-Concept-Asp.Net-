using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_project.Models
{
    public class Holdings
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string AssetName { get; set; }
        public string Ticker { get; set; }
        public double OpenPrice { get; set; }
        public string Date { get; set; }

    }
}