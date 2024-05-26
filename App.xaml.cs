using SimpleInjector;
using SQLite;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using UWP_JJCheckList.Controls.Helpers;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;
using UWP_JJCheckList.Models.Repositorios;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UWP_JJCheckList
{
    /// <summary>
    ///Fornece o comportamento específico do aplicativo para complementar a classe Application padrão.
    /// </summary>
    sealed partial class App : Application
    {
        public static Container Container { get; private set; }
        public static SQLiteConnection DBConnection { get; private set; }

        private static readonly ConcurrentQueue<TarefaOperacao> tarefaOperacoes = new ConcurrentQueue<TarefaOperacao>();
        private static bool ExecutandoOperacoes = false;

        public App()
        {
            this.InitializeComponent();

            InicializarRepositorios();
            InicializarBaseDados();
            InicializarParametros();

            this.Suspending += OnSuspending;
        }
        /// <summary>
        /// Invocado quando o aplicativo é iniciado normalmente pelo usuário final. Outros pontos de entrada
        /// serão usados, por exemplo, quando o aplicativo for iniciado para abrir um arquivo específico.
        /// </summary>
        /// <param name="e">Detalhes sobre a solicitação e o processo de inicialização.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Não repita a inicialização do aplicativo quando a Janela já tiver conteúdo,
            // apenas verifique se a janela está ativa
            if (rootFrame == null)
            {
                // Crie um Quadro para atuar como o contexto de navegação e navegue para a primeira página
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Carregue o estado do aplicativo suspenso anteriormente
                }

                // Coloque o quadro na Janela atual
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Quando a pilha de navegação não for restaurada, navegar para a primeira página,
                    // configurando a nova página passando as informações necessárias como um parâmetro
                    // de navegação
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Verifique se a janela atual está ativa
                Window.Current.Activate();
            }
        }
        /// <summary>
        /// Chamado quando ocorre uma falha na Navegação para uma determinada página
        /// </summary>
        /// <param name="sender">O Quadro com navegação com falha</param>
        /// <param name="e">Detalhes sobre a falha na navegação</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }
        /// <summary>
        /// Invocado quando a execução do aplicativo é suspensa. O estado do aplicativo é salvo
        /// sem saber se o aplicativo será encerrado ou retomado com o conteúdo
        /// da memória ainda intacto.
        /// </summary>
        /// <param name="sender">A origem da solicitação de suspensão.</param>
        /// <param name="e">Detalhes sobre a solicitação de suspensão.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Salvar o estado do aplicativo e parar qualquer atividade em segundo plano
            deferral.Complete();
        }
        private void InicializarRepositorios()
        {
            Container = new Container();

            Container.Register<IParametroRepository, ParametroRepository>();
            Container.Register<ITarefaRepository, TarefaRepository>();
            Container.Register<ITarefaGrupoRepository, TarefaGrupoRepository>();
            Container.Register<ITarefaCorRepository, TarefaCorRepository>();

            Container.Verify();
        }
        private void InicializarBaseDados()
        {
            try
            {
                string dbPath = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "DBCheckList.db");
                DBConnection = new SQLiteConnection(dbPath);

                DBConnection.BeginTransaction();
                //Tabelas
                DBConnection.CreateTable<Parametro>();
                DBConnection.CreateTable<Tarefa>();
                DBConnection.CreateTable<TarefaCor>();
                DBConnection.CreateTable<TarefaGrupo>();

                DBConnection.Commit();
            }
            catch (Exception ex)
            {
                DBConnection.Rollback();

                var msg = new ContentDialog
                {
                    Title = "Erro",
                    Content = ex.Message,
                    CloseButtonText = "OK"
                };

                msg.ShowAsync();
            }
        }
        private void InicializarParametros()
        {
            try
            {
                string parametroTeste = Enum.GetName(typeof(Parametros), Parametros.TituloPrincipal);
                var consultaTeste = DBConnection.Table<Parametro>().Where(i => i.Nome == parametroTeste).FirstOrDefault();

                if (consultaTeste != null)
                    return;

                // Paramêtros iniciais
                var pTituloPrincipal = new Parametro
                {
                    Grupo = Enum.GetName(typeof(GrupoParametros), GrupoParametros.MainPage),
                    Nome = Enum.GetName(typeof(Parametros), Parametros.TituloPrincipal),
                    Valor = "Título",
                };
                var pTituloPrincipalFontSize = new Parametro
                {
                    Grupo = Enum.GetName(typeof(GrupoParametros), GrupoParametros.MainPage),
                    Nome = Enum.GetName(typeof(Parametros), Parametros.TituloPrincipalFontSize),
                    Valor = "30",
                };


                DBConnection.BeginTransaction();
                DBConnection.Insert(pTituloPrincipal);
                DBConnection.Insert(pTituloPrincipalFontSize);
                DBConnection.Commit();

                // CORES INICIAIS
                var cLTaskColorRepository = App.Container.GetInstance<Models.Interfaces.ITarefaCorRepository>();
                cLTaskColorRepository.InserirCoresPadrao();

                // GRUPO PADRÃO DO SISTEMA
                var clTaskGroupRepository = App.Container.GetInstance<ITarefaGrupoRepository>();
                clTaskGroupRepository.InserirGrupoPadrao();
            }
            catch (Exception ex)
            {
                DBConnection.Rollback();
                var msg = new ContentDialog { Title = "Erro", Content = ex.Message, CloseButtonText = "OK" };
                msg.ShowAsync();
            }
        }

        public static void AddOperacao(TarefaOperacao tarefaOperacao)
        {
            tarefaOperacoes.Enqueue(tarefaOperacao);

            if (!ExecutandoOperacoes)
            {
                ExecutandoOperacoes = true;
                Task.Run(() => ExecutarOperacoes());
            }
        }

        private static async Task ExecutarOperacoes()
        {
            var tarefaRepository = App.Container.GetInstance<ITarefaRepository>();
  
            while (tarefaOperacoes.TryDequeue(out TarefaOperacao tarefaOperacao))
            {
                try
                {
                    switch (tarefaOperacao.TipoOperacao)
                    {
                        case Controls.Helpers.TipoOperacao.Adicionar:
                            await tarefaRepository.InserirAsync(tarefaOperacao.Tarefa);
                            break;
                        case Controls.Helpers.TipoOperacao.Atualizar:
                            await tarefaRepository.AtualizarAsync(tarefaOperacao.Tarefa);
                            break;
                        case Controls.Helpers.TipoOperacao.Deletar:
                            await tarefaRepository.DeletarAsync(tarefaOperacao.Tarefa);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Aviso.Toast(ex.Message);
                }
            }

            ExecutandoOperacoes = false;
        }
    }
}
