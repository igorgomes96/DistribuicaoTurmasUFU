using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class CursoMapper : ISingleMapper<Curso, CursoEntity>
    {
        public CursoEntity Map(Curso source)
        {
            throw new NotImplementedException();
        }

        public Curso Map(CursoEntity destination)
        {
            return destination == null ? null : new Curso
            {
                Codigo = destination.codigo,
                Nome = destination.nome,
                Unidade = destination.unidade,
                Campus = destination.campus,
                PermitirChoquePeriodo = destination.permitir_choque_periodo
            };
        }
    }
}