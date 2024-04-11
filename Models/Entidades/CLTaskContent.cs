using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_JJCheckList.Models.Entidades
{
    public class CLTaskContent
    {
        [PrimaryKey]
        [AutoIncrement]
        public int PK_CLTaskContent { get; set; }
        [NotNull]
        public bool Checked { get; set; }
        [NotNull]
        public string Tarefa { get; set; }
        [NotNull]
        public int IndiceLista { get; set; }

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
}
