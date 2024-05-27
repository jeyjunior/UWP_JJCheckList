using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Controls.Helpers;
using UWP_JJCheckList.Models.Entidades;
using UWP_JJCheckList.Models.Interfaces;

namespace UWP_JJCheckList.Models
{
    public static class OperacaoService
    {
        private static ConcurrentQueue<TarefaOperacao> tarefaOperacoes = new ConcurrentQueue<TarefaOperacao>();
        
        private static bool ExecutandoOperacoes = false;

        public static void AddOperacao(TarefaOperacao tarefaOperacao)
        {
            tarefaOperacoes.Enqueue(tarefaOperacao);

            if (!ExecutandoOperacoes)
            {
                ExecutandoOperacoes = true;
                Task.Run(() => ExecutarOperacoes());
            }
        }

        private static async Task ExecutarOperacoes()
        {
            var tarefaRepository = App.Container.GetInstance<ITarefaRepository>();

            while (tarefaOperacoes.TryDequeue(out TarefaOperacao tarefaOperacao))
            {
                try
                {
                    switch (tarefaOperacao.TipoOperacao)
                    {
                        case Controls.Helpers.TipoOperacao.Adicionar:
                            await tarefaRepository.InserirAsync(tarefaOperacao.Tarefa);
                            break;
                        case Controls.Helpers.TipoOperacao.Atualizar:
                            await tarefaRepository.AtualizarAsync(tarefaOperacao.Tarefa);
                            break;
                        case Controls.Helpers.TipoOperacao.Deletar:
                            await tarefaRepository.DeletarAsync(tarefaOperacao.Tarefa);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Aviso.Toast(ex.Message);
                }
            }

            ExecutandoOperacoes = false;
        }

        public static async Task ProcessarOperacoesPendentes()
        {
            var tarefaRepository = App.Container.GetInstance<ITarefaRepository>();

            while (tarefaOperacoes.TryDequeue(out TarefaOperacao tarefaOperacao))
            {
                try
                {
                    switch (tarefaOperacao.TipoOperacao)
                    {
                        case Controls.Helpers.TipoOperacao.Adicionar:
                            await tarefaRepository.InserirAsync(tarefaOperacao.Tarefa);
                            break;
                        case Controls.Helpers.TipoOperacao.Atualizar:
                            await tarefaRepository.AtualizarAsync(tarefaOperacao.Tarefa);
                            break;
                        case Controls.Helpers.TipoOperacao.Deletar:
                            await tarefaRepository.DeletarAsync(tarefaOperacao.Tarefa);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Aviso.Toast(ex.Message);
                }
            }
        }
    }

}
