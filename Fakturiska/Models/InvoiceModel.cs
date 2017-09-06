using Fakturiska.Business.Enumerations;
using Fakturiska.Business.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakturiska.Models
{
    public class InvoiceModel
    {
        public int InvoiceId { get; set; }
        public Guid InvoiceGuid { get; set; }
        public DateTime? Date { get; set; }
        public bool InvoiceEstimate { get; set; }
        public bool InvoiceTotal { get; set; }
        public bool Incoming { get; set; }
        public bool Paid { get; set; }
        public bool Risk { get; set; }
        public int? Sum { get; set; }
        public DateTime? PaidDate { get; set; }
        public PriorityEnum? Priority { get; set; }
        public string PriorityName { get; set; }
        public int? ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public int? PayerId { get; set; }
        public string PayerName { get; set; }
        public HttpPostedFileBase File { get; set; }
        public string FilePath { get; set; }
        public int? Archive { get; set; }

        public InvoiceModel()
        {

        }

        public static IEnumerable<InvoiceModel> GetAllInvoices()
        {
            return InvoiceLogic.GetAllInvoices().Select(invoice => new InvoiceModel
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceGuid = invoice.InvoiceGuid,
                Date = invoice.Date,
                InvoiceEstimate = Convert.ToBoolean(invoice.InvoiceEstimate),
                InvoiceTotal = Convert.ToBoolean(invoice.InvoiceTotal),
                Incoming = Convert.ToBoolean(invoice.Incoming),
                Paid = Convert.ToBoolean(invoice.Paid),
                Risk = Convert.ToBoolean(invoice.Risk),
                Sum = invoice.Sum,
                PaidDate = invoice.PaidDate,
                PriorityName = invoice.PriorityName,
                ReceiverName = invoice.ReceiverName,
                PayerName = invoice.PayerName,
                FilePath = invoice.FilePath,
                Archive = invoice.Archive,
            });
        }
    }
}