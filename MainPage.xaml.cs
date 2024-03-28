using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_JJCheckList.Assets;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;
using UWP_JJCheckList.Models.Repositorios;
using UWP_JJCheckList.Views.Task;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UWP_JJCheckList
{
    public sealed partial class MainPage : Page
    {
        #region Interfaces
        private readonly ICLParametroRepositorio cLParametroRepositorio;
        #endregion
        #region Propriedades
        private CLParametro pTituloPrincipal;
        private CLParametro pTituloPrincipalFontSize;
        #endregion

        #region Views
        private TaskSetup taskSetup;
        #endregion

        #region Construtor
        public MainPage()
        {
            this.InitializeComponent();

            cLParametroRepositorio = App.Container.GetInstance<ICLParametroRepositorio>();  
        }
        #endregion

        #region Eventos
        // Main
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarParametros();

            taskSetup = new TaskSetup();
            //taskSetup.children = this.gridViewConteudo.Items;
        }

        // Título
        private void txbTitulo_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            this.txtTitulo.Visibility = Visibility.Visible;
            this.txbTitulo.Visibility = Visibility.Collapsed;

            this.txtTitulo.Focus(FocusState.Programmatic);
            this.txtTitulo.SelectionStart = this.txtTitulo.Text.Length;
        }
        private void txtTitulo_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.txbTitulo.Text = this.txtTitulo.Text;
            pTituloPrincipal.Valor = this.txtTitulo.Text;
        }
        private void txtTitulo_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txtTitulo.Visibility = Visibility.Collapsed;
            this.txbTitulo.Visibility = Visibility.Visible;

            SalvarTitulo();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var taskContent = new TaskContent();
            taskContent.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            this.listViewConteudo.Items.Add(new TaskContent());
            //taskSetup.ShowAsync();
        }
        private void btnDeletarAll_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnLimparAll_Click(object sender, RoutedEventArgs e)
        {

        }
        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void chkAll_Unchecked(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Métodos
        private void CarregarParametros()
        {
            try
            {
                pTituloPrincipal = new CLParametro();


                pTituloPrincipal = cLParametroRepositorio.Obter(Parametros.TituloPrincipal);
                pTituloPrincipalFontSize = cLParametroRepositorio.Obter(Parametros.TituloPrincipalFontSize);

                if (!pTituloPrincipal.IsValid)
                {
                    ExibirMensagemErro("Erro", pTituloPrincipal.ValidationResult.ErrorMessage);
                    return;
                }

                this.txtTitulo.Text = pTituloPrincipal.Valor.ToString();
                this.txbTitulo.Text = this.txtTitulo.Text;

                if (!pTituloPrincipalFontSize.IsValid)
                {
                    ExibirMensagemErro("Erro", pTituloPrincipal.ValidationResult.ErrorMessage);
                    return;
                }

                this.txtTitulo.FontSize = Convert.ToInt32(pTituloPrincipalFontSize.Valor.Trim());
                this.txbTitulo.FontSize = this.txtTitulo.FontSize;
            }
            catch (Exception ex)
            {
                ExibirMensagemErro("Erro", ex.Message);
            }
        }
        private void SalvarTitulo()
        {
            try
            {
                pTituloPrincipal.ValidationResult = null;
                pTituloPrincipal.Valor = this.txtTitulo.Text.Trim();
                cLParametroRepositorio.Atualizar(pTituloPrincipal);

                if(!pTituloPrincipal.IsValid)
                {
                    var msg = new ContentDialog { Title = "Erro", Content = pTituloPrincipal.ValidationResult.ErrorMessage, CloseButtonText = "OK" };
                    msg.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                var msg = new ContentDialog { Title = "Erro", Content = ex.Message, CloseButtonText = "OK" };
                msg.ShowAsync();
            }
        }
        
        private void ExibirMensagemErro(string titulo, string conteudo)
        {
            var msg = new ContentDialog { Title = titulo, Content = conteudo, CloseButtonText = "OK" };
            msg.ShowAsync();
        }
        #endregion
    }
}
