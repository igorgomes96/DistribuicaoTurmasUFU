angular.module('distribuicaoApp').service('distribuicaoService', ['professoresService', 'turmasService', 'filasTurmasService', function(professoresService, turmasService, filasTurmasService) {

	var self = this;

    var tipoBloqueio = ['Deadlock', 'Disciplina com CH Diferente de 4 horas'];


	self.encadear = function(resposta) {
        resposta.Professores.forEach(function(p) {
            if (!p.hasOwnProperty('Prioridades'))
                p.Prioridades = [];
        });

        resposta.Turmas.forEach(function(t) {
            if (!t.hasOwnProperty('Posicoes'))
                t.Posicoes = [];

            var flagPendente = resposta.TurmasPendentes.some(function(x) {
                return x === t.Id;
            });

            if (flagPendente)
                t.Pendente = true;
            else
                t.Pendente = false;
        });

        resposta.FilasTurmas.forEach(function(ft) {
            var prof = professoresService.getProfessor(ft.Fila.Siape, resposta.Professores);
            var turma = turmasService.getTurma(ft.IdTurma, resposta.Turmas);

            ft.Turma = turma;
            ft.Professor = prof;
            prof.Prioridades.push(ft);
            turma.Posicoes.push(ft);
        });

        resposta.FilasTurmasBloqueadas = [];
        resposta.Bloqueios.forEach(function(b) {
            var cabeca = b;
            var aux = b;
            var limite = 20; //Evita Loop infinito - no máximo 20 dependentes
            var cont = 0;
            var bloqueio = [];
            while (aux) {
                
                bloqueio.push(filasTurmasService.getFilaTurma(aux.IdTurma, aux.IdFila, resposta.FilasTurmas));
                aux = aux.Dependente;
                
                if (cont === limite) break;

                if (aux.IdTurma == cabeca.IdTurma && aux.IdFila == cabeca.IdFila)
                    break;

                cont++;
            }

            resposta.FilasTurmasBloqueadas.push({
                FilasTurmas: bloqueio,
                TipoBloqueio: tipoBloqueio[cabeca.TipoBloqueio],
                Tamanho: cabeca.Tamanho - 1
            });
        });

        return resposta;
    }


}]);