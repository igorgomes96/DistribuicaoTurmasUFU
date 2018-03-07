angular.module('distribuicaoApp').service('cenariosService', [function() {
	var self = this;

	self.getAnosLetivos = function(cenarios) {

		var anosLetivos = [];
		// OrderBy(c => c.Ano).ThenBy(c => c.Semestre)
		cenarios.sort(function(c1, c2) { return c1.Semestre - c2.Semetre; });
		cenarios.sort(function(c1, c2) { return c1.Ano - c2.Ano; });

		var cenarioAnterior = null;
		var semestre = null;
		var ano = null;

		for (var i = 0; i < cenarios.length; i++) {
			var cenario = cenarios[i];

			if (cenarioAnterior && cenarioAnterior.Ano == cenario.Ano && cenarioAnterior.Semestre == cenario.Semestre) {
				semestre.cenarios.push(cenario);
			} else if (cenarioAnterior && cenarioAnterior.Ano == cenario.Ano) {
				ano.semestres.push({
					semestre: cenario.semestre,
					cenarios: new Array(cenario)
				});
			} else {
				anosLetivos.push({
					ano: cenario.Ano,
					semestres: [{
						semestre: cenario.Semestre,
						cenarios: new Array(cenario)
					}]
				});
			}

			cenarioAnterior = cenario;
		}

		return anosLetivos;
	}

}]);