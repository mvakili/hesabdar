namespace Hesabdar.Models
{
    public class PaymentModel : Payment
    {
        public Dealer Payee { get; set; }
        public Dealer Payer { get; set; }
        public bool IsDealPayment { get; internal set; }
        public bool IsDealPrice { get; internal set; }
        public Deal Deal { get; internal set; }
    }
}