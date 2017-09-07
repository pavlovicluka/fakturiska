using Fakturiska.Business.Enumerations;
using Fakturiska.Business.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fakturiska.Models
{
    public class InvoiceModel
    {
        [DisplayName("Broj fakture")]
        public int InvoiceId { get; set; }
        public Guid InvoiceGuid { get; set; }
        [DisplayName("Datum kreiranja")]
        public DateTime? Date { get; set; }
        [DisplayName("Predračun")]
        public int? InvoiceEstimate { get; set; }
        [DisplayName("Predračun")]
        public bool InvoiceEstimateChecked { get; set; }
        [DisplayName("Račun")]
        public int? InvoiceTotal { get; set; }
        [DisplayName("Račun")]
        public bool InvoiceTotalChecked { get; set; }
        [DisplayName("Ulazna")]
        public int? Incoming { get; set; }
        [DisplayName("Ulazna")]
        public bool IncomingChecked { get; set; }
        [DisplayName("Plaćena")]
        public int? Paid { get; set; }
        [DisplayName("Plaćena")]
        public bool PaidChecked { get; set; }
        [DisplayName("Problematična")]
        public int? Risk { get; set; }
        [DisplayName("Problematična")]
        public bool RiskChecked { get; set; }
        [DisplayName("Suma")]
        public int? Sum { get; set; }
        [DisplayName("Datum plaćanja")]
        public DateTime? PaidDate { get; set; }
        [DisplayName("Važnost")]
        public PriorityEnum Priority { get; set; }
        [DisplayName("Važnost")]
        public string PriorityName { get; set; }
        [DisplayName("Primalac")]
        public string ReceiverName { get; set; }
        [DisplayName("Uplatilac")]
        public string PayerName { get; set; }
        [DisplayName("Faktura")]
        [Required()]
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