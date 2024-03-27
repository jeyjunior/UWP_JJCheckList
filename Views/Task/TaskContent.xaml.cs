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
        private bool exibirBlocoNotas = false;

        public TaskContent()
        {
            this.InitializeComponent();
        }

        private void btnBlocoNotas_Click(object sender, RoutedEventArgs e)
        {
            exibirBlocoNotas = !exibirBlocoNotas;
            ExibirBlocoDeNotas();
        }

        private void ExibirBlocoDeNotas()
        {
            this.gridPrincipal.RowDefinitions[1].Height = exibirBlocoNotas ? new GridLength(200) : new GridLength(0);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ExibirBlocoDeNotas();
        }
    }
}
