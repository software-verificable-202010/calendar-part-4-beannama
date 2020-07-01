using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace Project
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region CONSTANTS
        private const int weeksInMonth = 6;
        private const int saturdayColumn = 5;
        private const int sundayColumn = 6;
        private const int calendarLargeMonths = 7;
        private const int sundayOnCalendar = 0;
        private const int oneDay = 1;
        private const int firstWeek = 1;

        private const int oneWeekOnDays = 7;
        private const int addIfPMHour = 12;
        private const int oneHour = 1;
        private const int pmSelected = 1;
        private const int noSeconds = 0;
        private const int indexFor0 = 0;
        private const int indexFor15 = 1;
        private const int indexFor30 = 2;
        private const int indexFor45 = 3;
        private const int startBeforeEndTime = -1;

        private const int numberLabelWidth = 30;
        private const int numberLabelHeight = 240;
        private const int fontSizeForFeedbackLabel = 24;
        private const int sizeForWeekLabel = 24;
        private const int sizeForAppointmentLabel = 12;

        private const int firstDayOfMonth = 1;

        private readonly string binaryFilePath = string.Format(CultureInfo.CurrentCulture, "{0}\\appointments.txt", Environment.CurrentDirectory);
        private readonly string usersFilePath = string.Format(CultureInfo.CurrentCulture, "{0}\\users.txt", Environment.CurrentDirectory);



        #endregion

        private DateTime targetedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, firstDayOfMonth);
        private readonly List<int> weekNumbers = new List<int>();

        private readonly List<Appointment> weekAppointments = new List<Appointment>();
        private readonly List<Appointment> appointments = new List<Appointment>();
        private readonly List<Appointment> userAppointments = new List<Appointment>();

        private User logedUser;
        private readonly List<User> users = new List<User>();
        private List<User> availableUsers = new List<User>();
        private readonly List<User> invitedUsers = new List<User>();

        private enum ViewMode{ 
            Weeks,
            Months,
        };
        private ViewMode modeView;

        
        public MainWindow()
        {
            InitializeComponent();
            appointments = DeserializeAppointments(binaryFilePath);
            users = DeserializeUsers(usersFilePath);
        }
        public static List<Appointment> DeserializeAppointments(string filepath)
        {
            List<Appointment> appointments = new List<Appointment>();
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists(filepath) == true)
            {
                Stream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                appointments = (List<Appointment>)formatter.Deserialize(stream);
                stream.Close();
                return appointments;
            }
            else
            {
                return appointments;
            }
        }
        public static List<User> DeserializeUsers(string filepath)
        {
            List<User> users = new List<User>();
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists(filepath) == true)
            {
                Stream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                users = (List<User>)formatter.Deserialize(stream);
                stream.Close();
                return users;
            }
            else
            {
                return users;
            }
        }


        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = TB_Email.Text;
            if (IsValidEmail(email))
            {
                bool containsEmail = users.Any(item => item.Email == email);

                if (!containsEmail)
                {
                    StoreUserEmailForm();
                    SerializeUsers(users, usersFilePath);
                }
                logedUser = new User(email);
                HideLoginView();
                CreateCalendar();
            }
            else
            {
                LoginFeedback_Text.Text = string.Format(CultureInfo.CurrentCulture, "Invalid Email!"); ;
                LoginFeedback_Text.Foreground = Brushes.Red;
                LoginFeedback_Text.FontSize = fontSizeForFeedbackLabel;
            }

        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch (FormatException)
            {
                return false;
            }
        }
        private void StoreUserEmailForm()
        {
            try
            {
                string email = TB_Email.Text;
                User user = new User(email);
                users.Add(user);
            }
            catch (ArgumentException e) when (e.ParamName == "…")
            {

                LoginFeedback_Text.Text = string.Format(CultureInfo.CurrentCulture, "Error!");
                LoginFeedback_Text.Foreground = Brushes.Red;
                LoginFeedback_Text.FontSize = fontSizeForFeedbackLabel;
            }
        }
        public static void SerializeUsers(List<User> users, string filepath)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, users);
            stream.Close();
        }
        private void HideLoginView()
        {
            string emailUser = TB_Email.Text.Split("@")[0];
            EmailTextBlock.Text = string.Format(CultureInfo.CurrentCulture, "Welcome\n " + emailUser);
            NavigationGrid.Visibility = Visibility.Visible;
            LoginGrid.Visibility = Visibility.Hidden;
            CheckVisibility();
        }
   

        private void LB_Appointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LB_Appointments.SelectedIndex != -1)
            {
                int currentItemIndex = LB_Appointments.SelectedIndex;
                Appointment currentAppointment = userAppointments[currentItemIndex];

                TB_EditAppointmentTitle.Text = currentAppointment.ToString();
                DatePicker_EditDateAppointment.SelectedDate = currentAppointment.Date;
                TB_EditDescriptionAppointment.Text = currentAppointment.Description;

                LB_EditUserAvailable.ItemsSource = users;
                LB_EditUserInvited.ItemsSource = currentAppointment.InvitedUsers;
            }
        }
        private void MoveForwardButton_Click(object sender, RoutedEventArgs e)
        {
            CheckVisibility();

            if (modeView == ViewMode.Months)
            {
                targetedDate = targetedDate.AddMonths(1);
            }
            else if (modeView == ViewMode.Weeks)
            {
                targetedDate = targetedDate.AddDays(7);
            }
            ResetCalendar();
        }
        private void MoveBackwardButton_Click(object sender, RoutedEventArgs e)
        {
            CheckVisibility();

            if (modeView == ViewMode.Months)
            {
                targetedDate = targetedDate.AddMonths(-1);
            }
            else if (modeView == ViewMode.Weeks)
            {
                targetedDate = targetedDate.AddDays(-oneWeekOnDays);
            }
            ResetCalendar();
        }
        private void ChangeViewModComboBox(object sender, SelectionChangedEventArgs e)
        {
            int monthIndex = 0;
            int weekIndex = 1;

            if (CB_Mode.SelectedIndex.Equals(monthIndex))
            {
                modeView = ViewMode.Months;
            }
            else if (CB_Mode.SelectedIndex.Equals(weekIndex))
            {
                modeView = ViewMode.Weeks;
            }
        }
        private void CloseDropdownViewComboBox(object sender, EventArgs e)
        {
            CheckVisibility();
            targetedDate = new DateTime(targetedDate.Year, targetedDate.Month, firstDayOfMonth);
            ResetCalendar();
        }


        private void AppointmentManagmentButton_Click(object sender, RoutedEventArgs e)
        {
            ResetCalendar();
            DisplayAppointmentsManagmentView();
        }
        private void DisplayAppointmentsManagmentView()
        {
            AppointmentFormGrid.Visibility = Visibility.Hidden;
            AppointmentEditationGrid.Visibility = Visibility.Hidden;

            if (Btn_AppointmentManagment.Content.ToString() != "Back")
            {
                AppointmentContainerGrid.Visibility = Visibility.Visible;

                Btn_AppointmentManagment.Content = string.Format(CultureInfo.CurrentCulture, "Back");
                MonthCalendarGrid.Visibility = Visibility.Hidden;
                WeekCalendarGrid.Visibility = Visibility.Hidden;
            }
            else
            {
                AppointmentContainerGrid.Visibility = Visibility.Hidden;
                Btn_AppointmentManagment.Content = string.Format(CultureInfo.CurrentCulture, "Appointment \n Managment");
                CheckVisibility();
            }
        }
        private void CreateAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyDataBinding();
            DisplayAppointmentFormView();
            TextBlockFeedback.Text = String.Empty;
        }
        private void DisplayAppointmentFormView()
        {
            AppointmentFormGrid.Visibility = Visibility.Visible;
            AppointmentEditationGrid.Visibility = Visibility.Hidden;

            MonthCalendarGrid.Visibility = Visibility.Hidden;
            WeekCalendarGrid.Visibility = Visibility.Hidden;
        }
        private void EditAppointmentsButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyDataBinding();
            DisplayEditAppointmentsView();
        }
        private void DisplayEditAppointmentsView()
        {
            AppointmentEditationGrid.Visibility = Visibility.Visible;
            AppointmentFormGrid.Visibility = Visibility.Hidden;
        }


        private void CancelAppointmentForm_Click(object sender, RoutedEventArgs e)
        {
            ClearAppointmentForm();
            Btn_AppointmentManagment.Content = "Appointment \n Managment";
            ResetCalendar();
            CheckVisibility();
        }
        private void ClearAppointmentForm()
        {
            TB_TitleAppointment.Text = String.Empty;
            DatePicker_DateAppointment.SelectedDate = null;

            CB_StartTimeHourAppointment.SelectedItem = null;
            CB_StartTimeMinuteAppointment.SelectedItem = null;
            CB_StartTimeAMPMAppointment.SelectedItem = null;

            CB_EndTimeHourAppointment.SelectedItem = null;
            CB_EndTimeMinuteAppointment.SelectedItem = null;
            CB_EndTimeAMPMAppointment.SelectedItem = null;

            TB_DescriptionAppointment.Text = String.Empty;

            LB_UsersInvited.ItemsSource = null;
        }
        private void SaveAppointmentFormClick(object sender, RoutedEventArgs e)
        {
            StoreAppointmentForm();
            SerializeAppointments(appointments, binaryFilePath);
            ClearAppointmentForm();

        }
        private void StoreAppointmentForm()
        {
            try
            {
                string title = TB_TitleAppointment.Text;
                DateTime date = DatePicker_DateAppointment.SelectedDate.Value;

                int startHourAux = ProcessHourForForm(CB_StartTimeHourAppointment, CB_StartTimeAMPMAppointment);
                int startMinuteAux = ProcessMinuteForForm(CB_StartTimeMinuteAppointment);

                DateTime startTime = new DateTime(date.Year, date.Month, date.Day, startHourAux, startMinuteAux, noSeconds);

                int endHourAux = ProcessHourForForm(CB_EndTimeHourAppointment, CB_EndTimeAMPMAppointment);
                int endMinuteAux = ProcessMinuteForForm(CB_EndTimeMinuteAppointment);

                DateTime endTime = new DateTime(date.Year, date.Month, date.Day, endHourAux, endMinuteAux, noSeconds);

                string description = TB_DescriptionAppointment.Text;

                int comparison = TimeSpan.Compare(startTime.TimeOfDay, endTime.TimeOfDay);
                if (comparison != startBeforeEndTime)
                {
                    throw new InvalidOperationException();
                }
                else
                {
                    Appointment appointment = new Appointment(title, date, startTime, endTime, description, logedUser, invitedUsers);
                    appointments.Add(appointment);

                    TextBlockFeedback.Text = string.Format(CultureInfo.CurrentCulture, "Saved!");
                    TextBlockFeedback.Foreground = Brushes.Green;
                    TextBlockFeedback.FontSize = fontSizeForFeedbackLabel;
                }
            }
            catch (InvalidOperationException)
            {
                TextBlockFeedback.Text = string.Format(CultureInfo.CurrentCulture, "Error");
                TextBlockFeedback.Foreground = Brushes.Red;
                TextBlockFeedback.FontSize = fontSizeForFeedbackLabel;
            }

        }
        private static int ProcessHourForForm(ComboBox hour, ComboBox AMPM)
        {
            if (hour == null || AMPM == null)
            {
                return -1;
            }
            int hourReturned = hour.SelectedIndex + oneHour;
            if (AMPM.SelectedIndex == pmSelected) hourReturned += addIfPMHour;

            return hourReturned;
        }
        private static int ProcessMinuteForForm(ComboBox minute)
        {
            if (minute == null)
            {
                return -1;
            }

            int auxMinute = minute.SelectedIndex;
            int minuteReturned = auxMinute;

            if (auxMinute == indexFor0)
            {
                minuteReturned = 0;
            }
            else if (auxMinute == indexFor15)
            {
                minuteReturned = 15;
            }
            else if (auxMinute == indexFor30)
            {
                minuteReturned = 30;
            }
            else if (auxMinute == indexFor45)
            {
                minuteReturned = 45;
            }
            return minuteReturned;
        }
        public static void SerializeAppointments(List<Appointment> appointments, string filepath)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, appointments);
            stream.Close();
        }


        private void AddUserOfAppointmentList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string currentItemText = LB_UsersAvailable.SelectedItem.ToString();
                int currentItemIndex = LB_UsersAvailable.SelectedIndex;

                LB_UsersInvited.Items.Add(currentItemText);
                invitedUsers.Add(new User(currentItemText));

                if (availableUsers != null)
                {
                    availableUsers.RemoveAt(currentItemIndex);
                }
                ApplyDataBinding();
                TextBlockFeedback.Text = "";
            }
            catch (NullReferenceException)
            {
                TextBlockFeedback.Text = string.Format(CultureInfo.CurrentCulture, "Error!");
            }

        }
        private void RemoveUserOfAppointmentList_Click(object sender, RoutedEventArgs e)
        {
            string currentItemText = LB_UsersInvited.SelectedItem.ToString();
            availableUsers.Add(new User(currentItemText));

            invitedUsers.Remove(new User(currentItemText));
            LB_UsersInvited.Items.RemoveAt(LB_UsersInvited.Items.IndexOf(LB_UsersInvited.SelectedItem));
            ApplyDataBinding();
        }
        private void Btn_ApplyChangeAppointments_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int currentItemIndex = LB_Appointments.SelectedIndex;
                Appointment selectedAppointment = userAppointments[currentItemIndex];

                selectedAppointment.Title = TB_EditAppointmentTitle.Text;
            }
            catch (ArgumentException)
            {

            }
            ApplyDataBinding();
        }
        private void ApplyDataBinding()
        {
            LB_UsersAvailable.ItemsSource = null;
            LB_Appointments.ItemsSource = null;
            LB_UsersInvited.ItemsSource = null;
            availableUsers = users;

            LB_Appointments.ItemsSource = userAppointments;
            LB_UsersAvailable.ItemsSource = availableUsers;
        }


        private void ResetCalendar()
        {
            ChangeTitle();
            CleanCalendar();
            CreateCalendar();
        }
        private void ChangeTitle()
        {
            MonthTextBlock.Text = targetedDate.ToString("MMMM", CultureInfo.CurrentCulture);
            YearTextBlock.Text = targetedDate.Year.ToString(CultureInfo.CurrentCulture);
        }
        private void CreateCalendar()
        {
            ChangeTitle();
            AddDaysToCalendar();
            AddAppointmentsToCalendar();
        }


        private void AddDaysToCalendar()
        {
            DateTime auxiliarDate = targetedDate;
            if (modeView == ViewMode.Months)
            {
                for (int weekNumber = 0; weekNumber < weeksInMonth; weekNumber++)
                {
                    FillWeekListWithNumbers(weekNumbers, auxiliarDate);
                    FillWeekWithNumber(weekNumber, weekNumbers);
                    weekNumbers.Clear();

                    auxiliarDate = auxiliarDate.AddDays(oneWeekOnDays);
                }
            }
            else if (modeView == ViewMode.Weeks)
            {
                FillWeekListWithNumbers(weekNumbers, auxiliarDate);
                FillWeekWithNumber(firstWeek, weekNumbers);
                weekNumbers.Clear();
            }
        }
        private static void FillWeekListWithNumbers(List<int> listNumber, DateTime targetedDate)
        {
            if (listNumber == null)
            {
                return;
            }
            DateTime auxDate = targetedDate;


            while (auxDate.DayOfWeek.ToString() != "Monday")
            {
                auxDate = auxDate.AddDays(-1);
            }
            int numberToFill = auxDate.Day;
            for (int count = 1; count <= oneWeekOnDays; count++)
            {
                listNumber.Add(numberToFill);
                auxDate = auxDate.AddDays(1);
                numberToFill = auxDate.Day;
            }
        }
        private void FillWeekWithNumber(int rowNumber, List<int> listNumber)
        {
            if (listNumber == null)
            {
                return;
            }
            int counter = 0;
            foreach (int number in listNumber)
            {
                Label numberLabel = CreateNumberLabel(number.ToString(CultureInfo.CurrentCulture));
                AddNumberToDay(numberLabel, rowNumber, counter);
                counter++;
            }
        }
        private void AddNumberToDay(Label numberLabel, int targetedRow, int targetedCol)
        {
            if (modeView == ViewMode.Months)
            {
                Grid grid = new Grid { Name = "DayGrid" };
                RowDefinition rowDefinitionAuto = new RowDefinition
                {
                    Height = GridLength.Auto
                };
                RowDefinition rowDefinitionStar = new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star)
                };
                grid.RowDefinitions.Add(rowDefinitionAuto);
                grid.RowDefinitions.Add(rowDefinitionStar);
                Grid.SetColumn(grid, targetedCol);
                Grid.SetRow(grid, targetedRow);
                grid.Children.Add(numberLabel);

                DaysOfMonthCalendarGrid.Children.Add(grid);
            }
            else if (modeView == ViewMode.Weeks)
            {
                Grid.SetColumn(numberLabel, targetedCol + 1);
                Grid.SetRow(numberLabel, 1);
                NumberOfWeekWeekCalendarGrid.Children.Add(numberLabel);
            }
        }
        private Label CreateNumberLabel(string contentText)
        {
            Label label = new Label();
            if (modeView == ViewMode.Months)
            {
                label = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Width = numberLabelWidth,
                    Height = numberLabelHeight,
                    Content = contentText
                };
            }
            else if (modeView == ViewMode.Weeks)
            {
                label = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = sizeForWeekLabel,
                    Content = contentText,
                    Foreground = Brushes.White
                };
            }
            return label;
        }

        private void AddAppointmentsToCalendar()
        {
            DateTime auxiliarDate = targetedDate;

            if (modeView == ViewMode.Months)
            {
                for (int weekNumber = 0; weekNumber < weeksInMonth; weekNumber++)
                {
                    FillWeekListWithAppointments(weekAppointments, auxiliarDate);
                    FillWeekWithAppointments(weekNumber, weekAppointments);

                    weekAppointments.Clear();

                    auxiliarDate = auxiliarDate.AddDays(oneWeekOnDays);
                }
            }
            else if (modeView == ViewMode.Weeks)
            {
                FillWeekListWithAppointments(weekAppointments, auxiliarDate);
                FillWeekWithAppointments(sundayOnCalendar, weekAppointments);
            }
        }
        private void FillWeekListWithAppointments(List<Appointment> weekAppointments, DateTime targetedDate)
        {
            if (weekAppointments == null)
            {
                return;
            }

            int month = targetedDate.Month;
            int year = targetedDate.Year;

            DateTime auxDate = targetedDate;

            while (auxDate.DayOfWeek.ToString() != "Monday")
            {
                auxDate = auxDate.AddDays(-1);
            }
            for (int day = targetedDate.Day; day < (auxDate.Day + oneWeekOnDays); day++)
            {
                foreach (Appointment appointment in appointments)
                {
                    if (appointment.IsDate(year, month, day) && appointment.HasUser(logedUser))
                    {
                        weekAppointments.Add(appointment);
                        userAppointments.Add(appointment);
                    }
                }
            }
        }
        private void FillWeekWithAppointments(int rowNumber, List<Appointment> weekAppointments)
        {
            if (weekAppointments == null)
            {
                return;
            }

            foreach (Appointment appointment in weekAppointments)
            {
                int dayColumn = (int)appointment.Date.DayOfWeek;
                if (dayColumn != sundayOnCalendar)
                {
                    dayColumn -= oneDay;
                }
                else
                {
                    dayColumn = sundayColumn;
                }

                if (modeView == ViewMode.Weeks)
                {
                    for (int duration = 1; duration <= appointment.Duration(); duration++)
                    {
                        Label appointmentLabel = CreateAppointmentLabel(appointment);

                        rowNumber = duration;
                        AddAppointmentToDay(appointmentLabel, rowNumber, dayColumn);
                    }
                }
                else
                {
                    Label appointmentLabel = CreateAppointmentLabel(appointment);
                    AddAppointmentToDay(appointmentLabel, rowNumber, dayColumn);
                }
            }
        }
        private Label CreateAppointmentLabel(Appointment appointment)
        {
            if (appointment == null)
            {
                return null;
            }
            Label label = new Label();
            string title = appointment.Title;
            string startTime = appointment.StartTime.ToString("HH:mm", CultureInfo.CurrentCulture);
            string endTime = appointment.EndTime.ToString("HH:mm", CultureInfo.CurrentCulture);
            string description = appointment.Description;

            string appointmentString = string.Format(CultureInfo.CurrentCulture, "{0}\n{1}\n{2}-{3}", title, description, startTime, endTime);
            if (modeView == ViewMode.Months)
            {
                label = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    FontSize = sizeForAppointmentLabel,
                    Content = appointmentString
                };
            }
            else if (modeView == ViewMode.Weeks)
            {
                label = new Label
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = sizeForAppointmentLabel,
                    Content = appointmentString
                };
            }

            return label;
        }
        private void AddAppointmentToDay(Label appointmenLabel, int targetedRow, int targetedCol)
        {
            if (modeView == ViewMode.Months)
            {
                Grid.SetColumn(appointmenLabel, targetedCol);
                Grid.SetRow(appointmenLabel, targetedRow);
                DaysOfMonthCalendarGrid.Children.Add(appointmenLabel);
            }
            else if (modeView == ViewMode.Weeks)
            {
                Grid.SetColumn(appointmenLabel, targetedCol + 1);
                TimesOfDaysWeekCalendarGrid.Children.Add(appointmenLabel);
            }
        }

        private void CleanCalendar()
        {
            if (modeView == ViewMode.Months)
            {
                DaysOfMonthCalendarGrid.Children.Clear();
                AddColorToCalendar(calendarLargeMonths, saturdayColumn);
                AddColorToCalendar(calendarLargeMonths, sundayColumn);
            }
            else if (modeView == ViewMode.Weeks)
            {
                NumberOfWeekWeekCalendarGrid.Children.Clear();
            }
        }
        private void AddColorToCalendar(int calendarLarge, int targetedCol)
        {
            Border colorWeekend = new Border
            {
                Background = Brushes.AliceBlue,
            };
            Grid.SetColumn(colorWeekend, targetedCol);
            Grid.SetRowSpan(colorWeekend, calendarLarge);

            if (modeView == ViewMode.Months)
            {
                DaysOfMonthCalendarGrid.Children.Add(colorWeekend);
            }
        }


        private void CheckVisibility()
        {
            if (modeView == ViewMode.Months)
            {
                DisplayMonthView();
            }
            else if (modeView == ViewMode.Weeks)
            {
                DisplayWeekView();
            }
        }
        private void DisplayMonthView()
        {
            MonthCalendarGrid.Visibility = Visibility.Visible;

            AppointmentContainerGrid.Visibility = Visibility.Hidden;
            WeekCalendarGrid.Visibility = Visibility.Hidden;
        }
        private void DisplayWeekView()
        {
            WeekCalendarGrid.Visibility = Visibility.Visible;

            AppointmentContainerGrid.Visibility = Visibility.Hidden;
            MonthCalendarGrid.Visibility = Visibility.Hidden;
        }
    }
}
