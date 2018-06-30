using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    public class Curso
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Unidade { get; set; }
        public string Campus { get; set; }
        public bool PermitirChoquePeriodo { get; set; }
        public bool PermitirChoqueHorario { get; set; }
    }
}