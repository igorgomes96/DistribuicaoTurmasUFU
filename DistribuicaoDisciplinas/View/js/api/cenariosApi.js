angular.module('distribuicaoApp').service('cenariosApi', ['$http', 'configValue', function($http, configValue) {

	var self = this;
	var urlBase = configValue.baseUrl + 'Cenarios/';

	self.getCenarios = function() {
		return $http.get(urlBase);
	}


}]);