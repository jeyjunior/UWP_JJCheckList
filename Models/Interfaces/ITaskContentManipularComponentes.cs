using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_JJCheckList.Controls;
using Windows.UI.Xaml;

namespace UWP_JJCheckList.Models.Interfaces
{
    public interface ITaskContentManipularComponentes
    {
        void MarcarCheckBox();
        void DesmarcarCheckBox();
        void DeletarItem();
        bool ObterCheckBoxStatus();
        void DefinirIndice(int indice);
        void AbrirNotepad();
    }
}
