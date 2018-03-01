angular.module('distribuicaoApp').controller('distribuicaoCtrl', ['$filter', '$scope', 'distribuicaoApi', 'statusValue', function($filter, $scope, distribuicaoApi, statusValue) {


    var self = this;
    self.codigoCenario = 1;
    self.resposta = {}; 
    self.posicoes = [];
    self.profPrioridades = [];
    self.qtdaTurmasDistribuidas = 0;
    self.indexBloqueio = 0;
    self.paginasBloqueios = null;
    self.statusAlgoritmo = statusValue;
    var profDesabilitadosAtribuicao = [];
    
    
    var tipoBloqueio = ['Deadlock', 'Disciplina com CH Diferente de 4 horas'];

    var getProfessor = function(siape, professores) {
        var result = professores.filter(function(p) {
            return p.Siape === siape;
        });
        return result.length > 0 ? result[0] : null;
    }

    var getTurma = function(id, turmas) {
        var result = turmas.filter(function(t) {
            return t.Id == id;
        });
        return result.length > 0 ? result[0] : null;
    }

    var getFilaTurma = function(idTurma, idFila, filasTurmas) {
        var result = filasTurmas.filter(function(ft) {
            return ft.Fila.Id === idFila && ft.IdTurma == idTurma;
        });
        return result.length > 0 ? result[0] : null;
    } 

    var getFilaTurmaBySiape = function(idTurma, siape, filasTurmas) {
        var result = filasTurmas.filter(function(ft) {
            return ft.Fila.Siape.trim() == siape.trim() && ft.IdTurma == idTurma;
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

        return resposta;
    }

    self.getStatusText = function(value) {
        for(var st in self.statusAlgoritmo) {
            if (self.statusAlgoritmo[st].value == value)
                return self.statusAlgoritmo[st].text;
        }
        return 'N/A';
    }


    var getFilasTurmasResposta = function(filasTurmas) {
        var result = [];
        filasTurmas.forEach(function(ft) {
            var newFt = {
                Fila: {
                    Id: ft.Fila.Id
                },
                IdTurma: ft.IdTurma,
                Status: ft.Status
            }
            result.push(newFt);
        });
        return result;
    }

    var atribuirFilaTurma = function(siape, idTurma, filasTurmas) {
        if (!filasTurmas)
            throw "FilasTurmas é nulo!";

        distribuicaoApi.postAtribuirTurma(self.codigoCenario, siape, idTurma, getFilasTurmasResposta(filasTurmas))
        .then(function(dado) {
            preparaResposta(dado.data);
        }, function(error) {
            console.log(error);
        });
    }


    self.removerTurma = function(filaTurma) {
        removerFilaTurma(filaTurma.Professor.Siape, filaTurma.Turma.Id, self.resposta.FilasTurmas);
    }

    var removerFilaTurma = function(siape, idTurma, filasTurmas) {
        if (!filasTurmas)
            throw "FilasTurmas é nulo!";

        distribuicaoApi.postRemoverTurma(self.codigoCenario, siape, idTurma, getFilasTurmasResposta(filasTurmas))
        .then(function(dado) {
            preparaResposta(dado.data);
        }, function(error) {
            console.log(error);
        });
    }

    //Ajusta a resposta recebida do servidor, gerando o encadeamento e criando as propriedades necessárias para o front.
    var preparaResposta = function(dado) {
        self.qtdaTurmasDistribuidas = dado.FilasTurmas.filter(function(x) {
            return x.Status == self.statusAlgoritmo.ATRIBUIDA.value;
        }).length;
        
        self.resposta = encadear(dado);
        self.indexBloqueio = 0;
        self.paginasBloqueios = [];
        for(var i = 0; i < self.resposta.FilasTurmasBloqueadas.length; i++)
            self.paginasBloqueios.push(i);
    }

    self.desbloqueioChange = function(filaTurma) {
        if (filaTurma.Status == self.statusAlgoritmo.ATRIBUIDA.value)
            atribuirFilaTurma(filaTurma.Professor.Siape, filaTurma.Turma.Id, self.resposta.FilasTurmas);
        else
            self.distribuir(self.resposta.FilasTurmas);
    }

    self.distribuir = function(filasTurmas) {

        var func = null;

        if (filasTurmas)
            func = distribuicaoApi.postDistribuir(self.codigoCenario, getFilasTurmasResposta(filasTurmas));
        else
            func = distribuicaoApi.getDistribuir(self.codigoCenario);
        
        func.then(function(dado) {

            preparaResposta(dado.data);

        }, function(error) {
            console.log(error);
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

    self.atribuirModalPrioridades = function(p) {
        atribuirFilaTurma(p.Professor.Siape, p.Turma.Id, self.resposta.FilasTurmas);
        $('#modalPrioridades').modal('hide');
    }

    self.atribuirModalPosicoes = function(p) {
        atribuirFilaTurma(p.Professor.Siape, p.Turma.Id, self.resposta.FilasTurmas);
        $('#modalPosicoes').modal('hide');
    }

    self.verPrioridades = function(professor) {
    	self.profPrioridades = professor;
    	$('#modalPrioridades').modal('show');
    }

    self.verPosicoes = function(posicoes) {
    	self.posicoes = posicoes;
    	$('#modalPosicoes').modal('show');
    }

    //Desabilita os professores que não tem a turma na fila de prioridades ou que 
    //estão com status diferente de EmEspera e NaoAnalisadaAinda
    var desabilitarProfessoresAtribuicao = function(turma) {
        profDesabilitadosAtribuicao = [];
        self.resposta.Professores.forEach(function(professor) {
            var filasTurmas = professor.Prioridades.filter(function(p) {
                return p.Turma.Id == turma.Id;
            });

            //Se a turma está em suas prioridades e não tem choque nem está atribuída a outro professor
            if (filasTurmas.length <= 0 
              || filasTurmas[0].Status == self.statusAlgoritmo.CHOQUE_HORARIO.value 
              || filasTurmas[0].Status == self.statusAlgoritmo.CHOQUE_RESTRICAO.value
              || filasTurmas[0].Status == self.statusAlgoritmo.CHOQUE_PERIODO.value
              || filasTurmas[0].Status == self.statusAlgoritmo.OUTRO_PROFESSOR.value) 
            {
                professor.Desabilitado = true;
                profDesabilitadosAtribuicao.push(professor);
            }

        });
    }

    var habilitarProfessoresAtribuicao = function() {
        profDesabilitadosAtribuicao.forEach(function(p) {
            p.Desabilitado = false;
        });
    }


    //Drag and Drop
    document.addEventListener("dragstart", function(ev) {
        var idTurma = $(ev.target).find('.id-turma').text();
        ev.dataTransfer.setData("text", idTurma);
        desabilitarProfessoresAtribuicao(getTurma(idTurma, self.resposta.Turmas));
        $scope.$apply();
    }, false);


    document.addEventListener("dragover", function(ev) {
        ev.preventDefault();
    });

    // document.addEventListener("dragenter", function(ev) {
    //     var itemLista = $(ev.target).closest('li');

    //     if (itemLista.hasClass('item-professor'))
    //         itemLista[0].style.background = "purple";

    // }, false);

    // document.addEventListener("dragleave", function(ev) {
    //     var itemLista = $(ev.target).closest('li');

    //     // reset background of potential drop target when the draggable element leaves it
    //     if (itemLista.hasClass('item-professor')) {
    //         itemLista[0].style.background = "";
    //     }

    // }, false);

    document.addEventListener("dragend", function(ev) {
        habilitarProfessoresAtribuicao();
        $scope.$apply();
    }, false);

    document.addEventListener("drop", function(ev) {
        ev.preventDefault();
        var idTurma = ev.dataTransfer.getData("text");
        var itemLista = $(ev.target).closest('li');

        if (!itemLista.hasClass('desabilitado')) {
            var elementsSiape = $(itemLista.find('.prof-siape'));
            if (elementsSiape.length > 0) {
                var siape = elementsSiape[0].outerText;
                //var filaTurma = getFilaTurmaBySiape(idTurma, elementsSiape[0].outerText, self.resposta.FilasTurmas);
                atribuirFilaTurma(siape, idTurma, self.resposta.FilasTurmas);
            }
        }

        //habilitarProfessoresAtribuicao();

        $scope.$apply();
    });

}]);
