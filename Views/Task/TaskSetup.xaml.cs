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
        private readonly ITarefaGrupoRepository tarefaGrupoRepository;
        private readonly ITarefaCorRepository tarefaCorRepository;
        private IMainPageManipularComponentes mainPageManipularComponentes;
        #endregion

        #region Propriedades
        private Tarefa tarefa;
        #endregion

        #region Método Construtor
        public TaskSetup(IMainPageManipularComponentes mainPageManipularComponentes)
        {
            this.InitializeComponent();

            this.mainPageManipularComponentes = mainPageManipularComponentes;

            tarefaGrupoRepository = App.Container.GetInstance<ITarefaGrupoRepository>();
            tarefaCorRepository = App.Container.GetInstance<ITarefaCorRepository>();

            tarefa = new Tarefa();

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
                var clTaskGroupCollection = tarefaGrupoRepository.ObterLista();

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

                this.cboTaskGroup.DisplayMemberPath = "Nome";
                this.cboTaskGroup.SelectedValuePath = "PK_TarefaGrupo";
            }
            catch (System.Exception ex)
            {
                Aviso.Toast(ex.Message);
            }
        }
        private void Limpar()
        {
            this.txtTarefa.Text = "";

            tarefa.Descricao = "";
            tarefa.Concluido = false;
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

                tarefa.Descricao = this.txtTarefa.Text;
                tarefa.Concluido = false;
                tarefa.BlocoDeNotas = "";
                tarefa.FK_TarefaGrupo = (int)cboTaskGroup.SelectedValue;
                this.mainPageManipularComponentes.AddNovoItem(tarefa);
                salvarTarefa = true;
                Limpar();

                Sair:;
            }
            catch (System.Exception ex)
            {
                Aviso.Toast(ex.Message);
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
