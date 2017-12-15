using System;
using api.Models.Data.Account;

namespace api.Models.Data
{
    public interface IEntityModel
    {
        int Id {get; set;}
        DateTime CreationTime {get; set;}
        int? CreatorId {get; set;}
        User Creator {get; set;}
        bool Deleted {get; set;}
        byte[] RowVersion { get; set; }
    }
}