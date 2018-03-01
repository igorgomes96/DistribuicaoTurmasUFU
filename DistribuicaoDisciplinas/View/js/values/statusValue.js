angular.module('distribuicaoApp').value('statusValue',
{
	NAO_ANALISADA_AINDA:{value:0, text:'Não Analisada Ainda'},
	ATRIBUIDA:{value:1, text:'Atribuída'},
	DESCONSIDERADA:{value:2, text:'Desconsiderada'},
	EM_ESPERA:{value:3, text:'Em Espera'},
	CHOQUE_HORARIO:{value:4, text:'Choque de Horário'},
	CHOQUE_RESTRICAO:{value:5, text:'Choque de Restrição'},
	CHOQUE_PERIODO:{value:6, text:'Choque de Período'},
	OUTRO_PROFESSOR:{value:7, text:'Atribuída a Outro Professor'},
	CH_COMPLETA:{value:8, text:'CH Completa'},
	ULTRAPASSARIA_CH:{value:9, text:'Ultrapassaria CH'}
});