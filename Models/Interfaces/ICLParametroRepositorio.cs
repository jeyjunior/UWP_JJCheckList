using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;

namespace UWP_JJCheckList.Models.Interfaces
{
    public interface ICLParametroRepositorio
    {
        int Inserir(CLParametro cLParametro);
        bool Atualizar(CLParametro cLParametro);
        CLParametro Obter(Parametros parametro);
        CLParametro Obter(string parametro);
        CLParametro Obter(CLParametro parametro);
    }
}
