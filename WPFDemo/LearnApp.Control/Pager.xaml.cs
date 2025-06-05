using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace LearnApp.Control
{
    /// <summary>
    /// Pager.xaml 的交互逻辑
    /// </summary>
    public partial class Pager : UserControl
    {
        public static RoutedEvent FirstPageEvent;
        public static RoutedEvent PreviousPageEvent;
        public static RoutedEvent NextPageEvent;
        public static RoutedEvent LastPageEvent;
        public static RoutedEvent GoToPageEvent;

        public static readonly DependencyProperty CurrentPageProperty;
        public static readonly DependencyProperty TotalPageProperty;
        public static readonly DependencyProperty TotalCountProperty;
        public static readonly DependencyProperty ToPageProperty;

        public string CurrentPage
        {
            get { return (string)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        public string TotalPage
        {
            get { return (string)GetValue(TotalPageProperty); }
            set { SetValue(TotalPageProperty, value); }
        }
        public string TotalCount
        {
            get { return (string)GetValue(TotalCountProperty); }
            set { SetValue(TotalCountProperty, value); }
        }

        public int ToPage
        {
            get { return (int)GetValue(ToPageProperty); }
            set { SetValue(ToPageProperty, value); }
        }


        public Pager()
        {
            InitializeComponent();
        }

        static Pager()
        {
            FirstPageEvent = EventManager.RegisterRoutedEvent("FirstPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));
            PreviousPageEvent = EventManager.RegisterRoutedEvent("PreviousPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));
            NextPageEvent = EventManager.RegisterRoutedEvent("NextPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));
            LastPageEvent = EventManager.RegisterRoutedEvent("LastPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));
            GoToPageEvent = EventManager.RegisterRoutedEvent("GoToPage", RoutingStrategy.Direct, typeof(RoutedEventHandler), typeof(Pager));

            CurrentPageProperty = DependencyProperty.Register("CurrentPage", typeof(string), typeof(Pager), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnCurrentPageChanged)));
            TotalPageProperty = DependencyProperty.Register("TotalPage", typeof(string), typeof(Pager), new PropertyMetadata(string.Empty, new PropertyChangedCallback(OnTotalPageChanged)));
            TotalCountProperty = DependencyProperty.Register("TotalCount", typeof(string), typeof(Pager), new PropertyMetadata(string.Empty, new PropertyChangedCallback(TotalCountChanged)));
            ToPageProperty = DependencyProperty.Register("ToPage", typeof(int), typeof(Pager), new PropertyMetadata(1, new PropertyChangedCallback(ToPageChanged)));
        }

        public event RoutedEventHandler FirstPage
        {
            add { AddHandler(FirstPageEvent, value); }
            remove { RemoveHandler(FirstPageEvent, value); }
        }

        public event RoutedEventHandler PreviousPage
        {
            add { AddHandler(PreviousPageEvent, value); }
            remove { RemoveHandler(PreviousPageEvent, value); }
        }

        public event RoutedEventHandler NextPage
        {
            add { AddHandler(NextPageEvent, value); }
            remove { RemoveHandler(NextPageEvent, value); }
        }

        public event RoutedEventHandler LastPage
        {
            add { AddHandler(LastPageEvent, value); }
            remove { RemoveHandler(LastPageEvent, value); }
        }

        public event RoutedEventHandler GoToPage
        {
            add { AddHandler(GoToPageEvent, value); }
            remove { RemoveHandler(GoToPageEvent, value); }
        }
        public static void OnTotalPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pager p = d as Pager;

            if (p != null)
            {
                Run rTotal = (Run)p.FindName("rTotal");

                rTotal.Text = (string)e.NewValue;
            }
        }

        public static void TotalCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pager p = d as Pager;

            if (p != null)
            {
                Run rTotalCount = (Run)p.FindName("rTotalCount");

                rTotalCount.Text = (string)e.NewValue;
            }
        }

        private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pager p = d as Pager;

            if (p != null)
            {
                Run rCurrrent = (Run)p.FindName("rCurrent");

                rCurrrent.Text = (string)e.NewValue;
            }
        }

        private static void ToPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Pager p = d as Pager;

            if (p != null)
            {
                TextBox rCurrrent = (TextBox)p.FindName("toPage");

                rCurrrent.Text = e.NewValue.ToString();
            }
        }

        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(FirstPageEvent, this));
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(PreviousPageEvent, this));
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(NextPageEvent, this));
        }

        private void LastPageButton_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(LastPageEvent, this));
        }

        private void TextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ToPage = int.Parse((sender as TextBox).Text);
                RaiseEvent(new RoutedEventArgs(GoToPageEvent, this));
            }
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            if (e.Handled = re.IsMatch(e.Text))
                e.Handled = true;
            else
                e.Handled = false;
        }
    }
}
