using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SQLite;

namespace UWP_JJCheckList.Models.Entidades
{
    public class TarefaCor
    {
        [PrimaryKey]
        [AutoIncrement]
        public int PK_TarefaCor { get; set; }
        [NotNull]
        public string Nome { get; set; }
        [NotNull]
        public string CorHex { get; set; }
        
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

            if (PK_TarefaCor <= 0)
            {
                ValidationResult = new ValidationResult("O ID da cor é obrigatório.");
                return false;
            }
            else if (string.IsNullOrEmpty(Nome))
            {
                ValidationResult = new ValidationResult("O nome da cor é obrigatório.");
                return false;
            }
            else if (string.IsNullOrEmpty(CorHex))
            {
                ValidationResult = new ValidationResult("O código da cor é obrigatório.");
                return false;
            }

            return false;
        }
    }
}
