using Fakturiska.Database;
using System;

namespace Fakturiska.Business.DTOs
{
    public class InvoiceDTO
    {
        public int InvoiceId { get; set; }
        public Guid InvoiceGuid { get; set; }
        public int UserId { get; set; }
        public DateTime? Date { get; set; }
        public int? InvoiceEstimate { get; set; }
        public int? InvoiceTotal { get; set; }
        public int? Incoming { get; set; }
        public int? Paid { get; set; }
        public int? Risk { get; set; }
        public int? Sum { get; set; }
        public DateTime? PaidDate { get; set; }
        public int? PriorityId { get; set; }
        public string PriorityName { get; set; }
        public int? ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public int? PayerId { get; set; }
        public string PayerName { get; set; }
        public string FilePath { get; set; }
        public int? Archive { get; set; }
        public DateTime? DeleteDate { get; set; }

        public InvoiceDTO()
        {

        }

        public InvoiceDTO(Invoice invoice)
        {
            InvoiceId = invoice.InvoiceId;
            InvoiceGuid = invoice.InvoiceUId;
            Date = invoice.Date;
            InvoiceEstimate = invoice.InvoiceEstimate;
            InvoiceTotal = invoice.InvoiceTotal;
            Incoming = invoice.Incoming;
            Paid = invoice.Paid;
            Risk = invoice.Risk;
            Sum = invoice.Sum;
            PaidDate = invoice.PaidDate;
            if (invoice.Priority != null)
            {
                PriorityId = invoice.PriorityId;
                PriorityName = invoice.Priority.Description;
            }
            if (invoice.CompanyReceiver != null)
            {
                ReceiverId = invoice.ReceiverId;
                ReceiverName = invoice.CompanyReceiver.Name;
            }
            if (invoice.CompanyPayer != null)
            {
                PayerId = invoice.PayerId;
                PayerName = invoice.CompanyPayer.Name;
            }
            FilePath = invoice.FilePath;
            Archive = invoice.Archive;
        }
    }
}
