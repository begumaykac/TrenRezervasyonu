using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class RezervasyonRes
    {
        public bool RezervasyonYapilabilir { get; set; }
        public List<YerlesimDetay> YerlesimAyrinti { get; set; } = new();
    }
}