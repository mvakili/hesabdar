using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.Data.Account;

namespace api.Models.Data.Transaction
{
    public class MaterialInvoice : IInvoice
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("From")]
        public int? FromId {get; set;}
        public virtual Shop From { get ; set ; }
        [ForeignKey("To")]
        public int? ToId {get; set;}
        public virtual Shop To { get ; set ; }
        [ForeignKey("FinancialInvoice")]
        public int? FinancialInvoiceId {get; set;}
        public FinancialInvoice FinancialInvoice {get;set;}
        public DateTime CreationTime { get; set; }
        [ForeignKey("Creator")]
        public int? CreatorId { get; set; }
        public virtual User Creator { get; set; }
        public bool Deleted { get; set; }

    }
}