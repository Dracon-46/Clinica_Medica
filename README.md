# ClinicaPro — ASP.NET MVC + EF Core + SQLite

Sistema de agendamento de consultas médicas desenvolvido com **ASP.NET MVC (C#)**, **Entity Framework Core** e **SQLite**, seguindo os princípios **SOLID** e **POO**.

---

## 🏗 Arquitetura de Camadas

```
ClinicaMVC/
├── Domain/                         # Regras de negócio (puro C#)
│   ├── Interfaces/
│   │   ├── IConsulta.cs            # Interface da consulta
│   │   ├── IExame.cs               # Interface do exame
│   │   ├── IReceituario.cs         # Interface do receituário
│   │   └── IRepositories.cs        # Contratos dos repositórios (DIP)
│   ├── Models/
│   │   ├── Paciente.cs
│   │   ├── Medico.cs
│   │   ├── Atendimento.cs
│   │   ├── ExameReceituarioImpls.cs
│   │   └── Consultas/
│   │       └── ConsultaImpls.cs    # ConsultaGeralImpl, Pediatria, Ortopedia
│   └── Factories/
│       ├── ConsultaFactory.cs      # Factory Method (abstract + 3 concretas)
│       ├── PacoteAtendimento.cs    # Abstract Factory (Básico / Completo)
│       └── GerenciadorAgenda.cs    # Singleton thread-safe
│
├── Infrastructure/                 # Acesso a dados
│   ├── Data/
│   │   └── ClinicaDbContext.cs     # DbContext EF Core + SQLite
│   └── Repositories/
│       └── Repositories.cs         # Implementações dos repositórios
│
├── Application/
│   └── Services/
│       └── ClinicaService.cs       # Orquestração dos casos de uso
│
└── Presentation/                   # Camada MVC
    ├── Controllers/
    │   ├── HomeController.cs
    │   ├── PacienteController.cs
    │   ├── MedicoController.cs
    │   └── AgendaController.cs
    ├── Views/
    │   ├── Home/Index.cshtml       # Dashboard
    │   ├── Paciente/               # Index + Criar
    │   ├── Medico/                 # Index + Criar
    │   ├── Agenda/                 # Index + Agendar
    │   └── Shared/_Layout.cshtml
    └── wwwroot/
        ├── css/site.css
        └── js/site.js
```

---

## 🎨 Padrões de Design Implementados

| Padrão | Onde | Descrição |
|---|---|---|
| **Factory Method** | `ConsultaFactory` | Cria o tipo de consulta correto (Geral, Pediatria, Ortopedia) |
| **Abstract Factory** | `PacoteAtendimento` | Cria família de objetos: Consulta + Exame + Receituário |
| **Singleton** | `GerenciadorAgenda` | Instância única thread-safe da agenda |
| **Repository** | `IPacienteRepository` etc. | Abstrai o acesso ao banco de dados |

---

## ⚙️ Princípios SOLID

- **S** — Cada classe tem uma única responsabilidade
- **O** — Novas especialidades são adicionadas sem alterar código existente
- **L** — Implementações substituem interfaces sem quebrar comportamento
- **I** — Interfaces pequenas e coesas (`IConsulta`, `IExame`, `IReceituario`)
- **D** — Controllers e services dependem de interfaces, não de implementações concretas

---

## 🚀 Como Rodar

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download)

### Passos

```bash
# 1. Restaurar pacotes
dotnet restore

# 2. Aplicar migrations (banco SQLite criado automaticamente)
dotnet ef migrations add InitialCreate --project ClinicaMVC.csproj
dotnet ef database update

# 3. Rodar
dotnet run
```

Acesse: `https://localhost:7100`

> O banco SQLite (`clinica.db`) é criado automaticamente na raiz do projeto.
> 3 médicos já são inseridos via Seed Data na primeira execução.

---

## 📦 Pacotes NuGet

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="8.0.0" />
```
