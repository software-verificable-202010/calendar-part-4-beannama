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

        DateTime targeted_date = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1);


        public MainWindow()
        {
            InitializeComponent();
            CalendarCreation();
        }

        public void TitleChanger()
        {
            Month_TextBlock.Text = targeted_date.ToString("MMMM");
            Year_TextBlock.Text = targeted_date.Year.ToString();
        }

        public void CalendarCreation()
        {
            TitleChanger();

            int weekNumber;
            int weekday;
            DateTime auxiliar_date = targeted_date;
            int daysinMonth = System.DateTime.DaysInMonth(targeted_date.Year, targeted_date.Month);


            for (int day = 1; day <= daysinMonth; day++)
            {
                weekNumber = GetWeekNumberOfMonth(auxiliar_date);
                weekday = (int)auxiliar_date.DayOfWeek;

                NumberCreation(weekNumber, weekday, day.ToString());
                auxiliar_date = targeted_date.AddDays(day);
            }

        }

        public int GetWeekNumberOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }

        public void NumberCreation(int targeted_row, int targeted_col, string content_text)
        {
            Label number_label = LabelNumberCreation(content_text);
            AddLabelToCalendar(number_label, targeted_row, targeted_col);
        }

        public Label LabelNumberCreation(string content_text)
        {
            Label label = new Label
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = 240,
                Height = 30,
                Content = content_text
            };

            return label;
        }

        public void AddLabelToCalendar(Label number_label, int targeted_row, int targeted_col)
        {
            Grid.SetColumn(number_label, targeted_col);
            Grid.SetRow(number_label, targeted_row);
            Calendar_Grid.Children.Add(number_label);
        }



        private void Change_Month_Positive(object sender, RoutedEventArgs e)
        {
            targeted_date = targeted_date.AddMonths(1);
            TitleChanger();
            CalendarCreation();

        }
        private void Change_Month_Negative(object sender, RoutedEventArgs e)
        {
            targeted_date = targeted_date.AddMonths(-1);
            TitleChanger();
            CalendarCreation();
        }
    }
}
