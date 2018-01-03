using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace api.Models.Data.Account {
    public class User: IEntityModel {
        [Key]
        public int Id {get; set; }
        public string Username {get; set;}
        public string Password {get; set;}
        [JsonConverter(typeof(StringEnumConverter))]
        public PermissionLevel Permission {get; set;}
        [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreationTime { get ; set ; }
        [ForeignKey("Creator")]
        public int? CreatorId { get ; set ; }
        public virtual User Creator { get ; set ; }
        public bool Deleted {get; set;}
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}