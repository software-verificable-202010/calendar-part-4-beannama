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


namespace Proyecto_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Consts
        static readonly int firstDayOfMonth = 1;
        int saturdayColumn = 5;
        int sundayColumn = 6;
        int numberLabelWidth = 30;
        int numberLabelHeight = 240;
        int oneWeekOnDays = 7;
        int sizeForWeekLabel = 24;
        public int calendarLargeMonths = 7;

        DateTime targetedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, firstDayOfMonth);
        public enum ViewModes{ 
            Weeks,
            Months,
        };
        public ViewModes modeView;
        


        public MainWindow()
        {
            InitializeComponent();
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
                    FontSize = 24,
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
                AddColorToCalendar(calendarLargeMonths, sundayColumn);
            }
            else if (modeView == ViewModes.Weeks)
            {
                //WeekCalendarGrid.Children.Clear();
            }
        }

        public void ResetCalendar()
        {
            ChangeTitle();
            CleanCalendar();
            CreateCalendar();
        }

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

        public void ChangeToDayOne()
        {
            targetedDate = new DateTime(targetedDate.Year, targetedDate.Month, firstDayOfMonth);
        }

        

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
        }

        private void Btn_ClickCancelForm(object sender, RoutedEventArgs e)
        {
            //TODO: Clean the Form
            CheckVisibility();
        }

        private void Btn_ClickSaveForm(object sender, RoutedEventArgs e)
        {
            //TODO:Store the data
            //TODO:Clean the Form
            CheckVisibility();
        }
    }
}
