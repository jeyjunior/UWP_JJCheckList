﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Models.Entidades;
using Windows.UI.Xaml.Media;

namespace UWP_JJCheckList.Models.Interfaces
{
    public interface ITarefaCorRepository
    {
        void InserirCoresPadrao();
        SolidColorBrush ObterCorGrupo(Tarefa clTaskContent);
    }
}
