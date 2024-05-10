using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using UWP_JJCheckList.Controls.Helpers;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UWP_JJCheckList.Views.Task
{
    public sealed partial class TaskContent : UserControl, ITaskContentManipularComponentes
    {
        #region Interfaces
        private readonly ICLTaskContentRepositorio cLTaskContentRepositorio;
        #endregion
        #region Propriedades
        private IMainPageManipularComponentes mainPageManipularComponentes;
        private CLTaskContent clTaskContent { get; set; }
        #endregion

        #region Método Construtor
        public TaskContent(CLTaskContent clTaskContent, IMainPageManipularComponentes mainPageManipularComponentes)
        {
            this.InitializeComponent();

            this.clTaskContent = clTaskContent;
            this.mainPageManipularComponentes = mainPageManipularComponentes;
            cLTaskContentRepositorio = App.Container.GetInstance<ICLTaskContentRepositorio>();

            this.txtTarefa.Text = clTaskContent.Tarefa;
            this.txbTarefa.Text = clTaskContent.Tarefa;
            this.tgbTarefa.IsChecked = clTaskContent.Checked;
        }
        #endregion

        #region Eventos
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AtualizarCorTextos();
        }
        private void txtTarefa_LostFocus(object sender, RoutedEventArgs e)
        {
            DesabilitarEdicao();
            AtualizarInformacoesBase();
        }
        private void txbTarefa_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            HabilitarEdicao();
        }
        private void tgbTarefa_Checked(object sender, RoutedEventArgs e)
        {
            AtualizarInformacoesBase();
            AtualizarCorTextos();
        }
        private void tgbTarefa_Unchecked(object sender, RoutedEventArgs e)
        {
            AtualizarInformacoesBase();
            AtualizarCorTextos();
        }
        private void btnDeletar_Click(object sender, RoutedEventArgs e)
        {
            Deletar();
        }
        private void UserControl_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            HabilitarEdicao();
        }
        private void txtTarefa_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                DesabilitarEdicao();
                AtualizarInformacoesBase();
            }
        }
        #endregion

        #region Métodos
        private void AtualizarInformacoesBase()
        {
            try
            {
                clTaskContent.Tarefa = this.txtTarefa.Text;
                clTaskContent.Checked = (bool)this.tgbTarefa.IsChecked;
                cLTaskContentRepositorio.AtualizarAsync(clTaskContent);
            }
            catch (Exception ex)
            {
                Aviso.ContentDialog("Erro", ex.Message);
            }
        }
        private void ExibirMensagemErro(string titulo, string conteudo)
        {
            var msg = new ContentDialog { Title = titulo, Content = conteudo, CloseButtonText = "OK" };
            msg.ShowAsync();
        }
        private void AtualizarCorTextos()
        {
            if (this.clTaskContent.Checked)
            {
                Color cor = (Color)XamlBindingHelper.ConvertValue(typeof(Color), "#666666");
                this.txbTarefa.Foreground = new SolidColorBrush(cor);
            }
            else
            {
                Color cor = (Color)XamlBindingHelper.ConvertValue(typeof(Color), "#F2F2F2");
                this.txbTarefa.Foreground = new SolidColorBrush(cor);
            }
        }
        private void Deletar()
        {
            cLTaskContentRepositorio.DeletarAsync(clTaskContent);

            if (!clTaskContent.IsValid)
            {
                ExibirMensagemErro("Erro", clTaskContent.ValidationResult.ErrorMessage);
                return;
            }

            mainPageManipularComponentes.DeletarItem(this);
        }
        private void DesabilitarEdicao()
        {
            this.txbTarefa.Text = this.txtTarefa.Text;

            this.txbTarefa.Visibility = Visibility.Visible;
            this.txtTarefa.Visibility = Visibility.Collapsed;
            this.Focus(FocusState.Programmatic);
        }
        private void HabilitarEdicao()
        {
            this.txbTarefa.Visibility = Visibility.Collapsed;
            this.txtTarefa.Visibility = Visibility.Visible;

            this.txtTarefa.Focus(FocusState.Programmatic);
            this.txtTarefa.SelectionStart = this.txtTarefa.Text.Length;
        }
        #endregion

        #region Métodos Público
        public void MarcarCheckBox()
        {
            this.tgbTarefa.IsChecked = true;
        }
        public void DesmarcarCheckBox()
        {
            this.tgbTarefa.IsChecked = false;
        }
        public void DeletarItem()
        {
            Deletar();
        }
        public bool ObterCheckBoxStatus()
        {
            return (bool)this.tgbTarefa.IsChecked;
        }
        public void DefinirIndice(int indice)
        {
            this.clTaskContent.IndiceLista = indice;
        }
        #endregion
    }
}
