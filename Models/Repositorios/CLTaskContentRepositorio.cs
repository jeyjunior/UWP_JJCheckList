using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;
using UWP_JJCheckList.Views.Task;

namespace UWP_JJCheckList.Models.Repositorios
{
    public class CLTaskContentRepositorio : ICLTaskContentRepositorio
    {
        public int Inserir(CLTaskContent taskContent)
        {
            try
            {
                if (taskContent == null)
                    return -1;

                App.DBConnection.BeginTransaction();

                var resultado = App.DBConnection.Insert(taskContent);

                if (resultado <= 0)
                {
                    App.DBConnection.Rollback();
                    taskContent.ValidationResult = new ValidationResult("Não foi possível registrar a tarefa na base de dados.");
                    return -1;
                }
                else
                    App.DBConnection.Commit();


                var ultimoItem = App.DBConnection.Table<CLTaskContent>()
                    .OrderByDescending(item => item.PK_CLTaskContent)
                    .FirstOrDefault();

                if (ultimoItem == null)
                {
                    taskContent.ValidationResult = new ValidationResult("Não foi possível obter o ID da tarefa registrada.");
                    return -1;
                }

                return taskContent.PK_CLTaskContent;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();
                taskContent.ValidationResult = new ValidationResult(ex.Message);
                return -1;
            }
        }
        public async Task<int> InserirAsync(CLTaskContent taskContent)
        {
            try
            {
                if (taskContent == null)
                    return -1;

                App.DBConnection.BeginTransaction();
                var resultado = await Task.Run(() => App.DBConnection.Insert(taskContent));

                if (resultado <= 0)
                {
                    App.DBConnection.Rollback();
                    taskContent.ValidationResult = new ValidationResult("Não foi possível registrar a tarefa na base de dados.");
                    return -1;
                }
                else
                    App.DBConnection.Commit();

                var ultimoItem = await Task.Run(() =>
                    App.DBConnection
                    .Table<CLTaskContent>()
                    .OrderByDescending(item => item.PK_CLTaskContent)
                    .FirstOrDefault());

                if (ultimoItem == null)
                {
                    taskContent.ValidationResult = new ValidationResult("Não foi possível obter o ID da tarefa registrada.");
                    return -1;
                }

                return taskContent.PK_CLTaskContent;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();
                taskContent.ValidationResult = new ValidationResult(ex.Message);
                return -1;
            }
        }
        public bool Atualizar(CLTaskContent taskContent)
        {
            try
            {
                if (taskContent == null)
                    return false;

                if (taskContent.PK_CLTaskContent <= 0)
                {
                    taskContent.ValidationResult = new ValidationResult("ID inválido!");
                    return false;
                }

                App.DBConnection.BeginTransaction();
                var resultado = App.DBConnection.Update(taskContent);

                if (resultado <= 0)
                {
                    App.DBConnection.Rollback();
                    taskContent.ValidationResult = new ValidationResult("Não foi possível atualizar a tarefa na base de dados!");
                    return false;
                }
                else
                    App.DBConnection.Commit();

                return true;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();
                taskContent.ValidationResult = new ValidationResult(ex.Message);
                return false;
            }
        }
        public async Task<bool> AtualizarAsync(CLTaskContent taskContent)
        {
            try
            {
                if (taskContent == null)
                    return false;

                if (taskContent.PK_CLTaskContent <= 0)
                {
                    taskContent.ValidationResult = new ValidationResult("ID inválido!");
                    return false;
                }

                App.DBConnection.BeginTransaction();
                var resultado = await Task.Run(() => App.DBConnection.Update(taskContent));

                if (resultado <= 0)
                {
                    App.DBConnection.Rollback();
                    taskContent.ValidationResult = new ValidationResult("Não foi possível atualizar a tarefa na base de dados!");
                    return false;
                }
                else
                    App.DBConnection.Commit();

                return true;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();
                taskContent.ValidationResult = new ValidationResult(ex.Message);
                return false;
            }
        }
        public CLTaskContent Obter(CLTaskContent taskContent)
        {
            try
            {

                if (taskContent == null)
                {
                    taskContent = new CLTaskContent
                    {
                        ValidationResult = new ValidationResult("É necessário informar alguma tarefa."),
                    };

                    return taskContent;
                }

                if (taskContent.PK_CLTaskContent <= 0 && string.IsNullOrEmpty(taskContent.Tarefa))
                {
                    taskContent.ValidationResult = new ValidationResult("É necessário inserir algum ID ou Tarefa.");
                    return taskContent;
                }

                CLTaskContent taskContentResultado = null;

                if (taskContent.PK_CLTaskContent > 0)
                {
                    taskContentResultado = App.DBConnection
                    .Table<CLTaskContent>()
                    .Where(x => x.PK_CLTaskContent == taskContent.PK_CLTaskContent)
                    .FirstOrDefault();
                }
                else if (taskContent.Tarefa.Length > 0)
                {
                    taskContentResultado = App.DBConnection
                    .Table<CLTaskContent>()
                    .Where(x => x.Tarefa == taskContent.Tarefa)
                    .FirstOrDefault();
                }

                if (taskContentResultado == null)
                {
                    taskContent.ValidationResult = new ValidationResult("Nenhuma tarefa encontrada.");
                    return taskContent;
                }

                return taskContentResultado;
            }
            catch (Exception ex)
            {
                taskContent = new CLTaskContent
                {
                    ValidationResult = new ValidationResult(ex.Message),
                };

                return taskContent;
            }
        }
        public async Task<CLTaskContent> ObterAsync(CLTaskContent taskContent)
        {
            try
            {
                if (taskContent == null)
                {
                    taskContent = new CLTaskContent
                    {
                        ValidationResult = new ValidationResult("É necessário informar alguma tarefa."),
                    };

                    return taskContent;
                }

                if (taskContent.PK_CLTaskContent <= 0 && string.IsNullOrEmpty(taskContent.Tarefa))
                {
                    taskContent.ValidationResult = new ValidationResult("É necessário inserir algum ID ou Tarefa.");
                    return taskContent;
                }

                CLTaskContent taskContentResultado = null;

                if (taskContent.PK_CLTaskContent > 0)
                {
                    taskContentResultado = await Task.Run(() => App.DBConnection
                        .Table<CLTaskContent>()
                        .Where(x => x.PK_CLTaskContent == taskContent.PK_CLTaskContent)
                        .FirstOrDefault());
                }
                else if (!string.IsNullOrEmpty(taskContent.Tarefa))
                {
                    taskContentResultado = await Task.Run(() => App.DBConnection
                        .Table<CLTaskContent>()
                        .Where(x => x.Tarefa == taskContent.Tarefa)
                        .FirstOrDefault());
                }

                if (taskContentResultado == null)
                {
                    taskContent.ValidationResult = new ValidationResult("Nenhuma tarefa encontrada.");
                    return taskContent;
                }

                return taskContentResultado;
            }
            catch (Exception ex)
            {
                taskContent = new CLTaskContent
                {
                    ValidationResult = new ValidationResult(ex.Message),
                };

                return taskContent;
            }
        }
        public IEnumerable<CLTaskContent> ObterLista()
        {
            try
            {
                var taskContentCollection = new List<CLTaskContent>();
                taskContentCollection = App.DBConnection.Table<CLTaskContent>().ToList();

                return taskContentCollection;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool Deletar(CLTaskContent taskContent)
        {
            try
            {
                if (taskContent == null)
                    return false;

                if (taskContent.PK_CLTaskContent <= 0 && string.IsNullOrEmpty(taskContent.Tarefa))
                {
                    taskContent.ValidationResult = new ValidationResult("É necessário inserir algum ID ou Tarefa.");
                    return false;
                }

                App.DBConnection.BeginTransaction();
                var taskContentResultado = App.DBConnection.Delete(taskContent);

                if (taskContentResultado < 0)
                {
                    App.DBConnection.Rollback();
                    taskContent.ValidationResult = new ValidationResult("Falha ao tentar deletar tarefa.");
                    return false;
                }
                else
                    App.DBConnection.Commit();

                return true;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();

                taskContent = new CLTaskContent
                {
                    ValidationResult = new ValidationResult(ex.Message),
                };

                return false;
            }
        }
        public async Task<bool> DeletarAsync(CLTaskContent taskContent)
        {
            try
            {
                if (taskContent == null)
                    return false;

                if (taskContent.PK_CLTaskContent <= 0 && string.IsNullOrEmpty(taskContent.Tarefa))
                {
                    taskContent.ValidationResult = new ValidationResult("É necessário inserir algum ID ou Tarefa.");
                    return false;
                }

                App.DBConnection.BeginTransaction();

                var taskContentResultado = await Task.Run(() => App.DBConnection.Delete(taskContent));

                if (taskContentResultado < 0)
                {
                    App.DBConnection.Rollback();
                    taskContent.ValidationResult = new ValidationResult("Falha ao tentar deletar tarefa.");
                    return false;
                }
                else
                    App.DBConnection.Commit();

                return true;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();

                taskContent = new CLTaskContent
                {
                    ValidationResult = new ValidationResult(ex.Message),
                };

                return false;
            }
        }
    }
}
