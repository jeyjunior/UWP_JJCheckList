using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UWP_JJCheckList.Controls.Helpers
{
    using Windows.UI.Notifications;
    using Windows.UI.Xaml.Controls;

    public static class Aviso
    {
        public static void Toast(string mensagem)
        {
            ExibirToast(mensagem);
        }

        private static void ExibirToast(string mensagem)
        {
            ToastNotification toast = new ToastNotification(ObterXmlToast(mensagem));

            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private static Windows.Data.Xml.Dom.XmlDocument ObterXmlToast(string mensagem)
        {
            string toastXml = $@"
            <toast>
                <visual>
                    <binding template='ToastGeneric'>
                        <text>{mensagem}</text>
                    </binding>
                </visual>
                <audio src='ms-winsoundevent:Notification.Mail' />
            </toast>";

            var xmlDoc = new Windows.Data.Xml.Dom.XmlDocument();
            xmlDoc.LoadXml(toastXml);

            return xmlDoc;
        }

        public static async void ContentDialog(string mensagem)
        {
            var dialog = new ContentDialog()
            {
                Title = "Mensagem",
                Content = mensagem,
                CloseButtonText = "OK"
            };

             await dialog.ShowAsync();
        }
    }
}
