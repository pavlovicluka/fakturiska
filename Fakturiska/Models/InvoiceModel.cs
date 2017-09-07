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
        public int? InvoiceEstimate { get; set; }
        public bool InvoiceEstimateChecked { get; set; }
        public int? InvoiceTotal { get; set; }
        public bool InvoiceTotalChecked { get; set; }
        public int? Incoming { get; set; }
        public bool IncomingChecked { get; set; }
        public int? Paid { get; set; }
        public bool PaidChecked { get; set; }
        public int? Risk { get; set; }
        public bool RiskChecked { get; set; }
        public int? Sum { get; set; }
        public DateTime? PaidDate { get; set; }
        public PriorityEnum Priority { get; set; }
        public string PriorityName { get; set; }
        public string ReceiverName { get; set; }
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
                InvoiceEstimate = invoice.InvoiceEstimate,
                InvoiceTotal = invoice.InvoiceTotal,
                Incoming = invoice.Incoming,
                Paid = invoice.Paid,
                Risk = invoice.Risk,
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