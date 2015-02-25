using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace KSUAdvising.Hubs
{
    public class AlertHub : Hub
    {
        public void AppointmentArrival(string name, string message)
        {
            Clients.All.addAppointmentArrivalNotification(name, message);
        }
    }
}