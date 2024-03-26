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

// O modelo de item de Página em Branco está documentado em https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x416

namespace UWP_JJCheckList
{
    /// <summary>
    /// Uma página vazia que pode ser usada isoladamente ou navegada dentro de um Quadro.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        #region Construtor
        public MainPage()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Eventos
        // Título
        private void txtTitulo_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txtTitulo.Visibility = Visibility.Collapsed;
            this.txbTitulo.Visibility = Visibility.Visible;
        }
        private void txbTitulo_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            this.txtTitulo.Visibility = Visibility.Visible;
            this.txbTitulo.Visibility = Visibility.Collapsed;
        }
        private void txtTitulo_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.txbTitulo.Text = this.txtTitulo.Text.Trim();
        }
        #endregion
    }
}
