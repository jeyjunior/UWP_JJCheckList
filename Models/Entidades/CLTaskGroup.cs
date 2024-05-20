using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SQLite;

namespace UWP_JJCheckList.Models.Entidades
{
    public class CLTaskGroup
    {
        [PrimaryKey]
        [AutoIncrement]
        public int PK_CLTaskGroup { get; set; }
        [NotNull]
        public string GroupName { get; set; }
        [NotNull]
        public int FK_CLTaskColor { get; set; }
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

            if (PK_CLTaskGroup <= 0)
            {
                ValidationResult = new ValidationResult("O ID do grupo é obrigatório.");
                return false;
            }
            else if (string.IsNullOrEmpty(GroupName))
            {
                ValidationResult = new ValidationResult("O nome do grupo é obrigatório.");
                return false;
            }
            else if (FK_CLTaskColor <= 0)
            {
                ValidationResult = new ValidationResult("O código da cor é obrigatório.");
                return false;
            }

            return false;
        }
    }
}
