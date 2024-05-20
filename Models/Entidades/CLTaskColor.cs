using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SQLite;

namespace UWP_JJCheckList.Models.Entidades
{
    public class CLTaskColor
    {
        [PrimaryKey]
        [AutoIncrement]
        public int PK_CLTaskColor { get; set; }
        [NotNull]
        public string ColorName { get; set; }
        [NotNull]
        public string ColorHex { get; set; }
        
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

            if (PK_CLTaskColor <= 0)
            {
                ValidationResult = new ValidationResult("O ID da cor é obrigatório.");
                return false;
            }
            else if (string.IsNullOrEmpty(ColorName))
            {
                ValidationResult = new ValidationResult("O nome da cor é obrigatório.");
                return false;
            }
            else if (string.IsNullOrEmpty(ColorHex))
            {
                ValidationResult = new ValidationResult("O código da cor é obrigatório.");
                return false;
            }

            return false;
        }
    }
}
