angular.module('distribuicaoApp', []);
angular.module('distribuicaoApp').controller('distribuicaoCtrl', ['$http', '$filter', '$scope', function($http, $filter, $scope) {


    var self = this;
    self.codigoCenario = 1;
    self.resposta = {}; 
    self.carregando = true;
    self.posicoes = [];
    self.profPrioridades = [];
    self.qtdaTurmasDistribuidas = 0;
    self.indexBloqueio = 0;
    self.paginasBloqueios = null;


    self.statusAlgoritmo = ['Desconsiderada', 'Atribuída', 'Nao Analisada Ainda', 'Em Espera', 'Choque Horário', 'Choque Restrição', 'Choque Período', 'Outro Professor', 'CH Completa', 'Ultrapassaria CH se Atribuída'];
    var tipoBloqueio = ['Deadlock', 'Disciplina com CH Diferente de 4 horas'];
    var url = 'http://localhost:62921/api/Testes/Distribuir/';

    var getProfessor = function(siape, professores) {
        var result = professores.filter(function(p) {
            return p.Siape === siape;
        });
        return result.length > 0 ? result[0] : null;
    }

    var getTurma = function(id, turmas) {
        var result = turmas.filter(function(t) {
            return t.Id === id;
        });
        return result.length > 0 ? result[0] : null;
    }

    var getFilaTurma = function(idTurma, idFila, filasTurmas) {
        var result = filasTurmas.filter(function(ft) {
            return ft.Fila.Id === idFila && ft.IdTurma == idTurma;
        });
        return result.length > 0 ? result[0] : null;
    } 

    var encadear = function(resposta) {
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
            var prof = getProfessor(ft.Fila.Siape, resposta.Professores);
            var turma = getTurma(ft.IdTurma, resposta.Turmas);

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
                
                bloqueio.push(getFilaTurma(aux.IdTurma, aux.IdFila, resposta.FilasTurmas));
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

        // resposta.Bloqueios.forEach(function(b) {
        //     var aux = b;
        //     var limite = 20; //Evita Loop infinito - no máximo 20 dependentes
        //     var cont = 0;
        //     var bloqueio = [];
        //     while (aux) {
        //         bloqueio.push({
        //             Professor: getProfessor(aux.Siape, resposta.Professores), 
        //             Turma: getTurma(aux.IdTurma, resposta.Turmas)
        //         });
        //         aux = aux.Dependente;
        //         if (cont === limite) break;
        //         cont++;
        //     }
        //     resposta.FilasTurmasBloqueadas.push(bloqueio);
        // });

        return resposta;
    }

    self.removerTurma = function(filaTurma) {
        filaTurma.Turma.Pendente = true;
        filaTurma.Status = 0;
    }

    self.gerarCenarios = function(codigoCenario) {
        self.carregando = true;
        $http.get(url + codigoCenario)
        .then(function(dado) {
            self.qtdaTurmasDistribuidas = dado.data.FilasTurmas.filter(function(x) {
                return x.Status == 1;
            }).length;
            self.resposta = encadear(dado.data);
            self.indexBloqueio = 0;
            self.paginasBloqueios = [];
            for(var i = 0; i < self.resposta.FilasTurmasBloqueadas.length; i++)
                self.paginasBloqueios.push(i);

            console.log(self.resposta);
        }, function(error) {
            console.log(error);
        }).finally(function() {
            self.carregando = false;
        });
    }

    self.nextBloqueio = function() {
        if ((self.indexBloqueio + 1) < self.resposta.FilasTurmasBloqueadas.length)
            self.indexBloqueio++;
    }

    self.previousBloqueio = function() {
        if (self.indexBloqueio > 0)
            self.indexBloqueio--;
    }

    self.verPrioridades = function(professor) {
    	self.profPrioridades = professor;
    	$('#modalPrioridades').modal('show');
    }

    self.verPosicoes = function(posicoes) {
    	self.posicoes = posicoes;
    	$('#modalPosicoes').modal('show');
    }


    //Drag and Drop
    $scope.dragStart = function(ev) {
        ev.dataTransfer.setData("text", $(ev.target).find('.id-turma').text());
    }

    $scope.allowDrop = function(ev) {
        ev.preventDefault();
    }

    $scope.drop = function(ev) {
        ev.preventDefault();
        var data = ev.dataTransfer.getData("text");
        console.log(data);
    }

}]);
