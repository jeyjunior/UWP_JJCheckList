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
    public class TarefaRepository : ITarefaRepository
    {
        public int Inserir(Tarefa tarefa)
        {
            try
            {
                if (tarefa == null)
                    return -1;

                App.DBConnection.BeginTransaction();

                var resultado = App.DBConnection.Insert(tarefa);

                if (resultado <= 0)
                {
                    App.DBConnection.Rollback();
                    tarefa.ValidationResult = new ValidationResult("Não foi possível registrar a tarefa na base de dados.");
                    return -1;
                }
                else
                    App.DBConnection.Commit();


                var ultimoItem = App.DBConnection.Table<Tarefa>()
                    .OrderByDescending(item => item.PK_Tarefa)
                    .FirstOrDefault();

                if (ultimoItem == null)
                {
                    tarefa.ValidationResult = new ValidationResult("Não foi possível obter o ID da tarefa registrada.");
                    return -1;
                }

                return tarefa.PK_Tarefa;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();
                tarefa.ValidationResult = new ValidationResult(ex.Message);
                return -1;
            }
        }
        public async Task<int> InserirAsync(Tarefa tarefa)
        {
            try
            {
                if (tarefa == null)
                    return -1;

                App.DBConnection.BeginTransaction();
                var resultado = await Task.Run(() => App.DBConnection.Insert(tarefa));

                if (resultado <= 0)
                {
                    App.DBConnection.Rollback();
                    tarefa.ValidationResult = new ValidationResult("Não foi possível registrar a tarefa na base de dados.");
                    return -1;
                }
                else
                    App.DBConnection.Commit();

                var ultimoItem = await Task.Run(() =>
                    App.DBConnection
                    .Table<Tarefa>()
                    .OrderByDescending(item => item.PK_Tarefa)
                    .FirstOrDefault());

                if (ultimoItem == null)
                {
                    tarefa.ValidationResult = new ValidationResult("Não foi possível obter o ID da tarefa registrada.");
                    return -1;
                }

                return tarefa.PK_Tarefa;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();
                tarefa.ValidationResult = new ValidationResult(ex.Message);
                return -1;
            }
        }
        public bool Atualizar(Tarefa tarefa)
        {
            try
            {
                if (tarefa == null)
                    return false;

                if (tarefa.PK_Tarefa <= 0)
                {
                    tarefa.ValidationResult = new ValidationResult("ID inválido!");
                    return false;
                }

                App.DBConnection.BeginTransaction();
                var resultado = App.DBConnection.Update(tarefa);

                if (resultado <= 0)
                {
                    App.DBConnection.Rollback();
                    tarefa.ValidationResult = new ValidationResult("Não foi possível atualizar a tarefa na base de dados!");
                    return false;
                }
                else
                    App.DBConnection.Commit();

                return true;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();
                tarefa.ValidationResult = new ValidationResult(ex.Message);
                return false;
            }
        }
        public async Task<bool> AtualizarAsync(Tarefa tarefa)
        {
            try
            {
                if (tarefa == null)
                    return false;

                if (tarefa.PK_Tarefa <= 0)
                {
                    tarefa.ValidationResult = new ValidationResult("ID inválido!");
                    return false;
                }

                App.DBConnection.BeginTransaction();
                var resultado = await Task.Run(() => App.DBConnection.Update(tarefa));

                if (resultado <= 0)
                {
                    App.DBConnection.Rollback();
                    tarefa.ValidationResult = new ValidationResult("Não foi possível atualizar a tarefa na base de dados!");
                    return false;
                }
                else
                    App.DBConnection.Commit();

                return true;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();
                tarefa.ValidationResult = new ValidationResult(ex.Message);
                return false;
            }
        }
        public Tarefa Obter(Tarefa tarefa)
        {
            try
            {

                if (tarefa == null)
                {
                    tarefa = new Tarefa
                    {
                        ValidationResult = new ValidationResult("É necessário informar alguma tarefa."),
                    };

                    return tarefa;
                }

                if (tarefa.PK_Tarefa <= 0 && string.IsNullOrEmpty(tarefa.Descricao))
                {
                    tarefa.ValidationResult = new ValidationResult("É necessário inserir algum ID ou Tarefa.");
                    return tarefa;
                }

                Tarefa tarefaResult = null;

                if (tarefa.PK_Tarefa > 0)
                {
                    tarefaResult = App.DBConnection
                    .Table<Tarefa>()
                    .Where(x => x.PK_Tarefa == tarefa.PK_Tarefa)
                    .FirstOrDefault();
                }
                else if (tarefa.Descricao.Length > 0)
                {
                    tarefaResult = App.DBConnection
                    .Table<Tarefa>()
                    .Where(x => x.Descricao == tarefa.Descricao)
                    .FirstOrDefault();
                }

                if (tarefaResult == null)
                {
                    tarefa.ValidationResult = new ValidationResult("Nenhuma tarefa encontrada.");
                    return tarefa;
                }

                return tarefaResult;
            }
            catch (Exception ex)
            {
                tarefa = new Tarefa
                {
                    ValidationResult = new ValidationResult(ex.Message),
                };

                return tarefa;
            }
        }
        public async Task<Tarefa> ObterAsync(Tarefa tarefa)
        {
            try
            {
                if (tarefa == null)
                {
                    tarefa = new Tarefa
                    {
                        ValidationResult = new ValidationResult("É necessário informar alguma tarefa."),
                    };

                    return tarefa;
                }

                if (tarefa.PK_Tarefa <= 0 && string.IsNullOrEmpty(tarefa.Descricao))
                {
                    tarefa.ValidationResult = new ValidationResult("É necessário inserir algum ID ou Tarefa.");
                    return tarefa;
                }

                Tarefa tarefaResult = null;

                if (tarefa.PK_Tarefa > 0)
                {
                    tarefaResult = await Task.Run(() => App.DBConnection
                        .Table<Tarefa>()
                        .Where(x => x.PK_Tarefa == tarefa.PK_Tarefa)
                        .FirstOrDefault());
                }
                else if (!string.IsNullOrEmpty(tarefa.Descricao))
                {
                    tarefaResult = await Task.Run(() => App.DBConnection
                        .Table<Tarefa>()
                        .Where(x => x.Descricao == tarefa.Descricao)
                        .FirstOrDefault());
                }

                if (tarefaResult == null)
                {
                    tarefa.ValidationResult = new ValidationResult("Nenhuma tarefa encontrada.");
                    return tarefa;
                }

                return tarefaResult;
            }
            catch (Exception ex)
            {
                tarefa = new Tarefa
                {
                    ValidationResult = new ValidationResult(ex.Message),
                };

                return tarefa;
            }
        }
        public IEnumerable<Tarefa> ObterLista()
        {
            try
            {
                var tarefaCollection = new List<Tarefa>();
                tarefaCollection = App.DBConnection.Table<Tarefa>().ToList();

                return tarefaCollection;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool Deletar(Tarefa tarefa)
        {
            try
            {
                if (tarefa == null)
                    return false;

                if (tarefa.PK_Tarefa <= 0 && string.IsNullOrEmpty(tarefa.Descricao))
                {
                    tarefa.ValidationResult = new ValidationResult("É necessário inserir algum ID ou Tarefa.");
                    return false;
                }

                App.DBConnection.BeginTransaction();
                var taskContentResultado = App.DBConnection.Delete(tarefa);

                if (taskContentResultado < 0)
                {
                    App.DBConnection.Rollback();
                    tarefa.ValidationResult = new ValidationResult("Falha ao tentar deletar tarefa.");
                    return false;
                }
                else
                    App.DBConnection.Commit();

                return true;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();

                tarefa = new Tarefa
                {
                    ValidationResult = new ValidationResult(ex.Message),
                };

                return false;
            }
        }
        public async Task<bool> DeletarAsync(Tarefa tarefa)
        {
            try
            {
                if (tarefa == null)
                    return false;

                if (tarefa.PK_Tarefa <= 0 && string.IsNullOrEmpty(tarefa.Descricao))
                {
                    tarefa.ValidationResult = new ValidationResult("É necessário inserir algum ID ou Tarefa.");
                    return false;
                }

                App.DBConnection.BeginTransaction();

                var taskContentResultado = await Task.Run(() => App.DBConnection.Delete(tarefa));

                if (taskContentResultado < 0)
                {
                    App.DBConnection.Rollback();
                    tarefa.ValidationResult = new ValidationResult("Falha ao tentar deletar tarefa.");
                    return false;
                }
                else
                    App.DBConnection.Commit();

                return true;
            }
            catch (Exception ex)
            {
                App.DBConnection.Rollback();

                tarefa = new Tarefa
                {
                    ValidationResult = new ValidationResult(ex.Message),
                };

                return false;
            }
        }
    }
}
