using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SQLite;
using UWP_JJCheckList.Controls.Helpers;

namespace UWP_JJCheckList.Models.Entidades
{
    public class Tarefa
    {
        [PrimaryKey]
        [AutoIncrement]
        public int PK_Tarefa { get; set; }
        [NotNull]
        public bool Concluido { get; set; }
        [NotNull]
        public string Descricao { get; set; }
        [NotNull]
        public string BlocoDeNotas { get; set; }
        [NotNull]
        public int FK_TarefaGrupo { get; set; }

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

            if (PK_Tarefa <= 0)
            {
                ValidationResult = new ValidationResult("O ID da tarefa é obrigatório.");
                return false;
            }
            else if (Descricao == null)
            {
                ValidationResult = new ValidationResult("A tarefa é obrigatório.");
                return false;
            }
            else if (BlocoDeNotas == null)
            {
                ValidationResult = new ValidationResult("O bloco de notas é obrigatório.");
                return false;
            }
            else if (FK_TarefaGrupo <= 0)
            {
                ValidationResult = new ValidationResult("O ID do grupo é obrigatório.");
                return false;
            }
            return false;
        }
    }

    public class TarefaOperacao
    {
        public TipoOperacao TipoOperacao { get; set; }
        public Tarefa Tarefa {get; set;}
    }
}
