using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;
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
        #region Interfaces
        private readonly ICLTaskContentRepositorio cLTaskContentRepositorio;
        #endregion
        #region Propriedades
        private CLTaskContent clTaskContent;
        #endregion

        #region Propriedades Pública
        public RoutedEventHandler DeletarItem;
        #endregion

        #region Método Construtor
        public TaskContent(CLTaskContent clTaskContent)
        {
            this.clTaskContent = clTaskContent;
            this.InitializeComponent();

            cLTaskContentRepositorio = App.Container.GetInstance<ICLTaskContentRepositorio>();  
        }
        #endregion

        #region Eventos
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.txtTarefa.Text = clTaskContent.Tarefa;
            this.txbTarefa.Text = clTaskContent.Tarefa;
            this.tgbTarefa.IsChecked = clTaskContent.Checked;
        }

        private void txtTarefa_LostFocus(object sender, RoutedEventArgs e)
        {
            this.txbTarefa.Text = this.txtTarefa.Text;

            this.txbTarefa.Visibility = Visibility.Visible;
            this.txtTarefa.Visibility = Visibility.Collapsed;

            AtualizarInformacoesBase();
        }

        private void txbTarefa_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            this.txbTarefa.Visibility = Visibility.Collapsed;
            this.txtTarefa.Visibility = Visibility.Visible;

            this.txtTarefa.Focus(FocusState.Programmatic);
            this.txtTarefa.SelectionStart = this.txtTarefa.Text.Length;

        }
        private void tgbTarefa_Checked(object sender, RoutedEventArgs e)
        {
            AtualizarInformacoesBase();
        }
        private void tgbTarefa_Unchecked(object sender, RoutedEventArgs e)
        {
            AtualizarInformacoesBase();
        }
        private void btnDeletar_Click(object sender, RoutedEventArgs e)
        {
            cLTaskContentRepositorio.Deletar(clTaskContent);

            if(!clTaskContent.IsValid)
            {
                ExibirMensagemErro("Erro", clTaskContent.ValidationResult.ErrorMessage);
                return;
            }

            DeletarItem?.Invoke(this, e);
        }
        #endregion

        #region Métodos
        private void AtualizarInformacoesBase()
        {
            clTaskContent.Tarefa = this.txtTarefa.Text;
            clTaskContent.Checked = (bool)this.tgbTarefa.IsChecked;
            cLTaskContentRepositorio.Atualizar(clTaskContent);
        }
        private void ExibirMensagemErro(string titulo, string conteudo)
        {
            var msg = new ContentDialog { Title = titulo, Content = conteudo, CloseButtonText = "OK" };
            msg.ShowAsync();
        }
        #endregion
    }
}
