using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models.Data.Account
{
    public class Shop : IEntityModel
    {
        [Key]
        public int Id { get ; set ; }
        public string Name {get; set;}
        public DateTime CreationTime { get ; set ; }
        [ForeignKey("Creator")]
        public int? CreatorId { get ; set ; }
        public virtual User Creator { get ; set ; }
        public bool Deleted { get ; set ; }
    }
}