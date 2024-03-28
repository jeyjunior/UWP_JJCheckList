using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UWP_JJCheckList.Views.Task
{
    public sealed partial class TaskContent : UserControl
    {

        public TaskContent()
        {
            this.InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void txtTarefa_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txbTarefa.Text = this.txtTarefa.Text;

            this.txbTarefa.Visibility = Visibility.Visible;
            this.txtTarefa.Visibility = Visibility.Collapsed;
        }

        private void txbTarefa_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            this.txbTarefa.Visibility = Visibility.Collapsed;
            this.txtTarefa.Visibility = Visibility.Visible;

            this.txtTarefa.Focus(FocusState.Programmatic);
            this.txtTarefa.SelectionStart = this.txtTarefa.Text.Length;

        }
    }
}
