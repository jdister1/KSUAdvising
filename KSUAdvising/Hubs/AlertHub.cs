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

        public void StudentInQueue(string name, string message)
        {
            Clients.All.addStudentInQueue(name, message);
        }

        public void StudentOutQueue(string name, string message)
        {
            Clients.All.removeStudentFromQueue(name, message);
        }

        public void StudentReturnedQueue(string name, string message)
        {
            Clients.All.returnStudentToQueue(name, message);
        }

        public void AdviserInQueue(string name, string message)
        {
            Clients.All.addAdviserInQueue(name, message);
        }
        public void AdviserOutQueue(string name, string message)
        {
            Clients.All.removeAdviserFromQueue(name, message);
        }
    }
}