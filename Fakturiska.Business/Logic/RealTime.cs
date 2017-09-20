using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fakturiska.Business.Logic
{
    public class RealTime : Hub
    {
        public void CompaniesChanged(string message)
        {
            Clients.Others.CompaniesChanged(message);
        }
    }
}
