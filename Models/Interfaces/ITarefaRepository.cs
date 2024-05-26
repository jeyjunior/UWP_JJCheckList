using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;


namespace UWP_JJCheckList.Models.Interfaces
{
    public interface ITarefaRepository
    {
        int Inserir(Tarefa taskContent);
        Task<int> InserirAsync(Tarefa taskContent);
        bool Atualizar(Tarefa taskContent);
        Task<bool> AtualizarAsync(Tarefa taskContent);
        Tarefa Obter(Tarefa taskContent);
        Task<Tarefa> ObterAsync(Tarefa taskContent);
        IEnumerable<Tarefa> ObterLista();
        bool Deletar(Tarefa taskContent);
        Task<bool> DeletarAsync(Tarefa taskContent);
    }
}
