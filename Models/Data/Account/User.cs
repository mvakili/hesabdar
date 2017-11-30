using System.ComponentModel.DataAnnotations;

namespace api.Models.Data.Account {
    public class User: IEntityModel {
        [Key]
        public int Id {get; set; }
        public string Username {get; set;}
        public string Password {get; set;}
        public PermissionLevel Permission {get; set;}
        public bool Deleted {get; set;}
    }
}