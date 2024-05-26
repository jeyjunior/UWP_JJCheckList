using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
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
    public sealed partial class TaskContent : UserControl, ITaskContent
    {
        #region Interfaces
        private readonly ITarefaCorRepository tarefaCorRepositorio;
        #endregion

        #region Propriedades
        private IMainPageManipularComponentes mainPageManipularComponentes;
        private Tarefa tarefa { get; set; }
        private bool abrirNotepad = false;
        private bool AtualizarBase = false;
        #endregion

        #region Método Construtor
        public TaskContent(Tarefa tarefa, IMainPageManipularComponentes mainPageManipularComponentes)
        {
            this.InitializeComponent();

            this.tarefa = tarefa;
            this.mainPageManipularComponentes = mainPageManipularComponentes;

            tarefaCorRepositorio = App.Container.GetInstance<ITarefaCorRepository>();
            AtualizarInterface();
        }
        #endregion

        private void AtualizarInterface()
        {
            this.tgbTarefa.IsChecked = tarefa.Concluido;
            this.txtTarefa.Text = tarefa.Descricao;
            this.txtNotepad.Text = tarefa.BlocoDeNotas;

            AtualizarBase = true;
            // Cor e grupo
        }

        #region Eventos
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarCorDoGrupo();
            gridPrincipal.RowDefinitions[1].Height = new GridLength(0);
        }
        private void txtTarefa_LostFocus(object sender, RoutedEventArgs e)
        {
            AtualizarInformacoesBase(TipoOperacao.Atualizar);
        }
        private void tgbTarefa_Checked(object sender, RoutedEventArgs e)
        {
            AtualizarInformacoesBase(TipoOperacao.Atualizar);
        }
        private void tgbTarefa_Unchecked(object sender, RoutedEventArgs e)
        {
            AtualizarInformacoesBase(TipoOperacao.Atualizar);
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
                AtualizarInformacoesBase(TipoOperacao.Atualizar);
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
            AtualizarInformacoesBase(TipoOperacao.Atualizar);
        }
        #endregion

        #region Métodos
        private void AtualizarInformacoesBase(TipoOperacao tipoOperacao)
        {
            try
            {
                if (AtualizarBase)
                {
                    tarefa.Descricao = this.txtTarefa.Text;
                    tarefa.Concluido = (bool)this.tgbTarefa.IsChecked;
                    tarefa.BlocoDeNotas = this.txtNotepad.Text;

                    App.AddOperacao(new TarefaOperacao { TipoOperacao = tipoOperacao, Tarefa = tarefa });
                }
            }
            catch (Exception ex)
            {
                Aviso.ContentDialog("Erro", ex.Message);
            }
        }
        private void Deletar()
        {
            AtualizarInformacoesBase(TipoOperacao.Deletar);
            mainPageManipularComponentes.DeletarItem(this);
        }
        private void CarregarCorDoGrupo()
        {
            this.fColor.Background = tarefaCorRepositorio.ObterCorGrupo(tarefa);
        }
        #endregion

        #region Métodos Público
        public Tarefa ObterTask()
        {
            return this.tarefa;
        }
        public void AtualizarCheck(bool check)
        {
            this.tarefa.Concluido = check;
            this.tgbTarefa.IsChecked = this.tarefa.Concluido;

            AtualizarInterface();
        }
        public void DeletarTarefa()
        {
            Deletar();
        }
        #endregion
    }
}

