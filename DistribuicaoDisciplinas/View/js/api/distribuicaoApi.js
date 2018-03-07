angular.module('distribuicaoApp').service('distribuicaoApi', ['$http', 'configValue', function($http, configValue) {

	var self = this;
	var urlBase = configValue.baseUrl + 'Distribuicao/';

	self.getDistribuir = function(codigoCenario) {
		return $http.get(urlBase + codigoCenario);
	}

	self.postDistribuir = function(codigoCenario, resposta) {
		return $http.post(urlBase + codigoCenario, resposta);
	}

	self.postAtribuirTurma = function(codigoCenario, siape, idTurma, filasTurmas) {
		return $http.post(urlBase + 'AtribuicaoManual/' + codigoCenario + '/' + siape + '/' + idTurma, filasTurmas);
	}

	self.postRemoverTurma = function(codigoCenario, siape, idTurma, filasTurmas) {
		return $http.post(urlBase + 'RemocaoManual/' + codigoCenario + '/' + siape + '/' + idTurma, filasTurmas);
	}

	self.postUltimaPrioridade = function(codigoCenario, siape, idTurma, filasTurmas) {
		return $http.post(urlBase + 'UltimaPrioridade/' + codigoCenario + '/' + siape + '/' + idTurma, filasTurmas);
	}

	self.postFinalFila = function(codigoCenario, siape, idTurma, filasTurmas) {
		return $http.post(urlBase + 'FinalFila/' + codigoCenario + '/' + siape + '/' + idTurma, filasTurmas);
	}

	self.postSalvarDistribuicao = function(filasTurmas) {
		return $http.post(urlBase + 'Salvar', filasTurmas);
	}

}]);