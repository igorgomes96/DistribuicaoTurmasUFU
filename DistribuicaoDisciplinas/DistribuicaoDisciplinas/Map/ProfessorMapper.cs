﻿using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class ProfessorMapper : ISingleMapper<Professor, ProfessorEntity>
    {
        private readonly IMapper<Restricao, RestricaoEntity> _restricaoMapper;

        public ProfessorMapper(IMapper<Restricao, RestricaoEntity> restricaoMapper)
        {
            _restricaoMapper = restricaoMapper;
        }

        public ProfessorEntity Map(Professor source)
        {
            throw new NotImplementedException();
        }

        public Professor Map(ProfessorEntity destination)
        {
            return destination == null ? null : new Professor
            {
                Siape = destination.siape,
                Nome = destination.nome,
                DataIngresso = destination.data_ingresso.Value,
                CH = destination.carga_atual.Value,
                Restricoes = _restricaoMapper.Map(destination.Restricoes)
            };
        }
    }
}