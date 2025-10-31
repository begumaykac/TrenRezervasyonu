using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Tren
    {
        public string Ad { get; set; } = string.Empty;
        public List<Vagon> Vagonlar { get; set; } = new();
    }
}