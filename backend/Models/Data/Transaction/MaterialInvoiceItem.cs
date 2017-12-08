using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using api.Models.Data.Account;
using api.Models.Data.Materials;

namespace api.Models.Data.Transaction
{
    public class MaterialInvoiceItem : IEntityModel
    {
        [Key]
        public int Id { get ; set ; }
        [ForeignKey("Material")]
        public int MaterialId {get; set;}
        public virtual Material Material {get; set;}
        [ForeignKey("Unit")]
        public int UnitId {get;set;}
        public Unit Unit {get;set;}
        [DataType("decimal(18,5)")]
        public float Quantity {get;set;}
        [ForeignKey("MaterialInvoice")]
        public int MaterialInvoiceId {get; set;}
        public virtual MaterialInvoice MaterialInvoice {get; set;}
        public DateTime CreationTime { get ; set ; }
        [ForeignKey("Creator")]
        public int? CreatorId { get ; set ; }
        public virtual User Creator { get ; set ; }
        public bool Deleted { get ; set ; }
    }
}