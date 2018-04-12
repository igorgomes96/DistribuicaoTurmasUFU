using DistribuicaoDisciplinas.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistribuicaoDisciplinas.Repository
{
    public interface IMinistraRepository: IGenericRepository<MinistraEntity>
    {
        void DeleteTurmasComFilaBySemestre(int ano, int semestre);
        void SalvarDistribuicao(ICollection<MinistraEntity> distribuicao);
    }
}
