using System.ServiceModel.Channels;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;
using UWP_JJCheckList.Models.Repositorios;
using UWP_JJCheckList.Views.Task;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace UWP_JJCheckList.Assets
{
    public sealed partial class TaskSetup : ContentDialog
    {
        #region Interfaces
        private readonly ICLTaskContentRepositorio cLTaskContentRepositorio;
        #endregion

        #region Propriedades
        private IMainPageManipularComponentes mainPageManipularComponentes;
        private CLTaskContent clTaskContent;
        #endregion

        #region Método Construtor
        public TaskSetup(IMainPageManipularComponentes mainPageManipularComponentes)
        {
            this.InitializeComponent();

            this.mainPageManipularComponentes = mainPageManipularComponentes;
            cLTaskContentRepositorio = App.Container.GetInstance<ICLTaskContentRepositorio>();  

            clTaskContent = new CLTaskContent() { Checked = false, };
        }
        #endregion

        #region Eventos
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            clTaskContent.Tarefa = txtTarefa.Text;
            clTaskContent.Checked = false;

            var taskResult = Task.Run(() => cLTaskContentRepositorio.InserirAsync(clTaskContent));
            

            if (!clTaskContent.IsValid)
            {
                ExibirMensagemErro("Erro", clTaskContent.ValidationResult.ErrorMessage);
                return;
            }
            else if(taskResult.Result <= 0)
            {
                ExibirMensagemErro("Erro", "Não foi possível registrar tarefa.");
                return;
            }

            clTaskContent.PK_CLTaskContent = taskResult.Result;
            TaskContent taskContent = new TaskContent(clTaskContent, mainPageManipularComponentes);
            taskContent.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            this.mainPageManipularComponentes.AddItem(taskContent);

            Limpar();
        }
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Limpar();
        }
        #endregion

        #region Métodos
        private void Limpar()
        {
            this.txtTarefa.Text = "";
        }
        private void ExibirMensagemErro(string titulo, string conteudo)
        {
            var msg = new ContentDialog { Title = titulo, Content = conteudo, CloseButtonText = "OK" };
            msg.ShowAsync();
        }
        #endregion
    }
}
