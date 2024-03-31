using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Views.Task;
using Windows.UI.Xaml;

namespace UWP_JJCheckList.Models.Interfaces
{
    public interface IMainPageManipularComponentes
    {
        void AddItem(TaskContent taskContent);
        void DeletarItem(TaskContent taskContent);
    }
}
