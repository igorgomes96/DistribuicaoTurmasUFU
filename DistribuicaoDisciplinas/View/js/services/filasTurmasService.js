angular.module('distribuicaoApp').service('filasTurmasService', [function() {

	var self = this;

	 self.getFilaTurma = function(idTurma, idFila, filasTurmas) {
        var result = filasTurmas.filter(function(ft) {
            return ft.Fila.Id === idFila && ft.IdTurma == idTurma;
        });
        return result.length > 0 ? result[0] : null;
    }


    self.getFilasTurmasResposta = function(filasTurmas) {
        var result = [];
        filasTurmas.forEach(function(ft) {
            var newFt = {
                Fila: {
                    Id: ft.Fila.Id,
                    PosicaoReal: ft.Fila.PosicaoReal,
                    Siape: ft.Professor.Siape
                },
                IdTurma: ft.IdTurma,
                Status: ft.Status,
                PrioridadeReal: ft.PrioridadeReal
            }
            result.push(newFt);
        });
        return result;
    }

}]);