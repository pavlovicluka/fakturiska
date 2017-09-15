using Fakturiska.Business.Logic;
using Microsoft.AspNet.SignalR;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fakturiska.App_Start
{
    public class JobClass : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
           // InvoiceLogic.ReceiveMail();
        }
    }
}