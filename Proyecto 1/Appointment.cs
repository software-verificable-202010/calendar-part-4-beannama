using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Proyecto_1
{
    [Serializable]
    public class Appointment
    {
        public string title { get; set; }
        public DateTime date { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string description { get; set; }

        public Appointment(string title, DateTime date, DateTime startTime, DateTime endTime, string description)
        {
            this.title = title;
            this.date = date;
            this.startTime = startTime;
            this.endTime = endTime;
            this.description = description;
        }
    }
}
