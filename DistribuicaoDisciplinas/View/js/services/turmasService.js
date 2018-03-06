angular.module('distribuicaoApp').service('turmasService', [function() {

	var self = this;


    self.getTurma = function(id, turmas) {
        var result = turmas.filter(function(t) {
            return t.Id == id;
        });
        return result.length > 0 ? result[0] : null;
    }

}]);