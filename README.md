# ServiceOrder — Projeto de Estudo em DDD & Clean Architecture

> Projeto pessoal de estudo construído para **praticar Domain-Driven Design (DDD)**, **Clean Architecture**, **mensageria com RabbitMQ** e **integração com serviços externos** (envio de e-mail via Resend) em .NET 8.
>
> O domínio escolhido é simples de propósito: **gestão de Ordens de Serviço (OS)** atribuídas a técnicos de campo. O foco do projeto **não é a complexidade do negócio**, e sim exercitar **modelagem rica, separação de camadas, comunicação assíncrona e boas práticas arquiteturais**.

---

## Objetivos do projeto

- Praticar **DDD tático**: entidades, value objects, agregados, invariantes, repositórios e eventos de domínio.
- Aplicar **Clean Architecture**: regras de dependência apontando para o domínio, camadas isoladas e testáveis.
- Exercitar **comunicação assíncrona entre serviços** com RabbitMQ.
- Integrar com um **serviço externo real** (Resend) a partir de um worker desacoplado.
- Tomar **decisões arquiteturais conscientes** — e documentar os trade-offs.

> Este é um projeto **didático**. Algumas decisões foram tomadas para *exercitar* um padrão específico, mesmo quando uma abordagem mais simples bastaria.

---

## Domínio

A aplicação modela **Ordens de Serviço (Service Orders)**:

- Uma **OS** é aberta com endereço, descrição e um tipo de serviço (`Normal` ou `Urgente`).
- Pode ser criada sem técnico (status `Pending`).
- Quando um técnico é atribuído e a OS é iniciada, ela passa para `InProgress` e dispara uma notificação.
- Após concluída, vai para `Completed`.

### Conceitos modelados

| Conceito | Tipo | Observação |
|---|---|---|
| `ServiceOrder` | Entidade / Aggregate Root | Encapsula transições de estado (`AssignTechnician`, `SetInitialized`, `SetCompleted`). |
| `Technician` | Entidade | Representa o profissional responsável. |
| `ServiceOrderAddress` | Value Object | Estruturado (`Street`, `Number`, `City`, …). |
| `ServiceType` | Value Object | Com instâncias estáticas (`Normal`, `Urgent`) — *type-as-data*. |
| `StatusEnum` | Enum | `Pending`, `InProgress`, `Completed`. |
| `ServiceOrderInitialized` | Evento de integração | Publicado no RabbitMQ ao iniciar a OS. |

---

## Arquitetura

Implementa **Clean Architecture** com 4 camadas físicas (projetos) + 1 worker.

**Regra da dependência:** tudo aponta para o `Core`. O `Core` não conhece ninguém.

---

## Stack & dependências

- **.NET 8** (Web API + Worker Service)
- **Entity Framework Core** — persistência (`OwnsOne` para Value Objects)
- **FluentValidation** — validação de entidades e VOs
- **RabbitMQ.Client** — mensageria assíncrona entre API e Worker
- **Resend** — envio transacional de e-mails
- **Scalar / Swashbuckle** — documentação OpenAPI

---

## Padrões aplicados

- **Aggregate Root** (`ServiceOrder`) com setters privados e métodos de comportamento.
- **Value Objects imutáveis** com `record` (`ServiceType`, `ServiceOrderAddress`).
- **Repository Pattern** com contratos no domínio.
- **Notification Pattern** (`NotificationContext`) para coletar erros de validação sem usar exceções.
- **Domain/Integration Events** + **Publisher/Subscriber** via RabbitMQ.
- **Composition Root** em `ApplicationModule.cs` e na Api.
- **BackgroundService** com classe base genérica reutilizável (`RabbitMqConsumerBase<T>`).

---

## Trade-offs e decisões de design

Aqui está o coração do projeto — onde algo foi feito **de propósito** para estudo, mesmo sabendo que existem alternativas. Cada item lista a **decisão**, o **motivo** e o **custo**.

### 1. FluentValidation **dentro** da entidade
- **Decisão:** `EntityBase` chama `Validate(this, validator)` no construtor.
- **Por quê:** queria experimentar o **Notification Pattern** sem lançar exceção de domínio.
- **Custo:** o domínio fica acoplado a uma biblioteca externa, e propriedades como `Valid`, `Invalid`, `ValidationResult` vazam para a infraestrutura (vide os `Ignore(...)` no `OnModelCreating`). Em DDD “ortodoxo” a entidade nunca poderia existir em estado inválido — invariantes seriam protegidas por exceções no construtor.

### 2. Application Service vs CQRS/MediatR
- **Decisão:** usei um `IServiceOrderService` único com vários métodos (`AddAsync`, `InitializeAsync`, etc.).
- **Por quê:** quis manter o pipeline simples para focar em DDD tático antes de adicionar CQRS.
- **Custo:** o serviço acumula responsabilidades; cada novo caso de uso engorda a mesma classe.
- **Próximo passo natural:** quebrar em `Command/Query` + `Handlers` com **MediatR**, e usar `ValidationBehavior` no pipeline.

### 3. Notification Pattern em vez de Result/Exception
- **Decisão:** `NotificationContext` (scoped) acumula notificações e a API responde 400 a partir dele.
- **Por quê:** queria experimentar essa abordagem popular.
- **Custo:** controle de fluxo implícito — quem chama precisa lembrar de checar o contexto antes de prosseguir.

### 4. Publicação de evento **na Application**, não no domínio
- **Decisão:** `ServiceOrderService.InitializeAsync` publica `ServiceOrderInitialized` direto no Rabbit após salvar.
- **Por quê:** mais simples de seguir e debugar em um projeto de estudo.
- **Custo:** o domínio não “diz” que algo aconteceu — quem decide é a camada de aplicação. Perde-se parte da expressividade DDD.
- **Caminho ideal:** entidade levanta `IDomainEvent`, um *dispatcher* publica após `SaveChanges`, e um handler na Application traduz para um **Integration Event** publicado no Rabbit.

### 5. Dois `DbContext` separados (`ServiceOrderDatabaseContext` e `TechnicianDatabaseContext`)
- **Decisão:** simular **bounded contexts distintos** com seus próprios contextos de persistência.
- **Por quê:** treinar o conceito de separação de contextos.
- **Custo:** consistência transacional entre os dois exige cuidado (não há `SaveChanges` atômico cruzado). Para o porte atual do domínio, um único contexto bastaria.

### 6. Repositórios expõem `SaveChangesAsync`
- **Decisão:** simplicidade — cada caso de uso chama `repo.SaveChangesAsync()` ao final.
- **Custo:** vaza a noção de transação do EF Core e mistura responsabilidades.
- **Alternativa:** introduzir um `IUnitOfWork` próprio.

### 7. Worker como projeto separado, sem referenciar a API
- **Decisão:** o `Notification.Worker` é independente e se comunica apenas via RabbitMQ.
- **Por quê:** simular um **microserviço** real com fronteira clara.
- **Custo:** duplicação dos contratos de evento entre publisher e consumer (poderia ser um pacote `Contracts` compartilhado).

---

##  Como executar

### Pré-requisitos

- .NET 8 SDK
- Docker (para subir RabbitMQ e banco)

### 1. Subir infraestrutura
docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management

### 4. Rodar o Worker
