using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace WpfAppDailyText
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : INotifyPropertyChanged
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public MainWindow()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {

   
            DataContext = this;
           
            checkDays();
            gettingData();

            InitializeComponent();
        }



        private void gettingData()
        {
            Url = "https://wol.jw.org/ro/wol/h/r34/lp-m/" + CurrentYear.ToString() + "/" + CurrentMonth.ToString() + "/" + CurrentDay.ToString();
            var httpClient = new HttpClient();

            var html = httpClient.GetStringAsync(Url).Result;
            if (html != null)
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var ceva3 = htmlDocument.DocumentNode.CssSelect("header>h2").ToList();
                Titlu = ceva3[1].InnerText;

                var ceva = htmlDocument.DocumentNode.CssSelect(".themeScrp").ToList();
                Verset = ceva[1].InnerText;

                var ceva2 = htmlDocument.DocumentNode.CssSelect(".bodyTxt > div >div").ToList();
                Comentariu = ceva2[1].InnerText;
            }
            else
            {
                Titlu = "page not found";
                Comentariu = " ";
                Verset = " ";
            }


        }

        public TextBlock CreateTextBlock(string inlines)
        {
            var xaml = "<TextBlock xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">"
                + inlines + "</TextBlock>";
            return XamlReader.Parse(xaml) as TextBlock;
        }


        private string url;
        private string titlu;
        private string verset;
        private string comentariu;
        private int currentYear = DateTime.Now.Year;
        private int currentMonth = DateTime.Now.Month;
        private int currentDay = DateTime.Now.Day;

        public bool thirty { get; set; } = false;
        public bool thirtyOne { get; set; } = false;
        public bool twentyEight { get; set; } = false;
        public bool twentyNine { get; set; } = false;

        private void checkDays()
        {


            switch (currentMonth)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    thirtyOne = true;
                    thirty = false;
                    twentyEight = false;
                    twentyNine = false;
                    break;
                case 2:
                    if ((currentYear % 4 == 0 && currentYear % 100 != 0) || (currentYear % 100 == 0 && currentYear % 400 == 0))
                    {
                        twentyNine = true;
                        thirtyOne = false;
                        thirty = false;
                        twentyEight = false;
                        break;
                    }
                    else
                    {
                        twentyEight = true;
                        twentyNine = false;
                        thirtyOne = false;
                        thirty = false;
                        break;
                    }
                case 4:
                case 6:
                case 9:
                case 11:
                    thirty = true;
                    twentyEight = false;
                    twentyNine = false;
                    thirtyOne = false;
                    break;

            }
        }

        public string Url
        {
            get { return url; }
            set
            {
                if (url != value)
                {
                    url = value;
                    OnPropertyChanged("Url");
                }
            }
        }
        public string Titlu
        {
            get { return titlu; }
            set
            {
                if (titlu != value)
                {
                    titlu = value;
                    OnPropertyChanged("Titlu");
                }
            }
        }
        public string Verset
        {
            get { return verset; }
            set
            {
                if (verset != value)
                {
                    verset = value;
                    OnPropertyChanged("Verset");
                }
            }
        }
        public string Comentariu
        {
            get { return comentariu; }
            set
            {
                if (comentariu != value)
                {
                    comentariu = value;
                    OnPropertyChanged("Comentariu");
                }
            }
        }
        public int CurrentYear
        {
            get { return currentYear; }
            set
            {
                if (currentYear != value)
                {
                    currentYear = value;
                    OnPropertyChanged("CurrentYear");
                    OnPropertyChanged("CurrentMonth");
                    OnPropertyChanged("CurrentDay");
                    OnPropertyChanged("Url");
                    OnPropertyChanged("Titlu");
                    OnPropertyChanged("Verset");
                    OnPropertyChanged("Comentariu");
                }
            }
        }
        public int CurrentMonth
        {
            get { return currentMonth; }
            set
            {
                if (currentMonth != value)
                {
                    currentMonth = value;
                    OnPropertyChanged("CurrentMonth");
                    OnPropertyChanged("CurrentDay");
                    OnPropertyChanged("Url");
                    OnPropertyChanged("Titlu");
                    OnPropertyChanged("Verset");
                    OnPropertyChanged("Comentariu");
                }
            }
        }
        public int CurrentDay
        {
            get { return currentDay; }
            set
            {
                if (currentDay != value)
                {
                    currentDay = value;
                    OnPropertyChanged("CurrentDay");
                    OnPropertyChanged("Url");
                    OnPropertyChanged("Titlu");
                    OnPropertyChanged("Verset");
                    OnPropertyChanged("Comentariu");

                }
            }
        }




        private void NextDay_Click(object sender, RoutedEventArgs e)
        {
            if (thirty)
            {
                if (currentDay == 30)
                {
                    currentDay = 1;
                    currentMonth++;
                    checkDays();
                    gettingData();
                    return;
                }
            }
            else if (thirtyOne)
            {
                if (currentDay == 31)
                {
                    if (currentMonth != 12)
                    {
                        currentDay = 1;
                        currentMonth++;
                        checkDays();
                        gettingData();
                        return;
                    }
                    else
                    {
                        currentDay = 1;
                        currentMonth = 1;
                        currentYear++;
                        checkDays();
                        gettingData();
                        return;
                    }
                }
            }
            else if (twentyEight)
            {
                if (currentDay == 28)
                {
                    currentDay = 1;
                    currentMonth++;
                    checkDays();
                    gettingData();
                    return;
                }
            }
            else
            {
                if (currentDay == 29)
                {
                    currentDay = 1;
                    currentMonth++;
                    checkDays();
                    gettingData();
                    return;
                }
            }

            CurrentDay++;

            gettingData();
        }

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        private void PreviousDay_Click(object sender, RoutedEventArgs e)
        {
            if (currentDay == 1)
            {
                if (thirty)
                {

                    currentDay = 31;
                    currentMonth--;
                    checkDays();
                    gettingData();
                    return;

                }
                else if (thirtyOne)
                {

                    if (currentMonth == 8)
                    {
                        currentDay = 31;
                        currentMonth = 7;
                        checkDays();
                        gettingData();
                        return;
                    }
                    else if (currentMonth == 1)
                    {
                        currentDay = 31;
                        currentMonth = 12;
                        currentYear--;
                        checkDays();
                        gettingData();
                        return;
                    }
                    else if (currentMonth == 3)
                    {
                        if ((currentYear % 4 == 0 && currentYear % 100 != 0) || (currentYear % 100 == 0 && currentYear % 400 == 0))
                        {
                            currentDay = 29;
                            currentMonth--;
                            checkDays();
                            gettingData();
                            return;
                        }
                        else
                        {
                            currentDay = 28;
                            currentMonth--;
                            checkDays();
                            gettingData();
                            return;
                        }
                    }
                    else
                    {
                        currentDay = 30;
                        currentMonth--;
                        checkDays();
                        gettingData();
                        return;
                    }

                }
                else
                {
                    currentDay = 31;
                    currentMonth--;
                    checkDays();
                    gettingData();
                    return;
                }
            }
            CurrentDay--;
            gettingData();
        }
    }


}
