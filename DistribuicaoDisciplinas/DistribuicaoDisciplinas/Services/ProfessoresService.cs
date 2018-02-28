//using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Services
{
    public class ProfessoresService : IProfessoresService
    {
        //private readonly IGenericRepository<ProfessorEntity> _profRep;
        //private readonly IMapper<Professor, ProfessorEntity> _profMap;

        //public ProfessoresService(IGenericRepository<ProfessorEntity> profRep,
        //    IMapper<Professor, ProfessorEntity> profMap)
        //{
        //    _profRep = profRep;
        //    _profMap = profMap;
        //}   

        //public ICollection<Professor> List()
        //{
        //    return _profMap.Map(_profRep.List());            
        //}

        private readonly IGenericRepository<Professor> _profRep;

        public ProfessoresService(IGenericRepository<Professor> profRep)
        {
            _profRep = profRep;
        }

        public ICollection<Professor> List()
        {
            return _profRep.List();
        }
    }
}