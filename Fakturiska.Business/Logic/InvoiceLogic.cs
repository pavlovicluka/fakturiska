using Fakturiska.Business.DTOs;
using Fakturiska.Database;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.SignalR;
using OpenPop.Mime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

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
                dc.SaveChanges();
            }
        }

        public static InvoiceDTO GetInvoiceByGuid(Guid invoiceGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var invoice = dc.Invoices.FirstOrDefault(i => i.InvoiceUId == invoiceGuid);
                return new InvoiceDTO(invoice);
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
                dc.SaveChanges();
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
                dc.SaveChanges();
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
                dc.SaveChanges();
            }
        }

        public static IEnumerable<InvoiceDTO> GetInvoices()
        {
            List<InvoiceDTO> invoiceDTOs = null;
            using (var dc = new FakturiskaDBEntities())
            {
                var invoices = dc.Invoices.Where(invoice => invoice.DeleteDate == null && invoice.Archive == null).ToList();
                invoiceDTOs = invoices.Select(invoice => new InvoiceDTO(invoice)).ToList();
            }
            return invoiceDTOs;
        }

        public static IEnumerable<InvoiceDTO> GetArchivedInvoices()
        {
            List<InvoiceDTO> invoiceDTOs = null;
            using (var dc = new FakturiskaDBEntities())
            {
                var invoices = dc.Invoices.Where(invoice => invoice.DeleteDate == null && invoice.Archive != null).ToList();
                invoiceDTOs = invoices.Select(invoice => new InvoiceDTO(invoice)).ToList();
            }
            return invoiceDTOs;
        }

        private static Invoice GetInvoiceByGuid(Guid invoiceGuid, FakturiskaDBEntities dc)
        {
            return dc.Invoices.FirstOrDefault(i => i.InvoiceUId == invoiceGuid && i.DeleteDate == null);
        }

        public static string ConvertAndSaveFile(HttpPostedFileBase invoiceFile)
        {
            string path = "";
            if (invoiceFile != null && invoiceFile.ContentLength > 0)
            {
                string name = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(invoiceFile.FileName);
                path = Path.Combine(HttpContext.Current.Server.MapPath("~/Files/"), name + ".pdf");

                if (extension.Equals(".jpg") || extension.Equals(".png"))
                {
                    Document document = new Document();
                    using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        PdfWriter.GetInstance(document, stream);
                        document.Open();
                        using (var imageStream = invoiceFile.InputStream)
                        {
                            var image = Image.GetInstance(imageStream);
                            document.Add(image);
                        }
                        document.Close();
                    }
                }
                else if (extension.Equals(".pdf"))
                {
                    invoiceFile.SaveAs(path);
                }
            }
            return path;
        }

        public static void ReceiveMail()
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<RealTime>();

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
                    /*var fileName = Guid.NewGuid().ToString() + Path.GetExtension(attachment.FileName);
                    string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Files/"), fileName);
                    FileStream Stream = new FileStream(filePath, FileMode.Create);
                    BinaryWriter BinaryStream = new BinaryWriter(Stream);
                    BinaryStream.Write(attachment.Body);
                    BinaryStream.Close();*/

                    context.Clients.All.Send("Mail received!");
                }
            }
            PopClient.DeleteAllMessages();
            PopClient.Disconnect();
        }
    }
}
