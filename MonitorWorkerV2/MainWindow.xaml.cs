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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonitorWorkerV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //subscribe loaded event
            Loaded += (sender,arg) => { AppController.Instance.Start(); };
            //set data context
            DataContext = MonitorWorkerV2.DataContext.Instance.Data;

          
        }
        //debug 
        //private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var catcher = new WindowCatcher();

        //    HwndSource source = (HwndSource)HwndSource.FromVisual(winCatch);
        //    IntPtr handle = source.Handle;

        //    catcher.CatchWindow(@"c:\windows\system32\notepad.exe", handle);
        //}
    }
}
