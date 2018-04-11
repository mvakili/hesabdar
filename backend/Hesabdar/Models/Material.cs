using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hesabdar.Models
{
    public class Material : BaseEntity
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
    }
}
