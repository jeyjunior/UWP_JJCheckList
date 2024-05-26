using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Views.Task;
using Windows.UI.Xaml;

namespace UWP_JJCheckList.Models.Interfaces
{
    public interface IMainPageManipularComponentes
    {
        void AddNovoItem(Tarefa taskContent);
        void DeletarItem(TaskContent taskContent);
    }
}
