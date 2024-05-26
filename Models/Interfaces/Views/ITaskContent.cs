using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Views.Task;

namespace UWP_JJCheckList.Models.Interfaces
{
    public interface ITaskContent
    {
        Tarefa ObterTask();
        void AtualizarCheck(bool check);
        void DeletarTarefa();
    }
}
