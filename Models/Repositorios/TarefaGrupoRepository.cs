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
    public class TarefaGrupoRepository : ITarefaGrupoRepository
    {
        public void InserirGrupoPadrao()
        {
            var clTaskGroup = new TarefaGrupo() { Nome = "Tarefas Gerais", FK_TarefaCor = 1 };
            var clTaskGroup2 = new TarefaGrupo() { Nome = "Teste 1", FK_TarefaCor = 3 };
            var clTaskGroup3 = new TarefaGrupo() { Nome = "Teste 2", FK_TarefaCor = 5 };

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

        public IEnumerable<TarefaGrupo> ObterLista()
        {
            var clTaskGroupCollection = new List<TarefaGrupo>();
            
            try
            {
                clTaskGroupCollection = App.DBConnection.Table<TarefaGrupo>().ToList();
                return clTaskGroupCollection;
            }
            catch
            {
                return clTaskGroupCollection;
            }
        }
    }
}
