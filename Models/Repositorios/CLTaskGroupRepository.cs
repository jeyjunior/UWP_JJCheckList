using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;

namespace UWP_JJCheckList.Models.Repositorios
{
    public class CLTaskGroupRepository : ICLTaskGroupRepository
    {
        public void InserirGrupoPadrao()
        {
            var clTaskGroup = new CLTaskGroup() { GroupName = "Tarefas Gerais", FK_CLTaskColor = 1 };
            var clTaskGroup2 = new CLTaskGroup() { GroupName = "Teste 1", FK_CLTaskColor = 3 };
            var clTaskGroup3 = new CLTaskGroup() { GroupName = "Teste 2", FK_CLTaskColor = 5 };

            try
            {
                App.DBConnection.BeginTransaction();
                App.DBConnection.Insert(clTaskGroup);
                App.DBConnection.Insert(clTaskGroup2);
                App.DBConnection.Insert(clTaskGroup3);
                App.DBConnection.Commit();
            }
            catch
            {
                App.DBConnection.Rollback();
                throw;
            }
        }

        public IEnumerable<CLTaskGroup> ObterLista()
        {
            var clTaskGroupCollection = new List<CLTaskGroup>();
            
            try
            {
                clTaskGroupCollection = App.DBConnection.Table<CLTaskGroup>().ToList();
                return clTaskGroupCollection;
            }
            catch
            {
                return clTaskGroupCollection;
            }
        }
    }
}
