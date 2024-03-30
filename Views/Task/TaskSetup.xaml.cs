using System.ServiceModel.Channels;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;
using UWP_JJCheckList.Models.Repositorios;
using UWP_JJCheckList.Views.Task;
using Windows.UI.Xaml.Controls;


namespace UWP_JJCheckList.Assets
{
    public sealed partial class TaskSetup : ContentDialog
    {
        #region Interfaces
        private readonly ICLTaskContentRepositorio cLTaskContentRepositorio;
        #endregion

        #region Propriedades
        public ListView listView;
        private CLTaskContent clTaskContent;
        #endregion

        #region Método Construtor
        public TaskSetup()
        {
            this.InitializeComponent();

            cLTaskContentRepositorio = App.Container.GetInstance<ICLTaskContentRepositorio>();  

            clTaskContent = new CLTaskContent() { Checked = false, };
        }
        #endregion

        #region Eventos
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            clTaskContent.Tarefa = txtTarefa.Text;
            int pK_CLTaskContent = cLTaskContentRepositorio.Inserir(clTaskContent);

            if (!clTaskContent.IsValid)
            {
                ExibirMensagemErro("Erro", clTaskContent.ValidationResult.ErrorMessage);
                return;
            }

            clTaskContent.PK_CLTaskContent = pK_CLTaskContent;
            TaskContent taskContent = new TaskContent(clTaskContent);
            taskContent.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            taskContent.DeletarItem += (s, e) => { listView.Items.Remove(taskContent); };

            listView.Items.Add(taskContent);

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
        
        private void DeletarItem()
        {

        }
        #endregion
    }
}
