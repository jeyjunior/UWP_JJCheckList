using System.ServiceModel.Channels;
using System.Threading.Tasks;
using UWP_JJCheckList.Controls.Helpers;
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

            Limpar();
        }
        #endregion

        #region Eventos
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            AdicionarTarefa();
        }
        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Limpar();
        }
        private void txtTarefa_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                AdicionarTarefa();
                this.Hide();
            }
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
        private void AdicionarTarefa()
        {
            try
            {
                taskContent.Tarefa = this.txtTarefa.Text;
                taskContent.Checked = false;
                taskContent.IndiceLista = 0;

                this.mainPageManipularComponentes.AddNovoItem(taskContent);

                Limpar();
            }
            catch (System.Exception ex)
            {
                Aviso.ContentDialog("Erro", ex.Message);
            }
            finally
            {
                this.Hide();
            }
        }
        #endregion
    }
}
