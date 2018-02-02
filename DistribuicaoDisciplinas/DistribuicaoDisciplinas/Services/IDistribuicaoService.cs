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
    }
}

