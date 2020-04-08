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
        //We couldn't use a variable
        //DateTime this_date = DateTime.Now;
        //Because a field initializer cannopt reference the non-static field, method, or property 'MainWindow.this_date'

        DateTime targetedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1);

        public MainWindow()
        {
            InitializeComponent();
            CalendarCreation();
        }

        public void TitleChanger()
        {
            MonthTextBlock.Text = targetedDate.ToString("MMMM");
            YearTextBlock.Text = targetedDate.Year.ToString();
        }

        public void CalendarCreation()
        {
            TitleChanger();
            int weekNumber;
            int weekDay;
            DateTime auxiliarDate = targetedDate;
            int daysinMonth = System.DateTime.DaysInMonth(targetedDate.Year, targetedDate.Month);
            for (int day = 1; day <= daysinMonth; day++)
            {
                weekNumber = GetWeekNumberOfMonth(auxiliarDate);
                weekDay = (int)auxiliarDate.DayOfWeek;
                NumberCreation(weekNumber, weekDay, day.ToString());
                auxiliarDate = targetedDate.AddDays(day);
            }

        }

        public int GetWeekNumberOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);
            int weekNumber;
            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);
            float daysOnWeek = 7f;
            weekNumber = (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / daysOnWeek) + 1;

            return weekNumber; //+1 So it starts on Monday and not Sunday
        }

        public void NumberCreation(int targetedRow, int targetedCol, string contentText)
        {
            Label numberLabel = LabelNumberCreation(contentText);
            AddElementToCalendar(numberLabel, targetedRow, targetedCol);
        }

        public Label LabelNumberCreation(string contentText)
        {
            Label label = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 240,
                Height = 30,
                Content = contentText
            };

            return label;
        }

        public void AddElementToCalendar(Label numberLabel, int targetedRow, int targetedCol)
        {
            Grid.SetColumn(numberLabel, targetedCol);
            Grid.SetRow(numberLabel, targetedRow);
            CalendarGrid.Children.Add(numberLabel);
        }

        public void AddColorToCalendar(int calendarLarge, int targetedCol)
        {
            Border colorWeekend = new Border
            {
                Background = Brushes.WhiteSmoke,
            };
            Grid.SetColumn(colorWeekend, targetedCol);
            Grid.SetRowSpan(colorWeekend, calendarLarge);
            CalendarGrid.Children.Add(colorWeekend);
        }



        public void CleanCalendar()
        {
            CalendarGrid.Children.Clear();
            int saturdayColumn = 5;
            int sundayColumn = 6;
            int calendarLarge = 7;
            AddColorToCalendar(calendarLarge, sundayColumn);
            AddColorToCalendar(calendarLarge, saturdayColumn);
        }


        private void ChangeMonthPositive(object sender, RoutedEventArgs e)
        {
            targetedDate = targetedDate.AddMonths(1);
            CleanCalendar();
            TitleChanger();
            CalendarCreation();

        }
        private void ChangeMonthNegative(object sender, RoutedEventArgs e)
        {
            targetedDate = targetedDate.AddMonths(-1);
            CleanCalendar();
            TitleChanger();
            CalendarCreation();
        }
    }
}
