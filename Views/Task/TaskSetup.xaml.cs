using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using UWP_JJCheckList.Controls.Helpers;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;
using UWP_JJCheckList.Models.Repositorios;
using UWP_JJCheckList.Views.Task;
using System.Linq;

namespace UWP_JJCheckList.Assets
{
    public sealed partial class TaskSetup : ContentDialog
    {
        #region Interfaces
        private readonly ICLTaskGroupRepository clTaskGroupRepository;
        private readonly ICLTaskColorRepository cLTaskColorRepository;
        private IMainPageManipularComponentes mainPageManipularComponentes;
        #endregion

        #region Propriedades
        private CLTaskContent taskContent;
        #endregion

        #region Método Construtor
        public TaskSetup(IMainPageManipularComponentes mainPageManipularComponentes)
        {
            this.InitializeComponent();

            this.mainPageManipularComponentes = mainPageManipularComponentes;

            clTaskGroupRepository = App.Container.GetInstance<ICLTaskGroupRepository>();
            cLTaskColorRepository = App.Container.GetInstance<ICLTaskColorRepository>();

            taskContent = new CLTaskContent();

            CarregarDropDowns();
            Limpar();
        }
        #endregion

        #region Eventos
        private void btnAdicionar_Click(object sender, RoutedEventArgs e)
        {
            AdicionarTarefa();
        }
        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            Limpar();
        }
        private void txtTarefa_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnAdicionar.Visibility = (this.txtTarefa.Text.Trim() == "") ? Visibility.Collapsed : Visibility.Visible;
        }
        private void txtTarefa_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter && btnAdicionar.Visibility == Visibility.Visible)
            {
                AdicionarTarefa();
                this.Hide();
            }

        }
        #endregion

        #region Métodos
        private void CarregarDropDowns()
        {
            CarregarGrupo();
        }

        private void CarregarGrupo()
        {
            try
            {
                var clTaskGroupCollection = clTaskGroupRepository.ObterLista();

                if(clTaskGroupCollection == null)
                {
                    Aviso.Toast("Falha ao carregar os grupos.");
                    return;
                }

                if(clTaskGroupCollection.Count() <= 0)
                {
                    Aviso.Toast("Nenhum grupo encontrado.");
                    return;
                }

                foreach (var item in clTaskGroupCollection)
                    this.cboTaskGroup.Items.Add(item);

                this.cboTaskGroup.DisplayMemberPath = "GroupName";
                this.cboTaskGroup.SelectedValuePath = "PK_CLTaskGroup";
            }
            catch (System.Exception)
            {
                throw;
            }
        }
        private void Limpar()
        {
            this.txtTarefa.Text = "";

            taskContent.Tarefa = "";
            taskContent.Checked = false;
            this.btnAdicionar.Visibility = Visibility.Collapsed;
            this.cboTaskGroup.SelectedIndex = 0;
            this.Hide();
        }
        private void AdicionarTarefa()
        {
            bool salvarTarefa = false;

            try
            {

                if (string.IsNullOrEmpty(this.txtTarefa.Text.Trim()))
                {
                    Aviso.Toast("É necessário inserir algum texto para a tarefa.");
                    txtTarefa.Focus(FocusState.Programmatic);
                    goto Sair;
                }

                taskContent.Tarefa = this.txtTarefa.Text;
                taskContent.Checked = false;
                taskContent.Notepad = "";
                taskContent.FK_CLTaskGroup = (int)cboTaskGroup.SelectedValue;
                this.mainPageManipularComponentes.AddNovoItem(taskContent);
                salvarTarefa = true;
                Limpar();

                Sair:;
            }
            catch (System.Exception ex)
            {
                Aviso.ContentDialog("Erro", ex.Message);
            }
            finally
            {
                if (this.Visibility == Visibility.Visible && salvarTarefa)
                    this.Hide();
            }
        }
        #endregion
    }
}
