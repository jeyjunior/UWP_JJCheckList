using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;

namespace UWP_JJCheckList.Models.Interfaces
{
    public interface ICLTaskGroupRepository
    {
        void InserirGrupoPadrao();
        IEnumerable<CLTaskGroup> ObterLista();
    }
}
