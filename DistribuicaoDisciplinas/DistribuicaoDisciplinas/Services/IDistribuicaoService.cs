using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using System;
using System.Collections.Generic;



namespace DistribuicaoDisciplinas.Services
{
    public interface IDistribuicaoService
    {
        RespostaDto Distribuir(int numCenario, ICollection<FilaTurmaDto> filasTurmasDto);
        RespostaDto Atribuir(int numCenario, string siape, int turma, ICollection<FilaTurmaDto> filasTurmasDto);
        RespostaDto Remover(int numCenario, string siape, int turma, ICollection<FilaTurmaDto> filasTurmasDto);
        RespostaDto UltimaPrioridade(int numCenario, string siape, int turma, ICollection<FilaTurmaDto> filasTurmasDto);
        RespostaDto FinalFila(int numCenario, string siape, int turma, ICollection<FilaTurmaDto> filasTurmasDto);
        void SalvarDistribuicao(ICollection<FilaTurmaDto> filasTurmasDto);
    }
}

