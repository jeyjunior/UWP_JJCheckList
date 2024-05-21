using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;

namespace UWP_JJCheckList.Models.Repositorios
{
    public class CLTaskColorRepositorio : ICLTaskColorRepositorio
    {
        public void InserirCoresPadrao()
        {
            var clTaskColorCollection = new List<CLTaskColor>()
            {
                new CLTaskColor() { ColorName = "Preto", ColorHex = "#000000" },
                new CLTaskColor() { ColorName = "Branco", ColorHex = "#ffffff" },
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

    }
}
