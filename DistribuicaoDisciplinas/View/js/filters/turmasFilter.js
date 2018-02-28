angular.module('distribuicaoApp').filter('turmasFilter', function() {

	return function(turmas, filtro) {

		if (!filtro) return turmas;
		var retorno = [];
		filtro = filtro.toLowerCase();
		turmas.forEach(function(turma) {
			codigoDisc = turma.CodigoDisc.toLowerCase();
			disciplina = turma.Disciplina.Nome.toLowerCase();
			curso = turma.Disciplina.Curso.Nome.toLowerCase();
			if (codigoDisc.indexOf(filtro) > -1 || disciplina.indexOf(filtro) > -1 || curso.indexOf(filtro) > -1) 
				retorno.push(turma);
		});

		return retorno;

	};
});