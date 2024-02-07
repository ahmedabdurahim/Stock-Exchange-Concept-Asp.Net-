using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NET_project.Models
{
    public class IPO
    {
        public int Id { get; set; }
        public string AssetName { get; set; }
        public string Ticker { get; set; }
        public int Shares { get; set; }
        public int Circulating { get; set; }
        public double Capital { get; set; }
        public double Value { get; set; }
    }
}