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
    public class CLParametroRepositorio : ICLParametroRepositorio
    {
        public int Inserir(CLParametro cLParametro)
        {
            try
            {
                if (cLParametro == null)
                    return -1;

                if (string.IsNullOrEmpty(cLParametro.Parametro))
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

                if(pExiste != null)
                {
                    cLParametro.ValidationResult = new ValidationResult("O Parâmetro já foi registrado anteriormente.");
                    return -1;
                }

                var resultado = App.DBConnection.Insert(cLParametro);

                if(resultado == null)
                {
                    cLParametro.ValidationResult = new ValidationResult("Não foi possível registrar o parâmetro na base de dados.");
                    return -1;
                }

                var cLParametroRegistrado = App.DBConnection.Table<CLParametro>()
                    .Where(i => i.Parametro == cLParametro.Parametro && i.Grupo == cLParametro.Grupo && i.Valor == cLParametro.Valor)
                    .FirstOrDefault();

                if(cLParametroRegistrado == null)
                {
                    cLParametro.ValidationResult = new ValidationResult("Não foi possível obter o ID do parâmetro registrado.");
                    return -1;
                }

                return cLParametro.PK_Parametro;
            }
            catch (Exception ex)
            {
                cLParametro.ValidationResult = new ValidationResult(ex.Message);
            }
            
            return -1;
        }
    
        public bool Atualizar(CLParametro cLParametro)
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

                var resultado = App.DBConnection.Update(cLParametro);

                if (resultado <= 0)
                {
                    cLParametro.ValidationResult = new ValidationResult("Não foi possível atualizar o parâmetro na base de dados!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                cLParametro.ValidationResult = new ValidationResult(ex.Message);
            }

            return true;
        }

        public CLParametro Obter(Parametros parametro)
        {
            var clParametro = new CLParametro()
            {
                Parametro = Enum.GetName(typeof(Parametros), parametro),
                ValidationResult = null,
            };

            var resultado = Obter(clParametro);

            if(resultado == null)
            {
                clParametro.ValidationResult = new ValidationResult("Nenhum parâmetro encontrado.");
                return clParametro;
            }

            return resultado;
        }

        public CLParametro Obter(string parametro)
        {
            var clParametro = new CLParametro()
            {
                Parametro = parametro,
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

        public CLParametro Obter(CLParametro parametro)
        {
            try
            {

                if (parametro == null)
                {
                    parametro = new CLParametro
                    {
                        ValidationResult = new ValidationResult("É necessário informar alguma parâmetro."),
                    };

                    return parametro;
                }

                if(parametro.PK_Parametro <= 0 && string.IsNullOrEmpty(parametro.Parametro))
                {
                    parametro.ValidationResult = new ValidationResult("É necessário inserir algum ID ou Parametro.");
                    return parametro;
                }

                CLParametro cLParametroResultado = null;

                if (parametro.PK_Parametro > 0)
                {
                    cLParametroResultado = App.DBConnection
                    .Table<CLParametro>()
                    .Where(x => x.PK_Parametro == parametro.PK_Parametro)
                    .FirstOrDefault();
                }
                else if(parametro.Parametro.Length > 0)
                {
                    cLParametroResultado = App.DBConnection
                    .Table<CLParametro>()
                    .Where(x => x.Parametro == parametro.Parametro)
                    .FirstOrDefault();
                }

                if(cLParametroResultado == null)
                {
                    parametro.ValidationResult = new ValidationResult("Nenhum parâmetro encontrado.");
                    return parametro;
                }

                return cLParametroResultado;
            }
            catch (Exception ex)
            {
                parametro = new CLParametro
                {
                    ValidationResult = new ValidationResult(ex.Message),
                };

                return parametro;
            }
        }

    }
}
