angular.module('distribuicaoApp').service('professoresService', [function() {

	var self = this;


    self.getProfessor = function(siape, professores) {
        var result = professores.filter(function(p) {
            return p.Siape === siape;
        });
        return result.length > 0 ? result[0] : null;
    }
    
}]);