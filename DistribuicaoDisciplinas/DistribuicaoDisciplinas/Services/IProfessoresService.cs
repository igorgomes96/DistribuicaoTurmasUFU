﻿using DistribuicaoDisciplinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistribuicaoDisciplinas.Services
{
    public interface IProfessoresService
    {
        ICollection<Professor> List();

    }
}
