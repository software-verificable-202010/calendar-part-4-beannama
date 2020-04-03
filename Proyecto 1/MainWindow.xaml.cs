using System;
using System.Collections.Generic;
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
        DateTime targeted_date = DateTime.Today;
        int columns = 7;
        int rows = 5;


        public MainWindow()
        {
            InitializeComponent();
            CalendarCreation();
            
        }

        public Label LabelNumberCreation(string content_text)
        {
            Label label = new Label();
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Top;
            label.Width = 240;
            label.Height = 30;
            label.Content = content_text;

            return label;
        }

        public void AddLabelToCalendar(Label number_label,int targeted_row, int targeted_col)
        {
            Grid.SetColumn(number_label, targeted_col);
            Grid.SetRow(number_label, targeted_row);
            Calendar_Grid.Children.Add(number_label);
        }

        public void NumberCreation(int targeted_row, int targeted_col, string content_text)
        {
            Label number_label = new Label();
            number_label = LabelNumberCreation(content_text);
            AddLabelToCalendar(number_label, targeted_row, targeted_col);

        }

  

        public void TitleChanger()
        {
            Month_TextBlock.Text = targeted_date.ToString("MMMM");
            Year_TextBlock.Text = targeted_date.Year.ToString();

        }

        public void CalendarCreation()
        {
            TitleChanger();

            int number_text = 1;
            for (int targeted_row = 1;targeted_row<=rows; targeted_row++)
            {
                for (int targeted_colummn = 0; targeted_colummn <columns; targeted_colummn++)
                {
                    NumberCreation(targeted_row, targeted_colummn, number_text.ToString());
                    number_text++;
                }
            }
        }

        private void Change_Month_Positive(object sender, RoutedEventArgs e)
        {
            targeted_date = targeted_date.AddMonths(1);
            TitleChanger();
        }

        private void Change_Month_Negative(object sender, RoutedEventArgs e)
        {
            targeted_date = targeted_date.AddMonths(-1);
            TitleChanger();
        }
    }
}
