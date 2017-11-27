using System.ComponentModel.DataAnnotations;

namespace api.Models.Account {
    public class User {
        [Key]
        public int Id {get; set; }
        public string Username {get; set; }
        public string Password {get; set; }
    }
}