using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Bezier
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BezierClock clock;
        public MainWindow()
        {
            InitializeComponent();

            clock = new BezierClock(this.canvas);
        }
      
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DigitHash dh = new DigitHash();
            var d = Settings.h.Distinct(new HashItemComparer());
            dh.h = new HashSet<HashItem>(d);

            dh.SaveAsXML("hash.xml");
            
            this.Close();
        }
    }
}
