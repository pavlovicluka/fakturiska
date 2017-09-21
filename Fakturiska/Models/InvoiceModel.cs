using Fakturiska.Business.DTOs;
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
        public Guid? InvoiceGuid { get; set; }
        [DisplayName("Datum kreiranja")]
        public DateTime? Date { get; set; }
        [DisplayName("Predračun")]
        public bool InvoiceEstimate { get; set; }
        [DisplayName("Račun")]
        public bool InvoiceTotal { get; set; }
        [DisplayName("Ulazna")]
        public bool Incoming { get; set; }
        [DisplayName("Plaćena")]
        public bool Paid { get; set; }
        [DisplayName("Problematična")]
        public bool Risk { get; set; }
        [DisplayName("Suma")]
        public int? Sum { get; set; }
        [DisplayName("Datum plaćanja")]
        public DateTime? PaidDate { get; set; }
        [DisplayName("Važnost")]
        public PriorityEnum? Priority { get; set; }
        [DisplayName("Važnost")]
        public string PriorityName { get { return PriorityMethods.GetString(Priority); } }
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

        public InvoiceModel(InvoiceDTO invoice)
        {
            InvoiceId = invoice.InvoiceId;
            InvoiceGuid = invoice.InvoiceGuid;
            Date = invoice.Date;
            InvoiceEstimate = invoice.InvoiceEstimate;
            InvoiceTotal = invoice.InvoiceTotal;
            Incoming = invoice.Incoming;
            Paid = invoice.Paid;
            Risk = invoice.Risk;
            Sum = invoice.Sum;
            PaidDate = invoice.PaidDate;
            Priority = invoice.Priority;
            ReceiverName = invoice.ReceiverName;
            PayerName = invoice.PayerName;
            FilePath = invoice.FilePath;
            Archive = invoice.Archive;
        }

        public static IEnumerable<InvoiceModel> GetInvoices()
        {
            return InvoiceLogic.GetInvoices().Select(invoice => new InvoiceModel(invoice));
        }

        public static IEnumerable<InvoiceModel> GetArchivedInvoices()
        {
            return InvoiceLogic.GetArchivedInvoices().Select(invoice => new InvoiceModel(invoice));
        }

        public static InvoiceDTO MapModelToDTO(InvoiceModel invoice, int? userId, int? receiverId, int? payerId, string filePath)
        {
            InvoiceDTO invoiceDTO = new InvoiceDTO();
            if (invoice.InvoiceGuid != null && invoice.InvoiceGuid != Guid.Empty)
                invoiceDTO.InvoiceGuid = (Guid)invoice.InvoiceGuid;
            if (userId != null)
                invoiceDTO.UserId = (int)userId;
            invoiceDTO.Date = invoice.Date;
            invoiceDTO.InvoiceEstimate = invoice.InvoiceEstimate;
            invoiceDTO.InvoiceTotal = invoice.InvoiceTotal;
            invoiceDTO.Incoming = invoice.Incoming;
            invoiceDTO.Paid = invoice.Paid;
            invoiceDTO.Risk = invoice.Risk;
            invoiceDTO.Sum = invoice.Sum;

            if (invoice.Priority != null)
                invoiceDTO.Priority = (PriorityEnum)invoice.Priority;
            else
                invoiceDTO.Priority = null;

            if (receiverId != null)
                invoiceDTO.ReceiverId = receiverId;
            if (payerId != null)
                invoiceDTO.PayerId = payerId;
            if (filePath != null)
                invoiceDTO.FilePath = filePath;

            return invoiceDTO;
        }
    }

    public static class EnumHelper<T>
    {
        public static T GetValueFromName(string name)
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                    typeof(DisplayAttribute)) as DisplayAttribute;
                if (attribute != null)
                {
                    if (attribute.Name == name)
                    {
                        return (T)field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == name)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentOutOfRangeException("name");
        }
    }
}