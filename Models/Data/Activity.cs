using api.Models.Data.Account;

namespace api.Models.Data 
{
    public class Activity {
        public int UserId {get; set;}
        public User User {get; set;}
    }
}