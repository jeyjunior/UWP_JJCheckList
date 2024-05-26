using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace UWP_JJCheckList.Models.Repositorios
{
    public class ParametroRepository : IParametroRepository
    {
        public int Inserir(Parametro cLParametro)
        {
            try
            {
                if (cLParametro == null)
                    return -1;

                if (string.IsNullOrEmpty(cLParametro.Nome))
                {
                    cLParametro.ValidationResult = new ValidationResult("Parâmetro inválido.");
                    return -1;
                }
                else if (string.IsNullOrEmpty(cLParametro.Grupo))
                {
                    cLParametro.ValidationResult = new ValidationResult("Grupo inválido.");
                    return -1;
                }
                else if (string.IsNullOrEmpty(cLParametro.Valor))
                {
                    cLParametro.ValidationResult = new ValidationResult("Valor inválido.");
                    return -1;
                }

                var pExiste = Obter(cLParametro);

                if (pExiste != null)
                {
                    cLParametro.ValidationResult = new ValidationResult("O Parâmetro já foi registrado anteriormente.");
                    return -1;
                }

                App.DBConnection.BeginTransaction();
                var resultado = App.DBConnection.Insert(cLParametro);

                if (resultado == null)
                {
                    App.DBConnection.Rollback();
                    cLParametro.ValidationResult = new ValidationResult("Não foi possível registrar o parâmetro na base de dados.");
                    return -1;
                }
                else
                    App.DBConnection.Commit();



                var cLParametroRegistrado = App.DBConnection.Table<Parametro>()
                    .Where(i => i.Nome == cLParametro.Nome && i.Grupo == cLParametro.Grupo && i.Valor == cLParametro.Valor)
                    .FirstOrDefault();

                if (cLParametroRegistrado == null)
                {
                    cLParametro.ValidationResult = new ValidationResult("Não foi possível obter o ID do parâmetro registrado.");
                    return -1;
                }

                return cLParametro.PK_Parametro;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();
                cLParametro.ValidationResult = new ValidationResult(ex.Message);
            }

            return -1;
        }

        public bool Atualizar(Parametro cLParametro)
        {
            try
            {
                if (cLParametro == null)
                    return false;

                if (cLParametro.PK_Parametro <= 0)
                {
                    cLParametro.ValidationResult = new ValidationResult("ID inválido!");
                    return false;
                }

                App.DBConnection.BeginTransaction();
                var resultado = App.DBConnection.Update(cLParametro);

                if (resultado <= 0)
                {
                    App.DBConnection.Rollback();
                    cLParametro.ValidationResult = new ValidationResult("Não foi possível atualizar o parâmetro na base de dados!");
                    return false;
                }
                else
                    App.DBConnection.Commit();

            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();
                cLParametro.ValidationResult = new ValidationResult(ex.Message);
            }

            return true;
        }

        public Parametro Obter(Parametros parametro)
        {
            var clParametro = new Parametro()
            {
                Nome = Enum.GetName(typeof(Parametros), parametro),
                ValidationResult = null,
            };

            var resultado = Obter(clParametro);

            if (resultado == null)
            {
                clParametro.ValidationResult = new ValidationResult("Nenhum parâmetro encontrado.");
                return clParametro;
            }

            return resultado;
        }

        public Parametro Obter(string parametro)
        {
            var clParametro = new Parametro()
            {
                Nome = parametro,
                ValidationResult = null,
            };

            var resultado = Obter(clParametro);

            if (resultado == null)
            {
                clParametro.ValidationResult = new ValidationResult("Nenhum parâmetro encontrado.");
                return clParametro;
            }

            return resultado;
        }

        public Parametro Obter(Parametro parametro)
        {
            try
            {

                if (parametro == null)
                {
                    parametro = new Parametro
                    {
                        ValidationResult = new ValidationResult("É necessário informar alguma parâmetro."),
                    };

                    return parametro;
                }

                if (parametro.PK_Parametro <= 0 && string.IsNullOrEmpty(parametro.Nome))
                {
                    parametro.ValidationResult = new ValidationResult("É necessário inserir algum ID ou Parametro.");
                    return parametro;
                }

                Parametro cLParametroResultado = null;

                if (parametro.PK_Parametro > 0)
                {
                    cLParametroResultado = App.DBConnection
                    .Table<Parametro>()
                    .Where(x => x.PK_Parametro == parametro.PK_Parametro)
                    .FirstOrDefault();
                }
                else if (parametro.Nome.Length > 0)
                {
                    cLParametroResultado = App.DBConnection
                    .Table<Parametro>()
                    .Where(x => x.Nome == parametro.Nome)
                    .FirstOrDefault();
                }

                if (cLParametroResultado == null)
                {
                    parametro.ValidationResult = new ValidationResult("Nenhum parâmetro encontrado.");
                    return parametro;
                }

                return cLParametroResultado;
            }
            catch (Exception ex)
            {
                parametro = new Parametro
                {
                    ValidationResult = new ValidationResult(ex.Message),
                };

                return parametro;
            }
        }

    }
}
