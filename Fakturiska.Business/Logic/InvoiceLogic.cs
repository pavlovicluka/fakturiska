using Fakturiska.Business.DTOs;
using Fakturiska.Database;
using OpenPop.Mime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using static System.Web.HttpContext;

namespace Fakturiska.Business.Logic
{
    public class InvoiceLogic
    {
        public static void CreateInvoice(InvoiceDTO invoice)
        {
            if(invoice.Paid == 1)
            {
                invoice.PaidDate = DateTime.Now.Date;
            }

            Invoice i = new Invoice()
            {
                InvoiceUId = invoice.InvoiceGuid,
                UserId = invoice.UserId,
                Date = invoice.Date,
                InvoiceEstimate = invoice.InvoiceEstimate,
                InvoiceTotal = invoice.InvoiceTotal,
                Incoming = invoice.Incoming,
                Paid = invoice.Paid,
                Risk = invoice.Risk,
                Sum = invoice.Sum,
                PaidDate = invoice.PaidDate,
                PriorityId = invoice.PriorityId,
                ReceiverId = invoice.ReceiverId,
                PayerId = invoice.PayerId,
                FilePath = invoice.FilePath
            };

            using (var dc = new FakturiskaDBEntities())
            {
                dc.Invoices.Add(i);

                try
                {
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static InvoiceDTO GetInvoiceByGuid(Guid invoiceGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var invoice = dc.Invoices.Where(i => i.InvoiceUId == invoiceGuid).FirstOrDefault();
                return new InvoiceDTO
                {
                    Date = invoice.Date,
                    InvoiceEstimate = invoice.InvoiceEstimate,
                    InvoiceTotal = invoice.InvoiceTotal,
                    Incoming = invoice.Incoming,
                    Paid = invoice.Paid,
                    Risk = invoice.Risk,
                    Sum = invoice.Sum,
                    PaidDate = invoice.PaidDate,
                    PriorityId = invoice.PriorityId,
                    ReceiverId = invoice.ReceiverId,
                    PayerId = invoice.PayerId,
                };
            }
        }

        public static void EditInvoice(InvoiceDTO invoice)
        {
            if (invoice.Paid == 1)
            {
                invoice.PaidDate = DateTime.Now.Date;
            }

            using (var dc = new FakturiskaDBEntities())
            {
                var i = GetInvoiceByGuid(invoice.InvoiceGuid, dc);
                if (i != null)
                {
                   i.Date = invoice.Date;
                   i.InvoiceEstimate = invoice.InvoiceEstimate;
                   i.InvoiceTotal = invoice.InvoiceTotal;
                   i.Incoming = invoice.Incoming;
                   i.Paid = invoice.Paid;
                   i.Risk = invoice.Risk;
                   i.Sum = invoice.Sum;
                   i.PaidDate = invoice.PaidDate;
                   i.PriorityId = invoice.PriorityId;
                   i.ReceiverId = invoice.ReceiverId;
                   i.PayerId = invoice.PayerId;
                }
                try
                {
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static void ArchiveInvoice(Guid invoiceGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var invoice = GetInvoiceByGuid(invoiceGuid, dc);
                if (invoice != null)
                {
                    invoice.Archive = 1;
                }
                try
                {
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static void DeleteInvoice(Guid invoiceGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var invoice = GetInvoiceByGuid(invoiceGuid, dc);
                if (invoice != null)
                {
                    invoice.DeleteDate = DateTime.Now;
                }
                try
                {
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public static IEnumerable<InvoiceDTO> GetAllInvoices()
        {
            List<InvoiceDTO> invoiceDTOs = new List<InvoiceDTO>();
            using (var dc = new FakturiskaDBEntities())
            {
                List<Invoice> invoices = dc.Invoices.Where(invoice => invoice.DeleteDate == null).ToList();
                foreach (var invoice in invoices)
                {
                    InvoiceDTO invoiceDTO = new InvoiceDTO();
                    invoiceDTO.InvoiceId = invoice.InvoiceId;
                    invoiceDTO.InvoiceGuid = invoice.InvoiceUId;
                    invoiceDTO.Date = invoice.Date;
                    invoiceDTO.InvoiceEstimate = invoice.InvoiceEstimate;
                    invoiceDTO.InvoiceTotal = invoice.InvoiceTotal;
                    invoiceDTO.Incoming = invoice.Incoming;
                    invoiceDTO.Paid = invoice.Paid;
                    invoiceDTO.Risk = invoice.Risk;
                    invoiceDTO.Sum = invoice.Sum;
                    invoiceDTO.PaidDate = invoice.PaidDate;
                    if (invoice.Priority != null)
                        invoiceDTO.PriorityName = invoice.Priority.Description;
                    if (invoice.CompanyReceiver != null)
                        invoiceDTO.ReceiverName = invoice.CompanyReceiver.Name;
                    if (invoice.CompanyPayer != null)
                        invoiceDTO.PayerName = invoice.CompanyPayer.Name;
                    invoiceDTO.FilePath = invoice.FilePath;
                    invoiceDTO.Archive = invoice.Archive;

                    invoiceDTOs.Add(invoiceDTO);
                }
            }
            return invoiceDTOs;
        }

        public static string SaveFile(HttpPostedFileBase invoiceFile)
        {
            string path = "";
            if (invoiceFile != null && invoiceFile.ContentLength > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(invoiceFile.FileName);
                path = Path.Combine(HttpContext.Current.Server.MapPath("~/Files/"), fileName);
                invoiceFile.SaveAs(path);
            }
            return path;
        } 

        private static Invoice GetInvoiceByGuid(Guid invoiceGuid, FakturiskaDBEntities dc)
        {
            return dc.Invoices.Where(i => i.InvoiceUId == invoiceGuid && i.DeleteDate == null).FirstOrDefault();
        }

        private static void ReceiveMail()
        {
            OpenPop.Pop3.Pop3Client PopClient = new OpenPop.Pop3.Pop3Client();
            PopClient.Connect("pop.mail.com", 995, true);
            PopClient.Authenticate("pavlovicluka.99@mail.com", "proba123", OpenPop.Pop3.AuthenticationMethod.UsernameAndPassword);

            var messageCount = PopClient.GetMessageCount();
            var Messages = new List<Message>(messageCount);

            for (int i = 0; i < messageCount; i++)
            {
                Message getMessage = PopClient.GetMessage(i + 1);
                Messages.Add(getMessage);
            }

            foreach (Message msg in Messages)
            {
                foreach (var attachment in msg.FindAllAttachments())
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(attachment.FileName);
                    string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Files/"), fileName);
                    FileStream Stream = new FileStream(filePath, FileMode.Create);
                    BinaryWriter BinaryStream = new BinaryWriter(Stream);
                    BinaryStream.Write(attachment.Body);
                    BinaryStream.Close();
                }
            }
            PopClient.DeleteAllMessages();
            PopClient.Disconnect();
        }
    }
}
