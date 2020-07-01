using NUnit.Framework;
using Project;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MainWindowTest
{
    public class StoreUserEmailForm
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Apartment(ApartmentState.STA)]
        public static void StoreUserEmailForm_ValidEmailAndList_ListIsNotZero()
        {

            List<User> users = new List<User>();
            string validEmail = "a@a";
            MainWindow.StoreUserEmailForm(validEmail, users);

            var result = users.Count;
            Assert.NotZero(result);
        }

        [Test, Apartment(ApartmentState.STA)]
        public static void StoreUserEmailForm_ValidEmailAndNullList_ListIsNull()
        {

            List<User> users = null;
            string validEmail = "a@a";
            MainWindow.StoreUserEmailForm(validEmail, users);

            var result = users;
            Assert.IsNull(result);
        }
    }

    public  class ProcessHourForForm
    {
        [SetUp]
        public  void Setup()
        {
        }
        [Test, Apartment(ApartmentState.STA)]
        public  void ProcessHourForForm_ValidHourAndAMPM_ReturnsInt()
        {
            ComboBox hour = new ComboBox();
            hour.Items.Add("1");
            ComboBox ampm = new ComboBox();
            ampm.Items.Add("am");
            hour.SelectedIndex = 0;
            ampm.SelectedIndex = 0;

            var result = MainWindow.ProcessHourForForm(hour, ampm);

            Assert.AreEqual(result, 1);
        }
        [Test, Apartment(ApartmentState.STA)]
        public  void ProcessHourForForm_ValidHourAndSecondAMPM_ReturnsInt()
        {
            ComboBox hour = new ComboBox();
            hour.Items.Add("1");
            ComboBox ampm = new ComboBox();
            ampm.Items.Add("am");
            ampm.Items.Add("pm");
            hour.SelectedIndex = 0;
            ampm.SelectedIndex = 1;

            var result = MainWindow.ProcessHourForForm(hour, ampm);

            Assert.AreEqual(result, 1+12);
        }
        [Test, Apartment(ApartmentState.STA)]
        public  void ProcessHourForForm_ValidHourNullAMPM_ReturnsInt()
        {
            ComboBox hour = new ComboBox();
            hour.Items.Add("1");
            ComboBox ampm = null;
            hour.SelectedIndex = 0;

            var result = MainWindow.ProcessHourForForm(hour, ampm);

            Assert.AreEqual(result, -1);
        }
        [Test, Apartment(ApartmentState.STA)]
        public  void ProcessHourForForm_NullHourValidAMPM_ReturnsInt()
        {
            ComboBox hour = null;
            ComboBox ampm = new ComboBox();
            ampm.Items.Add("am");
            ampm.SelectedIndex = 0;

            var result = MainWindow.ProcessHourForForm(hour, ampm);

            Assert.AreEqual(result, -1);
        }
        [Test, Apartment(ApartmentState.STA)]
        public  void ProcessHourForForm_NullHourAndAMPM_ReturnsInt()
        {
            ComboBox hour = null;
            ComboBox ampm = null;

            var result = MainWindow.ProcessHourForForm(hour, ampm);

            Assert.AreEqual(result, -1);
        }
    }
    public  class ProcessMinuteForForm
    {
        [SetUp]
        public  void Setup()
        {
        }
        [Test, Apartment(ApartmentState.STA)]
        public  void ProcessMinuteForForm_firstIndexSelected_ReturnsInt()
        {
            ComboBox minute = new ComboBox();
            minute.Items.Add("00");
            minute.Items.Add("15");
            minute.Items.Add("30");
            minute.Items.Add("45");
            minute.SelectedIndex = 0;

            var result = MainWindow.ProcessMinuteForForm(minute);

            Assert.AreEqual(result, 0);
        }
        [Test, Apartment(ApartmentState.STA)]
        public  void ProcessMinuteForForm_secondIndexSelected_ReturnsInt()
        {
            ComboBox minute = new ComboBox();
            minute.Items.Add("00");
            minute.Items.Add("15");
            minute.Items.Add("30");
            minute.Items.Add("45");
            minute.SelectedIndex = 1;


            var result = MainWindow.ProcessMinuteForForm(minute);

            Assert.AreEqual(result, 15);
        }
        [Test, Apartment(ApartmentState.STA)]
        public  void ProcessMinuteForForm_thirdIndexSelected_ReturnsInt()
        {
            ComboBox minute = new ComboBox();
            minute.Items.Add("00");
            minute.Items.Add("15");
            minute.Items.Add("30");
            minute.Items.Add("45");
            minute.SelectedIndex = 2;


            var result = MainWindow.ProcessMinuteForForm(minute);

            Assert.AreEqual(result, 30);
        }
        [Test, Apartment(ApartmentState.STA)]
        public  void ProcessMinuteForForm_fourthIndexSelected_ReturnsInt()
        {
            ComboBox minute = new ComboBox();
            minute.Items.Add("00");
            minute.Items.Add("15");
            minute.Items.Add("30");
            minute.Items.Add("45");
            minute.SelectedIndex = 3;


            var result = MainWindow.ProcessMinuteForForm(minute);

            Assert.AreEqual(result, 45);
        }
        [Test, Apartment(ApartmentState.STA)]
        public  void ProcessHourForForm_NullMinute_ReturnsInt()
        {
            ComboBox minute = null;

            var result = MainWindow.ProcessMinuteForForm(minute);

            Assert.AreEqual(result, -1);
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

    public class FillWeekListWithAppointments
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test, Apartment(ApartmentState.STA)]
        public void FillWeekListWithAppointments_NoAppointmentsValidDate_AppointmentsZero()
        {
            MainWindow mainwindow = new MainWindow();
            List<Appointment> appointments = new List<Appointment>();
            DateTime datetime = DateTime.Now;

            mainwindow.FillWeekListWithAppointments(appointments, datetime);

            var result = appointments.Count;
            Assert.Zero(result);
        }
        [Test, Apartment(ApartmentState.STA)]
        public void FillWeekListWithAppointments_NullAppointmentsValidDate_WeekAppointmentsNull()
        {
            MainWindow mainwindow = new MainWindow();
            List<Appointment> appointments = null;
            DateTime datetime = DateTime.Now;

            mainwindow.FillWeekListWithAppointments(appointments, datetime);

            var result = appointments;
            Assert.IsNull(result);
        }
    }
    public class FillWeekListWithNumbers
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test, Apartment(ApartmentState.STA)]
        public void FillWeekListWithNumbers_NoNumberValidDate_NumbersNoZero()
        {
            List<int> numbers= new List<int>();
            DateTime datetime = DateTime.Now;

            MainWindow.FillWeekListWithNumbers(numbers, datetime);

            var result = numbers.Count;
            Assert.NotZero(result);
        }
        [Test, Apartment(ApartmentState.STA)]
        public void FillWeekListWithNumbers_NullNumberValidDate_NumberNull()
        {
            List<int> numbers = null;
            DateTime datetime = DateTime.Now;

            MainWindow.FillWeekListWithNumbers(numbers, datetime);

            var result = numbers;
            Assert.IsNull(result);
        }
    }

    public class CreateAppointmentLabel
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test, Apartment(ApartmentState.STA)]
        public void CreateAppointmentLabel_NullAppointmentsMonthView_ReturnsNull()
        {
            Appointment appointment = null;
            MainWindow.ViewMode modeview = MainWindow.ViewMode.Months;

            var result = MainWindow.CreateAppointmentLabel(appointment, modeview);

            Assert.IsNull(result);
        }
        [Test, Apartment(ApartmentState.STA)]
        public void CreateAppointmentLabel_NullAppointmentsWeekView_ReturnsNull()
        {
            Appointment appointment = null;
            MainWindow.ViewMode modeview = MainWindow.ViewMode.Weeks;

            var result = MainWindow.CreateAppointmentLabel(appointment, modeview);

            Assert.IsNull(result);
        }
        [Test, Apartment(ApartmentState.STA)]
        public void CreateAppointmentLabel_ValidAppointmentsMonthView_ReturnsLabelRight()
        {
            MainWindow.ViewMode modeview = MainWindow.ViewMode.Months;

            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            string description = "";
            User creator = null;
            List<User> users = new List<User>();

            Appointment appointment = new Appointment(title, date, start, end, description, creator, users);

            Label label = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top,
                FontSize = 12,
                Content = "test"
            };

            var result = MainWindow.CreateAppointmentLabel(appointment, modeview);

            Assert.AreEqual(label.HorizontalAlignment, result.HorizontalAlignment);
        }
        [Test, Apartment(ApartmentState.STA)]
        public void CreateAppointmentLabel_ValidAppointmentsWeekView_ReturnsLabelCenter()
        {
            MainWindow.ViewMode modeview = MainWindow.ViewMode.Weeks;

            string title = "sample_title";
            DateTime date = new DateTime();
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            string description = "";
            User creator = null;
            List<User> users = new List<User>();

            Appointment appointment = new Appointment(title, date, start, end, description, creator, users);

            Label label = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 12,
                Content = "test"
            };

            var result = MainWindow.CreateAppointmentLabel(appointment, modeview);

            Assert.AreEqual(label.HorizontalAlignment, result.HorizontalAlignment);
        }

    }
    public class CreateNumberLabel
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test, Apartment(ApartmentState.STA)]
        public void CreateNumberLabel_ValidTextMonthView_ReturnsLabeLeftAlignment()
        {
            string contentText = "";
            MainWindow.ViewMode modeview = MainWindow.ViewMode.Months;
            Label label = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 30,
                Height = 240,
                Content = contentText
            };
            var result = MainWindow.CreateNumberLabel(contentText, modeview);

            Assert.AreEqual(label.HorizontalAlignment, result.HorizontalAlignment);
        }
        public void CreateNumberLabel_ValidTextWeekView_ReturnsLabelCenterAlignment()
        {
            string contentText = "";
            MainWindow.ViewMode modeview = MainWindow.ViewMode.Weeks;
            Label label = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 24,
                Content = contentText,
            };
            var result = MainWindow.CreateNumberLabel(contentText, modeview);

            Assert.AreEqual(label.HorizontalAlignment, result.HorizontalAlignment);
        }
        [Test, Apartment(ApartmentState.STA)]
        public void CreateNumberLabel_NullTextValidView_ReturnsLabelNullContent()
        {
            string contentText = null;
            MainWindow.ViewMode modeview = MainWindow.ViewMode.Months;
            Label label = new Label();
            var result = MainWindow.CreateNumberLabel(contentText, modeview);

            Assert.AreEqual(result.Content, label.Content);
        }

    }
    //TODO SERIALIZE
    public class SerializeUsers
    {
        [SetUp]
        public void Setup()
        {
        }
        [Test]
        public void SerializeUsers_ValiduserListAndFilePath_FileIsWritten()
        {

        }
    }

}