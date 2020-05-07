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
        const int weeksInMonth = 6;
        const int saturdayColumn = 5;
        const int sundayColumn = 6;
        const int numberLabelWidth = 30;
        const int numberLabelHeight = 240;
        const int oneWeekOnDays = 7;
        const int sizeForWeekLabel = 24;
        const int calendarLargeMonths = 7;

        const int addIfPMHour = 12;
        const int oneHour = 1;
        const int pmSelected = 1;
        const int noSeconds = 0;
        const int indexFor0 = 0;
        const int indexFor15 = 1;
        const int indexFor30 = 2;
        const int indexFor45 = 3;
        const int startBeforeEndTime = -1;
        const int fontSizeForFeedback = 24;
        List<int> weekNumbers = new List<int>();

        static readonly string binaryFilePath = Environment.CurrentDirectory + "\\appointments.txt";
        List<Appointment> appointments = new List<Appointment>();

        static readonly int firstDayOfMonth = 1;
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



        //Views Creation
        public void ChangeTitle()
        {
            MonthTextBlock.Text = targetedDate.ToString("MMMM");
            YearTextBlock.Text = targetedDate.Year.ToString();
        }
        public void CreateCalendar()
        {
            ChangeTitle();

            DateTime auxiliarDate = targetedDate;
            if (modeView == ViewModes.Months)
            {
                for (int weekNumber = 0; weekNumber < weeksInMonth; weekNumber++)
                {
                    FillListWithNumbers(weekNumbers, auxiliarDate);
                    FillWeekWithNumber(weekNumber, weekNumbers);
                    auxiliarDate = auxiliarDate.AddDays(oneWeekOnDays);
                    weekNumbers.Clear();
                }
            }
            else if (modeView == ViewModes.Weeks)
            {
                FillListWithNumbers(weekNumbers, auxiliarDate);
                FillWeekWithNumber(1, weekNumbers);
                weekNumbers.Clear();
            }
        }
        public void FillListWithNumbers(List<int> listNumber, DateTime targetedDate)
        {
            DateTime auxDate = targetedDate;
            int daysDiffFromMonday = 0;
            int numberToFill;

            while (auxDate.DayOfWeek.ToString() != "Monday")
            {
                daysDiffFromMonday += 1;
                auxDate = auxDate.AddDays(-1);
            }
            numberToFill = auxDate.Day;
            for (int count = 1; count <= oneWeekOnDays; count++)
            {
                listNumber.Add(numberToFill);
                auxDate = auxDate.AddDays(1);
                numberToFill = auxDate.Day;
            }
        }
        public void FillWeekWithNumber(int rowNumber, List<int> listNumber)
        {
            int counter = 0;
            foreach (int number in listNumber)
            {
                Label numberLabel = CreateNumberLabel(number.ToString());
                AddElementToCalendar(numberLabel, rowNumber, counter);
                counter++;
            }
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
            if (modeView == ViewModes.Months)
            {
                Grid.SetColumn(numberLabel, targetedCol);
                Grid.SetRow(numberLabel, targetedRow);
                DaysOfMonthCalendarGrid.Children.Add(numberLabel);
            }
            else if (modeView == ViewModes.Weeks)
            {
                Grid.SetColumn(numberLabel, targetedCol + 1);
                Grid.SetRow(numberLabel, 1);
                NumberOfWeekWeekCalendarGrid.Children.Add(numberLabel);
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
                NumberOfWeekWeekCalendarGrid.Children.Clear();
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
            StoreAppointmentForm();
            SerializeAppointments(appointments, binaryFilePath);
            ClearAppointmentForm();
        }
    }
}
