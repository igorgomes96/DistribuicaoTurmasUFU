angular.module('distribuicaoApp').controller('distribuicaoCtrl', ['$filter', '$scope', 'distribuicaoApi', 'statusValue', 'distribuicaoService', 'turmasService', 'filasTurmasService', function($filter, $scope, distribuicaoApi, statusValue, distribuicaoService, turmasService, filasTurmasService) {


    var self = this;
    self.codigoCenario = 1;
    self.resposta = {}; 
    self.posicoes = [];
    self.profPrioridades = [];
    self.qtdaTurmasDistribuidas = 0;
    self.indexBloqueio = 0;
    self.paginasBloqueios = null;
    self.statusAlgoritmo = statusValue;
    self.loading = false;
    var profDesabilitadosAtribuicao = [];


    self.getStatusText = function(value) {
        for(var st in self.statusAlgoritmo) {
            if (self.statusAlgoritmo[st].value == value)
                return self.statusAlgoritmo[st].text;
        }
        return 'N/A';
    }

    var atribuirFilaTurma = function(siape, idTurma, filasTurmas) {
        self.loading = true;
        if (!filasTurmas)
            throw "FilasTurmas é nulo!";

        distribuicaoApi.postAtribuirTurma(self.codigoCenario, siape, idTurma, filasTurmasService.getFilasTurmasResposta(filasTurmas))
        .then(function(dado) {
            preparaResposta(dado.data);
        }, function(error) {
            console.log(error);
        }).finally(function() {
            self.loading = false;
        });
    }


    self.removerTurma = function(filaTurma) {
        removerFilaTurma(filaTurma.Professor.Siape, filaTurma.Turma.Id, self.resposta.FilasTurmas);
    }

    self.turmaJaAtribuida = function(posicoes) {
        return posicoes.some(function(p) {
            return p.Status == self.statusAlgoritmo.ATRIBUIDA.value;
        });
    }

    self.salvarDistribuicao = function() {
        self.loading = true;
        distribuicaoApi.postSalvarDistribuicao(filasTurmasService.getFilasTurmasResposta(self.resposta.FilasTurmas))
        .then(function(dado) {
            self.loading = false;
            swal("Salvo!", "Distribuição salva com sucesso!", "success");
        }, function(error) {
            console.log(error);
        }).finally(function() {
            self.loading = false;
        });
    }

    var removerFilaTurma = function(siape, idTurma, filasTurmas) {
        self.loading = true;
        if (!filasTurmas)
            throw "FilasTurmas é nulo!";

        distribuicaoApi.postRemoverTurma(self.codigoCenario, siape, idTurma, filasTurmasService.getFilasTurmasResposta(filasTurmas))
        .then(function(dado) {
            preparaResposta(dado.data);
        }, function(error) {
            console.log(error);
        }).finally(function() {
            self.loading = false;
        });
    }

    //Ajusta a resposta recebida do servidor, gerando o encadeamento e criando as propriedades necessárias para o front.
    var preparaResposta = function(dado) {
        self.qtdaTurmasDistribuidas = dado.FilasTurmas.filter(function(x) {
            return x.Status == self.statusAlgoritmo.ATRIBUIDA.value;
        }).length;
        
        self.resposta = distribuicaoService.encadear(dado);
        self.indexBloqueio = 0;
        self.paginasBloqueios = [];
        for(var i = 0; i < self.resposta.FilasTurmasBloqueadas.length; i++)
            self.paginasBloqueios.push(i);
    }

    self.desbloqueioChange = function(filaTurma) {
        if (filaTurma.StatusDesbloqueio == self.statusAlgoritmo.ATRIBUIDA.value)
            atribuirFilaTurma(filaTurma.Professor.Siape, filaTurma.Turma.Id, self.resposta.FilasTurmas);
        else if (filaTurma.StatusDesbloqueio == self.statusAlgoritmo.DESCONSIDERADA.value)
            self.removerTurma(filaTurma);
        else if (filaTurma.StatusDesbloqueio == self.statusAlgoritmo.ULTIMA_PRIORIDADE.value)
            jogarUltimaPrioridade(filaTurma);
        else if (filaTurma.StatusDesbloqueio == self.statusAlgoritmo.FINAL_FILA.value)
            jogarUltimaPrioridade(filaTurma);

    }

    var jogarFinalFila = function(filaTurma) {
        self.loading = true;
        distribuicaoApi.postFinalFila(self.codigoCenario, filaTurma.Fila.Siape, filaTurma.Turma.Id, filasTurmasService.getFilasTurmasResposta(self.resposta.FilasTurmas))
        .then(function(dado) {
            preparaResposta(dado.data);
        }, function(error) {
            console.log(error);
        }).finally(function() {
            self.loading = false;
        });
    }

    var jogarUltimaPrioridade = function(filaTurma) {
        self.loading = true;
        distribuicaoApi.postUltimaPrioridade(self.codigoCenario, filaTurma.Fila.Siape, filaTurma.Turma.Id, filasTurmasService.getFilasTurmasResposta(self.resposta.FilasTurmas))
        .then(function(dado) {
            preparaResposta(dado.data);
        }, function(error) {
            console.log(error);
        }).finally(function() {
            self.loading = false;
        });
    }

    self.distribuir = function(filasTurmas) {

        self.loading = true;
        var func = null;

        if (filasTurmas)
            func = distribuicaoApi.postDistribuir(self.codigoCenario, filasTurmasService.getFilasTurmasResposta(filasTurmas));
        else
            func = distribuicaoApi.getDistribuir(self.codigoCenario);
        
        func.then(function(dado) {

            preparaResposta(dado.data);

        }, function(error) {
            console.log(error);
        }).finally(function() {
            self.loading = false;
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
        desabilitarProfessoresAtribuicao(turmasService.getTurma(idTurma, self.resposta.Turmas));
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
                atribuirFilaTurma(siape, idTurma, self.resposta.FilasTurmas);
            }
        }

        //habilitarProfessoresAtribuicao();

        $scope.$apply();
    });

}]);
