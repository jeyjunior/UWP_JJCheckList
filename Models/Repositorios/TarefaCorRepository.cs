using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;
using UWP_JJCheckList.Views.Task;
using Windows.UI.Xaml.Media;

namespace UWP_JJCheckList.Models.Repositorios
{
    public class TarefaCorRepository : ITarefaCorRepository
    {
        public void InserirCoresPadrao()
        {
            var clTaskColorCollection = new List<TarefaCor>()
            {
                new TarefaCor() { Nome = "Transparente", CorHex = "#292929" },
                new TarefaCor() { Nome = "Branco", CorHex = "#ffffff" },
                new TarefaCor() { Nome = "Preto", CorHex = "#000000" },
                new TarefaCor() { Nome = "Vermelho", CorHex = "#ff0000" },
                new TarefaCor() { Nome = "Verde", CorHex = "#00ff00" },
                new TarefaCor() { Nome = "Azul", CorHex = "#0000ff" },
                new TarefaCor() { Nome = "Amarelo", CorHex = "#FFFF00" },
                new TarefaCor() { Nome = "Ciano", CorHex = "#00FFFF" },
                new TarefaCor() { Nome = "Magenta", CorHex = "#FF00FF" },
                new TarefaCor() { Nome = "Cinza", CorHex = "#808080" },
                new TarefaCor() { Nome = "Laranja", CorHex = "#FFA500" },
            };

            try
            {
                App.DBConnection.BeginTransaction();

                foreach (var item in clTaskColorCollection)
                {
                    App.DBConnection.Insert(item);
                }

                App.DBConnection.Commit();
            }
            catch
            {
                App.DBConnection.Rollback();
                throw;
            }
        }

        public SolidColorBrush ObterCorGrupo(Tarefa clTaskContent)
        {
            try
            {
                var clTaskGrupo = App.DBConnection.Table<TarefaGrupo>().FirstOrDefault(grupo => grupo.PK_TarefaGrupo == clTaskContent.FK_TarefaGrupo); ;
                var clTaskColor = App.DBConnection.Table<TarefaCor>().FirstOrDefault(color => color.PK_TarefaCor == clTaskGrupo.FK_TarefaCor);

                if (clTaskColor != null)
                {
                    Windows.UI.Color cor = Windows.UI.Color.FromArgb(255,
                        Convert.ToByte(clTaskColor.CorHex.Substring(1, 2), 16),
                        Convert.ToByte(clTaskColor.CorHex.Substring(3, 2), 16),
                        Convert.ToByte(clTaskColor.CorHex.Substring(5, 2), 16));

                    return new SolidColorBrush(cor);
                }
            }
            catch (SQLiteException ex)
            {

            }
            return null; 
        }
    }
}
