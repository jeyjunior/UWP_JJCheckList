using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWP_JJCheckList.Assets;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;
using UWP_JJCheckList.Models.Repositorios;
using UWP_JJCheckList.Views.Task;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel;
 
namespace UWP_JJCheckList
{
    public sealed partial class MainPage : Page, IMainPageManipularComponentes
    {
        #region Interfaces
        private readonly IParametroRepository cLParametroRepositorio;
        private readonly ITarefaRepository cLTaskContentRepositorio;
        #endregion

        #region Propriedades
        private Parametro pTituloPrincipal;
        private Parametro pTituloPrincipalFontSize;
        private List<Tarefa> cLTaskContentCollection;
        #endregion

        #region Views
        private UWP_JJCheckList.Assets.TaskSetup taskSetup;
        #endregion

        #region Construtor
        public MainPage()
        {
            this.InitializeComponent();

            cLParametroRepositorio = App.Container.GetInstance<IParametroRepository>();
            cLTaskContentRepositorio = App.Container.GetInstance<ITarefaRepository>();

            cLTaskContentCollection = new List<Tarefa>();
        }
        #endregion

        #region Eventos
        // Main
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarParametros();
            CarregarTasks();

            taskSetup = new UWP_JJCheckList.Assets.TaskSetup(this);

            if (listViewConteudo.Items.Count > 0)
            {
                listViewConteudo.ScrollIntoView(this.listViewConteudo.Items.First());

            }
            else
            {
                this.btnAdd.Focus(FocusState.Programmatic);
            }

            HabilitarComponentes();
            AtualizarContador();
        }

        // Título
        private void txtTitulo_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                this.txtTitulo.Visibility = Visibility.Collapsed;
                this.txbTitulo.Visibility = Visibility.Visible;

                SalvarTitulo();
            }
        }
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
            taskSetup.ShowAsync();
        }
        private async void btnDeletarAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.listViewConteudo.Items.Count > 0)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Opções de exclusão",
                        Content = "Selecione uma opção:",
                        PrimaryButtonText = "Deletar tudo",
                        SecondaryButtonText = "Deletar selecionados",
                        CloseButtonText = "Cancelar"
                    };

                    var result = await dialog.ShowAsync();

                    CoreWindow.GetForCurrentThread().PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);

                    foreach (ITaskContent item in listViewConteudo.Items)
                    {
                        if(result == ContentDialogResult.Primary)
                        {
                            item.DeletarTarefa();
                        }
                        else if(result == ContentDialogResult.Secondary)
                        {
                            if(item.ObterTask() is Tarefa tarefa && tarefa.Concluido == true)
                            {
                                item.DeletarTarefa();
                            }
                        }
                    }
                }
            }
            finally
            {
                CoreWindow.GetForCurrentThread().PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
        }
        private void chkAll_Checked(object sender, RoutedEventArgs e)
        {
            CoreWindow.GetForCurrentThread().PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);

            try
            {
                if (this.listViewConteudo.Items.Count > 0)
                {
                    this.listViewConteudo.Items
                        .OfType<ITaskContent>()
                        .ToList()
                        .ForEach(i => i.AtualizarCheck(true));
                }
            }
            finally
            {
                CoreWindow.GetForCurrentThread().PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
        }
        private void chkAll_Unchecked(object sender, RoutedEventArgs e)
        {
            CoreWindow.GetForCurrentThread().PointerCursor = new CoreCursor(CoreCursorType.Wait, 1);

            try
            {
                if (this.listViewConteudo.Items.Count > 0)
                {
                    this.listViewConteudo.Items
                        .OfType<ITaskContent>()
                        .ToList()
                        .ForEach(i => i.AtualizarCheck(false));
                }
            }
            finally
            {
                CoreWindow.GetForCurrentThread().PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
        }
        private void pMainPage_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            bool ctrlPressed = (Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Control) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;
            bool shiftPressed = (Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Shift) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;

            // Nova tarefa
            if (e.Key == Windows.System.VirtualKey.N)
            {
                if (ctrlPressed)
                {
                    btnAdd_Click(null, null);
                    e.Handled = true; 
                }
            }

            // Deletar Tudo
            if (e.Key == Windows.System.VirtualKey.D && ctrlPressed && shiftPressed)
            {
                btnDeletarAll_Click(null, null);
                e.Handled = true;
            }

            // Concluir Tudo
            if (e.Key == Windows.System.VirtualKey.C && ctrlPressed && shiftPressed)
            {
                if (chkAll.IsChecked == true)
                {
                    chkAll.IsChecked = false;
                    chkAll_Unchecked(null, null);
                }
                else
                {
                    chkAll.IsChecked = true;
                    chkAll_Checked(null, null);
                }

                e.Handled = true;
            }
        }
        private void listViewConteudo_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //if (listViewConteudo.SelectedItem is TaskContent task && task != null)
            //{
            //    bool ctrlPressed = (Window.Current.CoreWindow.GetKeyState(Windows.System.VirtualKey.Control) & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;

            //    // Abrir Notepad
            //    if (e.Key == Windows.System.VirtualKey.E && ctrlPressed)
            //    {
            //        task.AbrirNotepad();
            //    }

            //    // Deletar Item
            //    if (e.Key == Windows.System.VirtualKey.Delete)
            //    {
            //        task.DeletarItemAsync();
            //    }
            //}
        }
        #endregion

        #region Métodos
        private void CarregarParametros()
        {
            try
            {
                pTituloPrincipal = new Parametro();


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
        private void CarregarTasks()
        {
            try
            {
                var taskContentCollection = cLTaskContentRepositorio.ObterLista();

                if (taskContentCollection == null)
                    return;

                if (taskContentCollection.Count() <= 0)
                    return;

                foreach (var item in taskContentCollection)
                {
                    if (item == null)
                        continue;

                    var taskContent = new TaskContent(item, this);
                    this.listViewConteudo.Items.Add(taskContent);
                }

                HabilitarComponentes();
                AtualizarContador();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void HabilitarComponentes()
        {
            bool habilitar = (this.listViewConteudo.Items.Count > 0);
            this.chkAll.Visibility = habilitar ? Visibility.Visible : Visibility.Collapsed;
            this.btnDeletarAll.Visibility = habilitar ? Visibility.Visible : Visibility.Collapsed;
        }
        private void AtualizarContador()
        {
            //this.txbQtd.Text = "";

            //if(this.listViewConteudo.Items.Count > 0)
            //{
            //    int qtdTotal = this.listViewConteudo.Items.Count;
            //    int qtdMarcada = this.listViewConteudo
            //        .Items
            //        .OfType<TaskContent>()
            //        .Where(i => i.ObterCheckBoxStatus() == true)
            //        .Count();

            //    this.txbQtd.Text = $"{qtdMarcada}/{qtdTotal}";
            //}
        }
        private void SalvarTitulo()
        {
            try
            {
                pTituloPrincipal.ValidationResult = null;
                pTituloPrincipal.Valor = this.txtTitulo.Text.Trim();
                cLParametroRepositorio.Atualizar(pTituloPrincipal);

                if (!pTituloPrincipal.IsValid)
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

        #region Métodos Público
        public void AddNovoItem(Tarefa clTaskContent)
        {
            TaskContent taskContent = new TaskContent(clTaskContent, this);
            taskContent.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            var taskResult = Task.Run(() => cLTaskContentRepositorio.InserirAsync(clTaskContent));

            if (!clTaskContent.IsValid)
            {
                ExibirMensagemErro("Erro", clTaskContent.ValidationResult.ErrorMessage);
                return;
            }
            else if (taskResult.Result <= 0)
            {
                ExibirMensagemErro("Erro", "Não foi possível registrar tarefa.");
                return;
            }

            clTaskContent.PK_Tarefa = taskResult.Result;

            listViewConteudo.Items.Add(taskContent);
            cLTaskContentCollection.Add(clTaskContent);

            HabilitarComponentes();
            AtualizarContador();
        }
        public void DeletarItem(TaskContent taskContent)
        {
            this.listViewConteudo.Items.Remove(taskContent);

            HabilitarComponentes();
            AtualizarContador();
        }
        #endregion
    }
}
