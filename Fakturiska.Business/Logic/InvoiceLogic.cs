using Fakturiska.Business.DTOs;
using Fakturiska.Database;
using OpenPop.Mime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace Fakturiska.Business.Logic
{
    public class InvoiceLogic
    {
        public static void CreateInvoice(InvoiceDTO invoice)
        {
            Invoice i = new Invoice()
            {
                InvoiceUId = invoice.InvoiceGuid,
                UserId = invoice.UserId,
                Date = invoice.Date,
                Sum = invoice.Sum,
                InvoiceEstimate = invoice.InvoiceEstimate,
                InvoiceTotal = invoice.InvoiceTotal,
                Incoming = invoice.Incoming,
                Paid = invoice.Paid,
                Risk = invoice.Risk,
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

        public static void ArchiveInvoice(Guid invoiceGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var invoice = GetInvoiceById(invoiceGuid, dc);
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
                var invoice = GetInvoiceById(invoiceGuid, dc);
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

        public static void PrintInvoice(String filePath)
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = filePath;
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = new Process();
            p.StartInfo = info;
            p.Start();
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
                    invoiceDTO.Sum = invoice.Sum;
                    invoiceDTO.InvoiceEstimate = invoice.InvoiceEstimate;
                    invoiceDTO.InvoiceTotal = invoice.InvoiceTotal;
                    invoiceDTO.Incoming = invoice.Incoming;
                    invoiceDTO.Paid = invoice.Paid;
                    invoiceDTO.Risk = invoice.Risk;
                    if(invoice.Priority != null)
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

        private static Invoice GetInvoiceById(Guid invoiceGuid, FakturiskaDBEntities dc)
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
