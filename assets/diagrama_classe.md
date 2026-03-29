---
config:
  layout: elk
  look: neo
  theme: redux
---
classDiagram
direction BT
    class Consulta {
	    +realizar() void
	    +getEspecialidade() String
    }

    class Exame {
	    +solicitar() void
	    +getTipo() String
    }

    class Receituario {
	    +emitir() void
	    +getPrescricao() String
    }

    class ConsultaGeralImpl {
	    -especialidade String
	    +realizar() void
	    +getEspecialidade() String
    }

    class ConsultaPediatriaImpl {
	    -especialidade String
	    +realizar() void
	    +getEspecialidade() String
    }

    class ConsultaOrtopediaImpl {
	    -especialidade String
	    +realizar() void
	    +getEspecialidade() String
    }

    class ConsultaFactory {
	    +criarConsulta() Consulta
	    +getDescricao() String
	    +validarHorario() boolean
    }

    class GeralFactory {
	    +criarConsulta() Consulta
	    +getDescricao() String
	    +validarHorario() boolean
    }

    class PediatriaFactory {
	    +criarConsulta() Consulta
	    +getDescricao() String
	    +validarHorario() boolean
    }

    class OrtopediaFactory {
	    +criarConsulta() Consulta
	    +getDescricao() String
	    +validarHorario() boolean
    }

    class PacoteAtendimento {
	    +criarConsulta() Consulta
	    +criarExame() Exame
	    +criarReceituario() Receituario
	    +montarPacote() void
    }

    class PacoteBasicoFactory {
	    +criarConsulta() Consulta
	    +criarExame() Exame
	    +criarReceituario() Receituario
	    +montarPacote() void
    }

    class PacoteCompletoFactory {
	    +criarConsulta() Consulta
	    +criarExame() Exame
	    +criarReceituario() Receituario
	    +montarPacote() void
    }

    class GerenciadorAgenda {
	    -instancia GerenciadorAgenda
	    -horariosDisponiveis List
	    -consultasAgendadas Map
	    +getInstancia() GerenciadorAgenda
	    +registrarConsulta(c Consulta) void
	    +cancelarConsulta(id String) void
	    +listarHorarios() List
    }

    class Paciente {
	    -id String
	    -nome String
	    -dataNascimento Date
	    -historicoConsultas List
	    +getId() String
	    +getNome() String
	    +getHistorico() List
    }

    class Medico {
	    -crm String
	    -nome String
	    -especialidade String
	    +getCrm() String
	    +getNome() String
	    +getEspecialidade() String
    }

    class Atendimento {
	    -paciente Paciente
	    -medico Medico
	    -pacote PacoteAtendimento
	    -consulta Consulta
	    +realizarAtendimento() void
	    +registrarNaAgenda() void
    }

    class ClinicaService {
	    -pacoteFactory PacoteAtendimento
	    +agendarConsulta(p Paciente, m Medico, tipo String) void
	    +cancelarConsulta(id String) void
	    +listarHorarios() List
    }

	<<interface>> Consulta
	<<interface>> Exame
	<<interface>> Receituario
	<<abstract>> ConsultaFactory
	<<abstract>> PacoteAtendimento
	<<singleton>> GerenciadorAgenda
	<<service>> ClinicaService

    ConsultaGeralImpl ..|> Consulta
    ConsultaPediatriaImpl ..|> Consulta
    ConsultaOrtopediaImpl ..|> Consulta
    GeralFactory --|> ConsultaFactory
    PediatriaFactory --|> ConsultaFactory
    OrtopediaFactory --|> ConsultaFactory
    ConsultaFactory ..> Consulta
    GeralFactory ..> Consulta
    PediatriaFactory ..> Consulta
    OrtopediaFactory ..> Consulta
    GeralFactory ..> GerenciadorAgenda
    PediatriaFactory ..> GerenciadorAgenda
    OrtopediaFactory ..> GerenciadorAgenda
    PacoteBasicoFactory --|> PacoteAtendimento
    PacoteCompletoFactory --|> PacoteAtendimento
    PacoteAtendimento ..> Consulta
    PacoteAtendimento ..> Exame
    PacoteAtendimento ..> Receituario
    PacoteBasicoFactory ..> Consulta
    PacoteBasicoFactory ..> Exame
    PacoteBasicoFactory ..> Receituario
    PacoteCompletoFactory ..> Consulta
    PacoteCompletoFactory ..> Exame
    PacoteCompletoFactory ..> Receituario
    Medico ..> GerenciadorAgenda
    Atendimento o-- Paciente
    Atendimento o-- Medico
    Atendimento ..> PacoteAtendimento
    Atendimento ..> Consulta
    Atendimento ..> GerenciadorAgenda
    ClinicaService ..> Atendimento
    ClinicaService ..> PacoteAtendimento
    ClinicaService ..> GerenciadorAgenda