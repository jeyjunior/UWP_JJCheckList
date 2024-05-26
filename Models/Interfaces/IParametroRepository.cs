using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;

namespace UWP_JJCheckList.Models.Interfaces
{
    public interface IParametroRepository
    {
        int Inserir(Parametro cLParametro);
        bool Atualizar(Parametro cLParametro);
        Parametro Obter(Parametros parametro);
        Parametro Obter(string parametro);
        Parametro Obter(Parametro parametro);
    }
}
