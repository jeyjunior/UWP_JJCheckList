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
    public class CLTaskColorRepository : ICLTaskColorRepository
    {
        public void InserirCoresPadrao()
        {
            var clTaskColorCollection = new List<CLTaskColor>()
            {
                new CLTaskColor() { ColorName = "Transparente", ColorHex = "#292929" },
                new CLTaskColor() { ColorName = "Branco", ColorHex = "#ffffff" },
                new CLTaskColor() { ColorName = "Preto", ColorHex = "#000000" },
                new CLTaskColor() { ColorName = "Vermelho", ColorHex = "#ff0000" },
                new CLTaskColor() { ColorName = "Verde", ColorHex = "#00ff00" },
                new CLTaskColor() { ColorName = "Azul", ColorHex = "#0000ff" },
                new CLTaskColor() { ColorName = "Amarelo", ColorHex = "#FFFF00" },
                new CLTaskColor() { ColorName = "Ciano", ColorHex = "#00FFFF" },
                new CLTaskColor() { ColorName = "Magenta", ColorHex = "#FF00FF" },
                new CLTaskColor() { ColorName = "Cinza", ColorHex = "#808080" },
                new CLTaskColor() { ColorName = "Laranja", ColorHex = "#FFA500" },
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

        public SolidColorBrush ObterCorGrupo(CLTaskContent clTaskContent)
        {
            try
            {
                var clTaskGrupo = App.DBConnection.Table<CLTaskGroup>().FirstOrDefault(grupo => grupo.PK_CLTaskGroup == clTaskContent.FK_CLTaskGroup); ;
                var clTaskColor = App.DBConnection.Table<CLTaskColor>().FirstOrDefault(color => color.PK_CLTaskColor == clTaskGrupo.FK_CLTaskColor);

                if (clTaskColor != null)
                {
                    Windows.UI.Color cor = Windows.UI.Color.FromArgb(255,
                        Convert.ToByte(clTaskColor.ColorHex.Substring(1, 2), 16),
                        Convert.ToByte(clTaskColor.ColorHex.Substring(3, 2), 16),
                        Convert.ToByte(clTaskColor.ColorHex.Substring(5, 2), 16));

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
