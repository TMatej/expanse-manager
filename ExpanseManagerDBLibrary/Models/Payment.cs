using Newtonsoft.Json;
using System;

namespace ExpanseManagerDBLibrary.Models
{
    public class Payment : IJsonConvertable
    {
        public long Id { get; set; }
        public Account Sender { get; set; }
        public Currency SenderCurrency { get; set; }
        public decimal SenderAmount { get; set; }
        public Account Receiver { get; set; }
        public Currency ReceiverCurrency { get; set; }
        public decimal ReceiverAmount { get; set; }
        public DateTime TransferDate { get; set; }

        public Payment() { }

        public Payment(Account sender, Currency senderCurrency, decimal senderAmount, Account receiver, Currency receiverCurrency, decimal receiverAmount, DateTime transferDate)
        {
            Sender = sender;
            SenderCurrency = senderCurrency;
            SenderAmount = senderAmount;
            Receiver = receiver;
            ReceiverCurrency = receiverCurrency;
            ReceiverAmount = receiverAmount;
            TransferDate = transferDate;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
