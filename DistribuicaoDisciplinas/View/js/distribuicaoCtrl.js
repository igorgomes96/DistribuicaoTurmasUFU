angular.module('distribuicaoApp', []);
angular.module('distribuicaoApp').controller('distribuicaoCtrl', ['$http', '$filter', function($http, $filter) {

    var self = this;
    self.codigoCenario = 1;
    self.resposta = {}; 
    self.carregando = true;
    self.posicoes = [];
    self.prioridades = [];

    self.statusAlgoritmo = ['Desconsiderada', 'Atribuída', 'Nao Analisada Ainda', 'Em Espera', 'Choque Horário', 'Choque Restrição', 'Choque Período', 'Outro Professor', 'CHCompleta'];

    var url = 'http://localhost:62921/api/Testes/Distribuir/';

    self.gerarCenarios = function(codigoCenario) {
        self.carregando = true;
        $http.get(url + codigoCenario)
        .then(function(dado) {
        	self.resposta = dado.data;
            console.log(dado);
        }, function(error) {
            console.log(error);
        }).finally(function() {
            self.carregando = false;
        })
    }

    self.verPrioridades = function(bloqueio) {
    	self.ministraSelecionado = bloqueio;
    	$('#modalPrioridades').modal('show');
    }

    self.verPosicoes = function(bloqueio) {
    	self.ministraSelecionado = bloqueio;
    	$('#modalPosicoes').modal('show');
    }

}]);
