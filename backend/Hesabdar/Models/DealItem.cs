using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Hesabdar.Models
{
    public class DealItem : BaseEntity
    {
        [Required]
        Material Material { get; set; }

        Decimal Quantity { get; set; }

        Decimal PricePerOne { get; set; }
    }
}
