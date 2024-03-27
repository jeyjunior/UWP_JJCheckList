using UWP_JJCheckList.Views.Task;
using Windows.UI.Xaml.Controls;


namespace UWP_JJCheckList.Assets
{
    public sealed partial class TaskSetup : ContentDialog
    {
        #region Propriedades
        public UIElementCollection children;
        #endregion

        #region Método Construtor
        public TaskSetup()
        {
            this.InitializeComponent();
        }
        #endregion

        #region Eventos
        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            children.Add(new TaskContent());
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
        #endregion
    }
}
