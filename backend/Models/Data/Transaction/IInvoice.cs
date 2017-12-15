using api.Models.Data.Account;

namespace api.Models.Data.Transaction
{
    public interface IInvoice: IEntityModel
    {
        int? FromId {get; set;}
        //Shop From {get; set;}
        int? ToId {get; set;}
        //Shop To {get; set;}
    }
}