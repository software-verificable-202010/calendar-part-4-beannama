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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Proyecto_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Consts
        static readonly int firstDayOfMonth = 1;
        static readonly string binaryFilePath = Environment.CurrentDirectory + "\\appointments.txt";

        readonly int  saturdayColumn = 5;
        readonly int sundayColumn = 6;
        readonly int numberLabelWidth = 30;
        readonly int numberLabelHeight = 240;
        readonly int oneWeekOnDays = 7;
        readonly int sizeForWeekLabel = 24;
        readonly int calendarLargeMonths = 7;
        readonly int addIfPMHour = 12;
        readonly int oneHour = 1;
        readonly int pmSelected = 1;
        readonly int noSeconds = 0;
        readonly int indexFor0 = 0;
        readonly int indexFor15 = 1;
        readonly int indexFor30 = 2;
        readonly int indexFor45 = 3;
        readonly int startBeforeEndTime = -1;
        readonly int fontSizeForFeedback = 24;

        List<Appointment> appointments = new List<Appointment>();

        DateTime targetedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, firstDayOfMonth);
        public enum ViewModes{ 
            Weeks,
            Months,
        };
        public ViewModes modeView;
        


        public MainWindow()
        {
            InitializeComponent();
            appointments = DeserializeAppointments(binaryFilePath);
            CreateCalendar();
        }

        
        public void ChangeTitle()
        {
            MonthTextBlock.Text = targetedDate.ToString("MMMM");
            YearTextBlock.Text = targetedDate.Year.ToString();
        }

        public void CreateCalendar()
        {
            ChangeTitle();
            int weekNumber;
            int weekDay;
            DateTime auxiliarDate = targetedDate;
            int daysinMonth = System.DateTime.DaysInMonth(targetedDate.Year, targetedDate.Month);
            
            

            if (modeView == ViewModes.Months)
            {
                for (int day = 1; day <= daysinMonth; day++)
                {
                    weekNumber = GetWeekNumberOfMonth(auxiliarDate);
                    weekDay = (int)auxiliarDate.DayOfWeek;
                    CreateNumber(weekNumber, weekDay, day.ToString());
                    auxiliarDate = targetedDate.AddDays(day);
                }
            }
            else if (modeView == ViewModes.Weeks)
            {
                //TODO: Put numbers on WeekDays

                for (int day = 1; day <= oneWeekOnDays; day++)
                {
                    CreateNumber(1, day, day.ToString());
                }
            }
            
        }

        public int GetWeekNumberOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, firstDayOfMonth);
            int weekNumber;
            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);
            float daysOnWeek = 7f;
            weekNumber = (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / daysOnWeek);

            return weekNumber;
        }

        public void CreateNumber(int targetedRow, int targetedCol, string contentText)
        {
            Label numberLabel = CreateNumberLabel(contentText);
            AddElementToCalendar(numberLabel, targetedRow, targetedCol);
        }

        public Label CreateNumberLabel(string contentText)
        {
            Label label = new Label();
            if (modeView == ViewModes.Months)
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
            else if (modeView == ViewModes.Weeks)
            {
                //TODO: Check This
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

        public void AddElementToCalendar(Label numberLabel, int targetedRow, int targetedCol)
        {
            Grid.SetColumn(numberLabel, targetedCol);
            Grid.SetRow(numberLabel, targetedRow);
            if (modeView == ViewModes.Months)
            {
                DaysOfMonthCalendarGrid.Children.Add(numberLabel);
            }
            else if (modeView == ViewModes.Weeks)
            {
                //TODO: Check This
                DaysOfWeekWeekCalendarGrid.Children.Add(numberLabel);
            }
            
        }

        public void AddColorToCalendar(int calendarLarge, int targetedCol)
        {
            Border colorWeekend = new Border
            {
                Background = Brushes.AliceBlue,
            };
            Grid.SetColumn(colorWeekend, targetedCol);
            Grid.SetRowSpan(colorWeekend, calendarLarge);
            ;
            if (modeView == ViewModes.Months)
            {
                DaysOfMonthCalendarGrid.Children.Add(colorWeekend);
            }
        }

        public void CleanCalendar()
        {
            if (modeView == ViewModes.Months)
            {
                DaysOfMonthCalendarGrid.Children.Clear();
                AddColorToCalendar(calendarLargeMonths, saturdayColumn);
                AddColorToCalendar(calendarLargeMonths, sundayColumn);
            }
            else if (modeView == ViewModes.Weeks)
            {
                //WeekCalendarGrid.Children.Clear();
            }
        }

        //Serialization Of Binary File
        public void SerializeAppointments(List<Appointment> appointments, string filepath)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, appointments);
            stream.Close();          
        }
        public List<Appointment> DeserializeAppointments(string filepath)
        {
            List<Appointment> appointments = new List<Appointment>();
            IFormatter formatter = new BinaryFormatter();
            if (File.Exists(filepath))
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
        //Useful Methods
        public void ResetCalendar()
        {
            ChangeTitle();
            CleanCalendar();
            CreateCalendar();
        }
        public void ChangeToDayOne()
        {
            targetedDate = new DateTime(targetedDate.Year, targetedDate.Month, firstDayOfMonth);
        }

        //Display Managment
        public void DisplayAppointmentFormView()
        {
            AppointmentContainerGrid.Visibility = Visibility.Visible;

            MonthCalendarGrid.Visibility = Visibility.Hidden;
            WeekCalendarGrid.Visibility = Visibility.Hidden;
        }
        public void DisplayMonthView()
        {
            MonthCalendarGrid.Visibility = Visibility.Visible;

            AppointmentContainerGrid.Visibility = Visibility.Hidden;
            WeekCalendarGrid.Visibility = Visibility.Hidden;
        }
        public void DisplayWeekView()
        {
            WeekCalendarGrid.Visibility = Visibility.Visible;

            AppointmentContainerGrid.Visibility = Visibility.Hidden;
            MonthCalendarGrid.Visibility = Visibility.Hidden;
        }
        public void CheckVisibility()
        {
            if (modeView == ViewModes.Months)
            {
                DisplayMonthView();
            }
            else if (modeView == ViewModes.Weeks)
            {
                DisplayWeekView();
            }

        }

        //Appointment Managment
        public void ClearAppointmentForm()
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

            
        }
        public int ProcessHourForForm(ComboBox hour, ComboBox AMPM)
        {
            int hourReturned = hour.SelectedIndex + oneHour;
            if (AMPM.SelectedIndex == pmSelected) hourReturned += addIfPMHour;

            return hourReturned;
        }
        public int ProcessMinuteForForm(ComboBox minute)
        {
            int auxMinute = minute.SelectedIndex;
            int minuteReturned = auxMinute;

            if (auxMinute == indexFor0) minuteReturned = 0;
            else if (auxMinute == indexFor15) minuteReturned = 15;
            else if (auxMinute == indexFor30) minuteReturned = 30;
            else if (auxMinute == indexFor45) minuteReturned = 45;
            return minuteReturned;
        }
        public void StoreAppointmentForm()
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
                    throw new Exception("End time is Before Start time");
                }
                Appointment appointment = new Appointment(title, date, startTime, endTime, description);
                appointments.Add(appointment);
                TextBlockFeedback.Text = "Saved!";
                TextBlockFeedback.Foreground = Brushes.Green;
                TextBlockFeedback.FontSize = fontSizeForFeedback;
            }
            catch
            {
                TextBlockFeedback.Text = "Error!";
                TextBlockFeedback.Foreground = Brushes.Red;
                TextBlockFeedback.FontSize = fontSizeForFeedback;
            }
        }

        //Application Element Actions
        private void Btn_ChangePositive(object sender, RoutedEventArgs e)
        {
            CheckVisibility();

            if (modeView== ViewModes.Months)
            {
                targetedDate = targetedDate.AddMonths(1);
            }
            else if (modeView == ViewModes.Weeks)
            {
                targetedDate = targetedDate.AddDays(7);
            }
            ResetCalendar();
        }
        private void Btn_ChangeNegative(object sender, RoutedEventArgs e)
        {
            CheckVisibility();
            
            if (modeView == ViewModes.Months)
            {
                targetedDate = targetedDate.AddMonths(-1);
            }
            else if (modeView == ViewModes.Weeks)
            {
                targetedDate = targetedDate.AddDays(-oneWeekOnDays);
            }

            ResetCalendar();

        }
        private void CB_ChangeViewMode(object sender, SelectionChangedEventArgs e)
        {
            int monthIndex = 0;
            int weekIndex = 1;

            if (CB_Mode.SelectedIndex.Equals(monthIndex)) {
                modeView = ViewModes.Months;
            }
            else if (CB_Mode.SelectedIndex.Equals(weekIndex))
            {
                modeView = ViewModes.Weeks;
            }
        }
        private void CB_CloseDropdownView(object sender, EventArgs e)
        {
            CheckVisibility();
            ChangeToDayOne();
            ResetCalendar();
        }
        private void Btn_CreateAppointment(object sender, RoutedEventArgs e)
        {
            DisplayAppointmentFormView();
            TextBlockFeedback.Text = String.Empty;
        }
        private void Btn_ClickCancelForm(object sender, RoutedEventArgs e)
        {
            ClearAppointmentForm();
            CheckVisibility();
        }
        private void Btn_ClickSaveForm(object sender, RoutedEventArgs e)
        {
            //TODO:Store the data
            
            StoreAppointmentForm();
            SerializeAppointments(appointments, binaryFilePath);
            ClearAppointmentForm();

        }
    }
}
