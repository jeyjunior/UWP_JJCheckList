using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_JJCheckList.Models.Interfaces
{
    public interface ITaskContentManipularComponentes
    {
        void MarcarCheckBox();
        void DesmarcarCheckBox();
        void DeletarItem();
        bool ObterCheckBoxStatus();
    }
}
