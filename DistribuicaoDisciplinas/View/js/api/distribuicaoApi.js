angular.module('distribuicaoApp').service('distribuicaoApi', ['$http', 'configValue', function($http, configValue) {

	var self = this;
	var urlBase = configValue.baseUrl + 'Testes/Distribuir/';

	self.getDistribuir = function(codigoCenario) {
		return $http.get(urlBase + codigoCenario);
	}

	self.postDistribuir = function(codigoCenario, resposta) {
		return $http.post(urlBase + codigoCenario, resposta)
	}

	self.postAtribuirTurma = function(codigoCenario, siape, idTurma, filasTurmas) {
		return $http.post(urlBase + 'AtribuicaoManual/' + codigoCenario + '/' + siape + '/' + idTurma, filasTurmas)
	}

}]);