using SQLite;
using System;
using System.Collections.Generic;
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
    }
}
