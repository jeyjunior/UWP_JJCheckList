﻿using System;
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
        private readonly ICLParametroRepositorio cLParametroRepositorio;
        private readonly ICLTaskContentRepositorio cLTaskContentRepositorio;
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
            cLTaskContentRepositorio = App.Container.GetInstance<ICLTaskContentRepositorio>();
        }
        #endregion

        #region Eventos
        // Main
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarParametros();
            CarregarTasks();

            taskSetup = new TaskSetup(this);

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

                    if (result == ContentDialogResult.Primary)
                    {
                        this.listViewConteudo.Items
                            .OfType<ITaskContentManipularComponentes>()
                            .ToList()
                            .ForEach(i => i.DeletarItem());

                        this.listViewConteudo.Items.Clear();
                    }
                    else if (result == ContentDialogResult.Secondary)
                    {
                        this.listViewConteudo.Items
                            .OfType<ITaskContentManipularComponentes>()
                            .ToList()
                            .ForEach(i =>
                            {
                                if (i.ObterCheckBoxStatus() == true)
                                {
                                    i.DeletarItem();
                                    listViewConteudo.Items.Remove(i);
                                }
                            });
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
                        .OfType<ITaskContentManipularComponentes>()
                        .ToList()
                        .ForEach(i => i.MarcarCheckBox());
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
                                                .OfType<ITaskContentManipularComponentes>()
                                                .ToList()
                                                .ForEach(i => i.DesmarcarCheckBox());
                }
            }
            finally
            {
                CoreWindow.GetForCurrentThread().PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
            }
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
                    if(item == null)
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
        public void AddNovoItem(CLTaskContent clTaskContent)
        {
            TaskContent taskContent = new TaskContent(clTaskContent, this);
            taskContent.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            this.listViewConteudo.Items.Add(taskContent);

            int indice = this.listViewConteudo.Items.IndexOf(taskContent);
            clTaskContent.IndiceLista = indice;

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

            clTaskContent.PK_CLTaskContent = taskResult.Result;

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
