using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using UWP_JJCheckList.Controls;
using UWP_JJCheckList.Controls.Helpers;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
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
        private readonly ICLTaskContentRepository cLTaskContentRepositorio;
        private readonly ICLTaskColorRepository cLTaskColorRepositorio;
        #endregion

        #region Propriedades
        private IMainPageManipularComponentes mainPageManipularComponentes;
        private CLTaskContent clTaskContent { get; set; }
        private bool abrirNotepad = false;
        #endregion

        #region Método Construtor
        public TaskContent(CLTaskContent clTaskContent, IMainPageManipularComponentes mainPageManipularComponentes)
        {
            this.InitializeComponent();

            this.clTaskContent = clTaskContent;
            this.mainPageManipularComponentes = mainPageManipularComponentes;

            cLTaskContentRepositorio = App.Container.GetInstance<ICLTaskContentRepository>();
            cLTaskColorRepositorio = App.Container.GetInstance<ICLTaskColorRepository>();

            this.tgbTarefa.IsChecked = clTaskContent.Checked;
            this.txtTarefa.Text = clTaskContent.Tarefa;
            this.txtNotepad.Text = clTaskContent.Notepad;
        }
        #endregion

        #region Eventos
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarCorDoGrupo();
            gridPrincipal.RowDefinitions[1].Height = new GridLength(0);
        }
        private void txtTarefa_LostFocus(object sender, RoutedEventArgs e)
        {
            AtualizarInformacoesBase();
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
            Deletar();
        }
        private void UserControl_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            this.txtTarefa.Focus(FocusState.Pointer);
        }
        private void txtTarefa_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter || e.Key == VirtualKey.Tab)
            {
                AtualizarInformacoesBase();
            }
        }
        private void btnNotepad_Click(object sender, RoutedEventArgs e)
        {
            if (this.btnNotepad.FocusState == FocusState.Unfocused)
                this.btnNotepad.Focus(FocusState.Programmatic);

            if (btnNotepad.Content == null)
                return;

            Image img = btnNotepad.Content as Image;

            if (img == null)
                return;

            CompositeTransform ct = img.RenderTransform as CompositeTransform;

            if (abrirNotepad == false)
            {
                ct.Rotation = 90;
                abrirNotepad = true;
                gridPrincipal.RowDefinitions[1].Height = new GridLength(200);
            }
            else
            {
                ct.Rotation = 0;
                abrirNotepad = false;
                gridPrincipal.RowDefinitions[1].Height = new GridLength(0);
            }
        }
        private void TxtNotepad_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //if (e.Key == Windows.System.VirtualKey.Tab)
            //{
            //    int caretIndex = txtNotepad.SelectionStart;
            //    txtNotepad.Text = txtNotepad.Text.Insert(caretIndex, "\t");
            //    txtNotepad.SelectionStart = caretIndex + 1;
            //    e.Handled = true;
            //}
        }
        private void txtNotepad_LostFocus(object sender, RoutedEventArgs e)
        {
            AtualizarInformacoesBase();
        }
        #endregion

        #region Métodos
        private void AtualizarInformacoesBase()
        {
            try
            {
                clTaskContent.Tarefa = this.txtTarefa.Text;
                clTaskContent.Checked = (bool)this.tgbTarefa.IsChecked;
                clTaskContent.Notepad = this.txtNotepad.Text;
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

        private void CarregarCorDoGrupo()
        {
            this.fColor.Background = cLTaskColorRepositorio.ObterCorGrupo(clTaskContent);
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
            //this.clTaskContent.IndiceLista = indice;
        }
        public void AbrirNotepad()
        {
            btnNotepad_Click(null, null);
        }
        #endregion
    }
}

