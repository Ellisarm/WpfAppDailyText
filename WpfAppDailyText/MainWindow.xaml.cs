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
using System.Diagnostics;
using System.Windows.Navigation;

namespace WpfAppDailyText
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : INotifyPropertyChanged
    {
        public MainWindow()

        {
            InitializeComponent();
            checkDays();
            gettingData();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            e.Handled = true;
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

                /* var ceva3 = htmlDocument.DocumentNode.CssSelect("header>h2").ToList();
                 Titlu = ceva3[1].InnerText;*/
                var ceva3 = htmlDocument.DocumentNode.CssSelect("header>h2>").ToList();
                string Titlu = ceva3[1].InnerText;
                Titlu = "<TextBlock  xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Grid.Row=\"0\"  FontSize=\"16\" FontWeight=\"Bold\">" + Titlu + "</TextBlock>";
                grid.Children.Add(CreateTextBlock(Titlu));



                /*var ceva = htmlDocument.DocumentNode.CssSelect(".themeScrp").ToList();
                Verset = ceva[1].InnerText;*/

                var ceva = htmlDocument.DocumentNode.CssSelect(".themeScrp").ToList();
                string Verset = ceva[1].InnerHtml;
                var vReplacement = Verset.Replace("em>", "Italic>");
                var vReplacement2 = vReplacement.Replace("<a href", "<Hyperlink NavigateUri");
                var vReplacement3 = vReplacement2.Replace("/ro", "https://wol.jw.org/ro");
                var vReplacement4 = vReplacement3.Replace("class=\"b\"", " ");
                var vReplacement5 = vReplacement4.Replace("</a>", "</Hyperlink>");
                var vReplacement6 = vReplacement5.Replace("data-bid", "Tag");
                vReplacement6 = "<TextBlock Grid.Column = \"0\" Grid.Row = \"1\" TextWrapping = \"Wrap\" FontSize = \"14\" Margin = \"0,0,0,10\" xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">" + vReplacement6 + "</TextBlock>";
                grid.Children.Add(CreateTextBlock(vReplacement6));



                /*var ceva2 = htmlDocument.DocumentNode.CssSelect(".bodyTxt > div >div").ToList();
                Comentariu = ceva2[1].InnerText;*/
                var ceva2 = htmlDocument.DocumentNode.CssSelect(".bodyTxt > div >div").ToList();
                string Comentariu = ceva2[1].InnerHtml;
                var replacement = Comentariu.Replace("em>", "Italic>");
                var replacement2 = replacement.Replace("<a href", "<Hyperlink NavigateUri");
                var replacement3 = replacement2.Replace("/ro", "https://wol.jw.org/ro");
                var replacement4 = replacement3.Replace("class=\"b\"", " ");
                var replacement5 = replacement4.Replace("</p>", "</TextBlock>");
                var replacement6 = replacement5.Replace("data-pid", "Tag");
                var replacement7 = replacement6.Replace("id=\"p", "Uid=\"");
                var replacement8 = replacement7.Replace("</a>", "</Hyperlink>");
                var replacement9 = replacement8.Replace("<p", "<TextBlock xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" Grid.Column=\"0\" Grid.Row=\"2\"   MaxWidth=\"400\" TextWrapping=\"Wrap\"  VerticalAlignment=\"Stretch\" HorizontalAlignment=\"Center\" FontSize=\"14\" ");
                var replacement10 = replacement9.Replace("class=\"sb\"", " ");
                var replacement11 = replacement10.Replace("data-bid", "Tag");
                grid.Children.Add(CreateTextBlock(replacement11));

                grid.AddHandler(Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(Hyperlink_RequestNavigate));
            }
           


        }

        public TextBlock CreateTextBlock(string inlines)
        {
            return XamlReader.Parse(inlines) as TextBlock;
        }


        private string url;
        
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
            grid.Children.RemoveRange(2, 3);
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
            grid.Children.RemoveRange(2, 3);
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
