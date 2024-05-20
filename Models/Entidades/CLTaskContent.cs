﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SQLite;

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
        public string Notepad { get; set; }
        [NotNull]
        public int FK_CLTaskGroup { get; set; }

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

            if (PK_CLTaskContent <= 0)
            {
                ValidationResult = new ValidationResult("O ID da tarefa é obrigatório.");
                return false;
            }
            else if (Tarefa == null)
            {
                ValidationResult = new ValidationResult("A tarefa é obrigatório.");
                return false;
            }
            else if (Notepad == null)
            {
                ValidationResult = new ValidationResult("O bloco de notas é obrigatório.");
                return false;
            }
            else if (FK_CLTaskGroup <= 0)
            {
                ValidationResult = new ValidationResult("O ID do grupo é obrigatório.");
                return false;
            }
            return false;
        }
    }
}
