using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace KSUAdvising.Hubs
{
    public class AlertHub : Hub
    {
        public void AppointmentArrival(string studentFlashlineID, string adviserFlashlineID)
        {
            //gets appointment arrival time
            string arrivalTime = DateTime.Now.ToShortTimeString();
            Clients.All.addAppointmentArrivalNotification(studentFlashlineID, adviserFlashlineID, arrivalTime);
        }

        public void StudentInQueue(string name, string message)
        {
            Clients.All.addStudentInQueue(name, message);
        }

        public void StudentOutQueue(string studentFlashlineID,string adviserFlashlineID, string message)
        {
            Clients.All.removeStudentFromQueue(studentFlashlineID,adviserFlashlineID, message);
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

        public void ForceAdviserInQueue(string name, string message)
        {
            Clients.All.forceAddAdviserInQueue(name, message);
        }

        public void ForceAdviserOutQueue(string name, string message)
        {
            Clients.All.forceRemoveAdviserFromQueue(name, message);
        }
    }
}