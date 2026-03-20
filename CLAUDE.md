# Fintazz — Briefing do Projeto

## Visão Geral

Fintazz é uma API de gestão financeira familiar. O sistema permite que grupos de pessoas (famílias, repúblicas, casais) compartilhem e gerenciem finanças em conjunto — contas bancárias, cartões de crédito, transações e cobranças recorrentes.

## Documentação do Produto

Toda a especificação de negócio está em arquivos Markdown na pasta `/docs`. Antes de implementar qualquer funcionalidade, leia o arquivo correspondente:

| Módulo | Arquivo |
|--------|---------|
| Visão geral da stack e arquitetura | `/docs/Backend.md` |
| Grupos Familiares | `/docs/Grupos - Residência - Família.md` |
| Cadastro de Usuário e Autenticação | `/docs/Cadastro de Usuário.md` |
| Contas Bancárias | `/docs/Contas Bancária.md` |
| Cartão de Crédito | `/docs/Cartão de Crédito.md` |
| Transações | `/docs/Transação.md` |
| Transações Recorrentes | `/docs/Transações Recorrentes.md` |
| Categorias | `/docs/Categoria.md` |

## Stack

- .NET 10 / C#
- MongoDB Atlas (MongoDB.Driver 3.x)
- MediatR 14 — CQRS
- FluentValidation 12 — validação no pipeline
- Hangfire + MongoDB — jobs agendados
- Swagger (Swashbuckle) + ReDoc — documentação da API

## Projetos da Solution

```
Fintazz.Domain          → Entidades, repositórios (interfaces), domain services
Fintazz.Application     → Commands, Queries, Validators, Behaviors
Fintazz.Infrastructure  → Repositórios MongoDB, MongoContext, DependencyInjection
Fintazz.Api             → Controllers, GlobalExceptionHandler, Program.cs
Fintazz.Worker          → Jobs agendados com Hangfire
```

Regra de dependência: `Domain ← Application ← Infrastructure ← Api/Worker`

## Arquitetura e Padrões

### Fluxo de uma Requisição

```
Controller → Command/Query → ValidationBehavior → Handler → Repository → MongoDB
```

### Padrão Result

Todos os Handlers retornam `Result` ou `Result<T>` — **nunca lançam exceções para fluxos de negócio esperados**.

```csharp
// Sucesso
return Result<Guid>.Success(entity.Id);

// Falha de negócio
return Result<Guid>.Failure(new Error("HouseHold.NotFound", "HouseHold não encontrado."));
```

O Controller sempre verifica `result.IsFailure` e retorna `BadRequest` ou `Ok`:

```csharp
if (result.IsFailure)
    return BadRequest(result.Error);

return Ok(result.Value);
```

### Como criar um novo Command

1. `Fintazz.Application/{Módulo}/Commands/{NomeDoCommand}/{NomeDoCommand}.cs` — record com os dados
2. `Fintazz.Application/{Módulo}/Commands/{NomeDoCommand}/{NomeDoCommand}Validator.cs` — FluentValidation
3. `Fintazz.Application/{Módulo}/Commands/{NomeDoCommand}/{NomeDoCommand}Handler.cs` — lógica de negócio
4. Registrar o endpoint no Controller correspondente em `Fintazz.Api/Controllers/`

### Como criar uma nova Query

Mesmo padrão de pastas, mas usando `IQuery<TResponse>` e `IQueryHandler<TQuery, TResponse>` em vez de `ICommand`.

### Interfaces de Repositório

Sempre declaradas em `Fintazz.Domain/Repositories/` e implementadas em `Fintazz.Infrastructure/Repositories/`. Novos repositórios devem ser registrados no `Fintazz.Infrastructure/DependencyInjection.cs`.

### Convenções MongoDB

- Campos serializados em `camelCase`
- Enums serializados como `string`
- GUIDs no formato `Standard`
- Herança de `Transaction` mapeada via discriminador BSON — qualquer novo subtipo deve ser registrado no `MongoContext.RegisterDomainClasses()`

## Módulos Implementados

- HouseHolds — criação e listagem
- BankAccounts — criação e listagem
- CreditCards — criação, listagem, compras parceladas, estorno de compra
- Transactions — criação e listagem por período
- RecurringCharges — criação, listagem e cancelamento (soft delete)
- Dashboards — balanço mensal e fatura de cartão por mês/ano
- Worker — `ProcessRecurringChargesJob` processa recorrentes diariamente

## Módulos Pendentes

Implemente seguindo a documentação em `/docs` antes de começar:

- **Cadastro de Usuário e Autenticação JWT** — ver `/docs/Cadastro de Usuário.md`
- **BankAccounts** — edição, exclusão em cascata e pagamento de fatura (via cartão)
- **CreditCards** — edição, exclusão em cascata
- **Transactions** — exclusão e marcar como paga
- **RecurringCharges** — edição, reativação e aprovação manual
- **Categorias** — módulo completo, ver `/docs/Categoria.md`
- **HouseHolds** — edição, exclusão, membros e convites