using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_JJCheckList.Models.Entidades
{
    public class CLParametro
    {
        [PrimaryKey][AutoIncrement]
        public int PK_Parametro { get; set; }
        [NotNull]
        public string Grupo { get; set; }
        [NotNull]
        public string Parametro { get; set;}
        [NotNull]
        public string Valor { get; set; }

        [Ignore]
        public ValidationResult ValidationResult { get; set; }
        [Ignore]
        public bool IsValid => IsValidInternal();

        private bool IsValidInternal()
        {
            if (ValidationResult == null)
                return true;
            else if (ValidationResult.ErrorMessage.Count() == 0)
                return true;

            return false;
        }
    }

    public enum Parametros
    {
        TituloPrincipal = 0,
    }

    public enum GrupoParametros
    {
        MainPage = 0,
    }
}
