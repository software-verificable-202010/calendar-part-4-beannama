using NUnit.Framework;
using Project;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MainWindowTest
{
    public class MainWindowTest
    {
        [SetUp]
        public void Setup()
        {
        }
    }
    public class StoreUserEmailForm
    {
        [SetUp]
        public void Setup()
        {
            
        }
        [Test]
        public void StoreUserEmailForm_EmailIsValid_ListIsNotZero()
        {
            _ = Dispatcher.CurrentDispatcher;
            if (Application.Current != null)
            {
                _ = Application.Current.Dispatcher;
                string validEmail = "a@a";
                TextBox TB_tb = new TextBox
                {
                    Text = validEmail
                };
                string email = TB_tb.Text;
                List<User> users = new List<User>();


                User user = new User(email);
                users.Add(user);

                var result = users.Count;

                Assert.NotZero(result);
            }

            
        }
    }
    public class DeserializeUsers
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void DeserializeUsers_FileContainUsers_ListIsNotZero()
        {
            string filePath = string.Format(CultureInfo.CurrentCulture, "{0}\\users.txt", Environment.CurrentDirectory);

            var users = MainWindow.DeserializeUsers(filePath);

            var result = users.Count;

            Assert.NotZero(result);
        }
        [Test]
        public void DeserializeUsers_FileDontExist_ListIsZero()
        {
            string filePath = string.Format(CultureInfo.CurrentCulture, "{0}\\emptyfile.txt", Environment.CurrentDirectory);


            var users = MainWindow.DeserializeUsers(filePath);

            var result = users.Count;

            Assert.Zero(result);
        }
    }
    public class DeserializeAppointments
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void DeserializeAppointments_FileContainAppointments_ListIsNotZero()
        {
            string filePath = string.Format(CultureInfo.CurrentCulture, "{0}\\appointments.txt", Environment.CurrentDirectory);

            var appointments = MainWindow.DeserializeAppointments(filePath);

            var result = appointments.Count;

            Assert.NotZero(result);
        }
        [Test]
        public void DeserializeAppointments_FileDontExist_ListIsZero()
        {
            string filePath = string.Format(CultureInfo.CurrentCulture, "{0}\\emptyfile.txt", Environment.CurrentDirectory);


            var appointments = MainWindow.DeserializeAppointments(filePath);

            var result = appointments.Count;

            Assert.Zero(result);
        }
    }
    public class IsValid
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsValidEmail_ValidEmail_ReturnsTrue()
        {
            string validMail = "a@a";

            var result = MainWindow.IsValidEmail(validMail);

            Assert.IsTrue(result);
        }
        [Test]
        public void IsValidEmail_InvalidEmail_ReturnsFalse()
        {
            string invalidMail = "";

            var result = MainWindow.IsValidEmail(invalidMail);

            Assert.IsFalse(result);
        }
        [Test]
        public void IsValidEmail_NullEmail_ReturnsFalse()
        {
            string invalidMail = null;

            var result = MainWindow.IsValidEmail(invalidMail);

            Assert.IsFalse(result);
        }
    }

}