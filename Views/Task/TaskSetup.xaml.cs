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
        #region Propriedades
        private IMainPageManipularComponentes mainPageManipularComponentes;
        private CLTaskContent taskContent;
        #endregion

        #region Método Construtor
        public TaskSetup(IMainPageManipularComponentes mainPageManipularComponentes)
        {
            this.InitializeComponent();

            this.mainPageManipularComponentes = mainPageManipularComponentes;
            taskContent = new CLTaskContent();
        }
        #endregion

        #region Eventos
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            taskContent.Tarefa = this.txtTarefa.Text;
            this.mainPageManipularComponentes.AddNovoItem(taskContent);

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

            taskContent.Tarefa = "";
            taskContent.Checked = false;
            taskContent.IndiceLista = 0;
        }
        private void ExibirMensagemErro(string titulo, string conteudo)
        {
            var msg = new ContentDialog { Title = titulo, Content = conteudo, CloseButtonText = "OK" };
            msg.ShowAsync();
        }
        #endregion
    }
}
