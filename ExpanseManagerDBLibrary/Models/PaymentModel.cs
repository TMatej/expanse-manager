using Newtonsoft.Json;
using System;

namespace ExpanseManagerDBLibrary.Models
{
    public class PaymentModel : IJsonConvertable
    {
        public long Id { get; set; }
        public AccountModel Sender { get; set; }
        public CurrencyModel SenderCurrency { get; set; }
        public decimal SenderAmount { get; set; }
        public AccountModel Receiver { get; set; }
        public CurrencyModel ReceiverCurrency { get; set; }
        public decimal ReceiverAmount { get; set; }
        public DateTime TransferDate { get; set; }

        public PaymentModel() { }

        public PaymentModel(AccountModel sender, CurrencyModel senderCurrency, decimal senderAmount, AccountModel receiver, CurrencyModel receiverCurrency, decimal receiverAmount, DateTime transferDate)
        {
            Sender = sender;
            SenderCurrency = senderCurrency;
            SenderAmount = senderAmount;
            Receiver = receiver;
            ReceiverCurrency = receiverCurrency;
            ReceiverAmount = receiverAmount;
            TransferDate = transferDate;
        }

        public string ToJSON(Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }
    }
}
