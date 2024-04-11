using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;


namespace UWP_JJCheckList.Models.Interfaces
{
    public interface ICLTaskContentRepositorio
    {
        int Inserir(CLTaskContent taskContent);
        Task<int> InserirAsync(CLTaskContent taskContent);
        bool Atualizar(CLTaskContent taskContent);
        Task<bool> AtualizarAsync(CLTaskContent taskContent);
        CLTaskContent Obter(CLTaskContent taskContent);
        Task<CLTaskContent> ObterAsync(CLTaskContent taskContent);
        IEnumerable<CLTaskContent> ObterLista();
        bool Deletar(CLTaskContent taskContent);
        Task<bool> DeletarAsync(CLTaskContent taskContent);
    }
}
