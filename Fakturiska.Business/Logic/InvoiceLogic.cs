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
        public static Dictionary<string, int?> CreateEditInvoice(InvoiceDTO invoice, CompanyDTO companyReceiver, CompanyDTO companyPayer)
        {
            Dictionary<string, int?> response = null;
            using (var dc = new FakturiskaDBEntities())
            {   
                int? receiverId = null;
                if (companyReceiver != null)
                {
                    if (companyReceiver.CompanyGuid == null || companyReceiver.CompanyGuid == Guid.Empty)
                    {
                        response = CompanyLogic.CreateCompany(companyReceiver, dc, "Receiver");
                    }
                    else
                    {
                        response = CompanyLogic.CheckCompany(companyReceiver, dc, "Receiver");
                    }
                }
                
                int? payerId = null;
                if (companyPayer != null)
                {
                    if (companyPayer.CompanyGuid == null || companyPayer.CompanyGuid == Guid.Empty)
                    {
                        response = response.Concat(CompanyLogic.CreateCompany(companyPayer, dc, "Payer")).GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.First().Value);
                    }
                    else
                    {
                        response = response.Concat(CompanyLogic.CheckCompany(companyPayer, dc, "Payer")).GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.First().Value);
                    }
                }

                response.TryGetValue("companyReceiver", out receiverId);
                response.TryGetValue("companyPayer", out payerId);

                if (receiverId <= 0 || payerId <= 0)
                {
                    return response;
                }

                if (invoice.InvoiceGuid == null || invoice.InvoiceGuid == Guid.Empty)
                {
                    string filePath = SaveFile(invoice.File);
                    if (filePath != "")
                    {
                        invoice.ReceiverId = receiverId;
                        invoice.PayerId = payerId;
                        invoice.FilePath = filePath;
                        CreateInvoice(invoice, dc);
                    } else
                    {
                        response.Add("FileProblem", 1);
                        return response;
                    }
                }
                else
                {
                    invoice.ReceiverId = receiverId;
                    invoice.PayerId = payerId;
                    EditInvoice(invoice, dc);
                }
                dc.SaveChanges();
                response.Add("success", 1);
            }
            return response;
        }

        public static void CreateInvoice(InvoiceDTO invoice, FakturiskaDBEntities db = null)
        {
            if(invoice.Paid)
            {
                invoice.PaidDate = DateTime.Now.Date;
            }

            int? priorityId = null;
            if (invoice.Priority != null)
                priorityId = (int)invoice.Priority;

            Invoice i = new Invoice()
            {
                InvoiceUId = Guid.NewGuid(),
                UserId = invoice.UserId,
                Date = invoice.Date,
                InvoiceEstimate = invoice.InvoiceEstimate,
                InvoiceTotal = invoice.InvoiceTotal,
                Incoming = invoice.Incoming,
                Paid = invoice.Paid,
                Risk = invoice.Risk,
                Sum = invoice.Sum,
                PaidDate = invoice.PaidDate,
                PriorityId = priorityId,
                ReceiverId = invoice.ReceiverId,
                PayerId = invoice.PayerId,
                FilePath = invoice.FilePath
            };

            if(db == null)
            {
                using (var dc = new FakturiskaDBEntities())
                {
                    dc.Invoices.Add(i);
                    dc.SaveChanges();
                }
            } else
            {
                db.Invoices.Add(i);
            }
        }

        public static void EditInvoice(InvoiceDTO invoice)
        {
            if (invoice.Paid)
            {
                invoice.PaidDate = DateTime.Now.Date;
            }

            int? priorityId = null;
            if (invoice.Priority != null)
                priorityId = (int)invoice.Priority;

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
                   i.PriorityId = priorityId;
                   i.ReceiverId = invoice.ReceiverId;
                   i.PayerId = invoice.PayerId;
                }
                dc.SaveChanges();
            }
        }

        public static void EditInvoice(InvoiceDTO invoice, FakturiskaDBEntities dc)
        {
            if (invoice.Paid)
            {
                invoice.PaidDate = DateTime.Now.Date;
            }

            int? priorityId = null;
            if (invoice.Priority != null)
                priorityId = (int)invoice.Priority;

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
                i.PriorityId = priorityId;
                i.ReceiverId = invoice.ReceiverId;
                i.PayerId = invoice.PayerId;
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

        public static InvoiceDTO GetInvoiceByGuid(Guid invoiceGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var invoice = dc.Invoices.FirstOrDefault(i => i.InvoiceUId == invoiceGuid);
                return new InvoiceDTO(invoice);
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

        public static string GetFilePathByGuid(Guid invoiceGuid)
        {
            using (var dc = new FakturiskaDBEntities())
            {
                var invoice = dc.Invoices.FirstOrDefault(i => i.InvoiceUId == invoiceGuid);
                return invoice.FilePath;
            }
        }

        private static Invoice GetInvoiceByGuid(Guid invoiceGuid, FakturiskaDBEntities dc)
        {
            return dc.Invoices.FirstOrDefault(i => i.InvoiceUId == invoiceGuid && i.DeleteDate == null);
        }

        public static string SaveFile(HttpPostedFileBase invoiceFile)
        {
            string path = "";
            if (invoiceFile != null && invoiceFile.ContentLength > 0)
            {
                string name = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(invoiceFile.FileName);
                if (extension == ".pdf" || extension == ".jpg" || extension == ".png")
                {
                    path = Path.Combine(HttpContext.Current.Server.MapPath("~/Files/"), name + extension);
                    invoiceFile.SaveAs(path);
                }  
            }
            return path;
        }

        static string SERVER_PATH = @"C:\Users\ING\source\repos\Fakturiska\Fakturiska\Files\";
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

            int count = 0;
            foreach (Message msg in Messages)
            {
                count++;
                string email = msg.Headers.From.Address;
                int? userId = 0;
                userId = UserLogic.GetUserIdByEmail(email);
                foreach (var attachment in msg.FindAllAttachments())
                {
                    try
                    {
                        string filePath = "";
                        string name = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(attachment.FileName);

                        if (extension == ".pdf" || extension == ".jpg" || extension == ".png")
                        {
                            //filePath = Path.Combine(HttpContext.Current.Server.MapPath("~/Files/"), name + extension);
                            filePath = SERVER_PATH + name + extension;

                            FileStream Stream = new FileStream(filePath, FileMode.Create);
                            BinaryWriter BinaryStream = new BinaryWriter(Stream);
                            BinaryStream.Write(attachment.Body);
                            BinaryStream.Close();

                            if (filePath != "" && userId != null)
                            {
                                CreateInvoice(new InvoiceDTO
                                {
                                    UserId = (int)userId,
                                    InvoiceEstimate = false,
                                    InvoiceTotal = false,
                                    Incoming = false,
                                    Paid = false,
                                    Risk = false,
                                    FilePath = filePath
                                });
                                context.Clients.All.MailReceived("Invoice added by email");
                            }
                        }
                    }
                    finally
                    {
                        PopClient.DeleteMessage(count);
                    }
                }
            }
            PopClient.Disconnect();
        }

        private static string ConvertFile(HttpPostedFileBase invoiceFile)
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
    }
}
