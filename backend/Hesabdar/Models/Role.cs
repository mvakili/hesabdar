using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Hesabdar.Models.Enums;

namespace Hesabdar.Models
{
    public class Role: IdentityRole
    {
        public RoleType Type {get; set;}
    }
}