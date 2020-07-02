using NUnit.Framework;
using Project;
using System;
using System.Collections.Generic;
using System.IO;

namespace AppointmentsTest
{
    
    public class HasUser
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void HasUser_UserIsNull_ReturnsTrue()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            string description = "";
            User creator = null;
            List<User> users = new List<User>();

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.HasUser(creator);

            Assert.IsFalse(result);
        }
        [Test]
        public void HasUser_UserIsCreator_ReturnsTrue()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            string description = "";
            string creatorMail= "a@a";
            User creator = new User(creatorMail);
            List<User> users = new List<User>();

            var appointment = new Appointment(title,date,start,end,description,creator, users);

            var result = appointment.HasUser(creator);

            Assert.IsTrue(result);
        }
        [Test]
        public void HasUser_UserIsOnList_ReturnsTrue()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);

            string invitedMail = "b@b";

            User invitedUser = new User(invitedMail);
            List<User> users = new List<User> { 
                invitedUser 
                };

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.HasUser(invitedUser);

            Assert.IsTrue(result);
        }
        [Test]
        public void HasUser_UserIsNotCreator_ReturnsFalse()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);
            string otherMail = "b@b";
            User otherUser = new User(otherMail);
            List<User> users = new List<User>();

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.HasUser(otherUser);

            Assert.IsFalse(result);
        }
        [Test]
        public void HasUser_UserIsNotOnList_ReturnsTrue()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);
            string otherMail = "b@b";
            User otherUser = new User(otherMail);
            List<User> users = new List<User>();

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.HasUser(otherUser);
            Assert.IsFalse(result);
        }
    }
    public class HasUsers
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void HasUsers_ListIsNull_ReturnFalse()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);
            List<User> users = new List<User>();

            List<User> testList = null;
            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.HasUsers(testList);
            Assert.IsFalse(result);
        }
        [Test]
        public void HasUsers_UserIsInList_ReturnTrue()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);
            string otherMail = "b@b";
            User otherUser = new User(otherMail);
            List<User> users = new List<User>
            {
                otherUser
            };

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.HasUsers(users);
            Assert.IsTrue(result);
        }
        [Test]
        public static void HasUsers_UserNotIsInList_ReturnFalse()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);
            string otherMail = "b@b";
            User otherUser = new User(otherMail);
            List<User> users = new List<User>();

            List<User> testList = new List<User>
            {
                otherUser
            };

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.HasUsers(testList);
            Assert.IsFalse(result);
        }
    }
    public class Duration
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void Duration_HoursDifferece_ReturnsNotZero()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            end = end.AddHours(1);
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);

            List<User> users = new List<User>();

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.Duration();

            Assert.NotZero(result);
        }
        [Test]
        public void Duration_MinutesDifference_ReturnsZero()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            end = end.AddMinutes(1);
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);

            List<User> users = new List<User>();

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.Duration();

            Assert.Zero(result);
        }
    }
    
    public class ToString
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void ToString_CorrectAppointment_ReturnsTitle()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);

            List<User> users = new List<User>();

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.ToString();

            Assert.AreEqual(appointment.Title,result);
        }
    }

    public class IsDate
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void IsDate_AllParamsAreCorrect_ReturnsTrue()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);

            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            List<User> users = new List<User>();

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.IsDate(year, month, day);

            Assert.IsTrue(result);
        }
        [Test]
        public void IsDate_YearIsBad_ReturnsFalse()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);

            int year = date.Year+1;
            int month = date.Month;
            int day = date.Day;

            List<User> users = new List<User>();

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.IsDate(year, month, day);

            Assert.IsFalse(result);
        }
        [Test]
        public void IsDate_MonthIsBad_ReturnsFalse()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);

            int year = date.Year;
            int month = date.Month + 1;
            int day = date.Day;

            List<User> users = new List<User>();

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.IsDate(year, month, day);

            Assert.IsFalse(result);
        }
        [Test]
        public void IsDate_DayIsBad_ReturnsFalse()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);

            int year = date.Year;
            int month = date.Month;
            int day = date.Day + 1;

            List<User> users = new List<User>();

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.IsDate(year, month, day);

            Assert.IsFalse(result);
        }
    }

    public class GetDescription
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void GetDescription_ValidAppointment_ReturnsDescription()
        {
            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            string description = "";
            string creatorMail = "a@a";
            User creator = new User(creatorMail);

            List<User> users = new List<User>();

            var appointment = new Appointment(title, date, start, end, description, creator, users);

            var result = appointment.Description;

            Assert.AreEqual(result, description);
        }
    }
}