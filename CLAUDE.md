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

## Módulos Implementados (completos)

- **Cadastro de Usuário e Autenticação JWT** ✅ — login, registro, refresh token, confirmação de e-mail, recuperação de senha, edição de perfil, troca de senha
- **BankAccounts** ✅ — criação, listagem, edição, exclusão em cascata, pagamento de fatura via cartão
- **CreditCards** ✅ — criação, listagem, edição, exclusão em cascata, compras parceladas com categoria, edição de compra, marcar parcela como paga, fatura mensal, pagamento de fatura
- **Transactions** ✅ — criação, listagem paginada, exclusão, marcar como paga
- **RecurringCharges** ✅ — criação, listagem, edição, cancelamento, reativação, aprovação manual
- **Categorias** ✅ — CRUD completo, categorias de sistema (seed), subcategorias (1 nível), visibilidade por grupo familiar
- **HouseHolds** ✅ — criação, edição, exclusão, listagem, membros, convites, aceitar convite

## Melhorias Pendentes

- **Grupos Familiares**
	- Ao enviar e-mail com o convite do grupo para o usuário, ao clicar em entrar no grupo no link do e-mail, irá trazer o usuário para o sistema, precisamos que ele faça um cadastro se não tiver ou seja registrado no grupo caso já se autenticar.

## Melhorias Implementadas

- **Usuário / Cadastro** ✅
	- Confirmação de e-mail no cadastro — `POST /api/auth/confirm-email` e `POST /api/auth/resend-confirmation`
	- Recuperação de senha — `POST /api/auth/forgot-password` e `POST /api/auth/reset-password`
	- Edição de perfil (nome, apelido, data de nascimento) — `PUT /api/users/me`
	- Troca de senha (exige senha atual) — `PUT /api/users/me/password`

- **Categorias** ✅
	- Categorias de sistema pré-cadastradas (Alimentação, Moradia, Saúde, Transporte, etc.) — seed automático na inicialização
	- Categorias criadas pelo usuário são visíveis a todos os membros do grupo familiar
	- Resposta inclui campo `isSystem`; categorias de sistema não podem ser excluídas nem renomeadas
	- Subcategorias — campo `parentCategoryId` opcional no cadastro; mesmo tipo da pai; máximo 1 nível; resposta inclui `parentCategoryId`

- **Grupos Familiares** ✅
	- Convite não aceita o e-mail do próprio dono do grupo — erro `HouseHold.CannotInviteYourself`
	- Verificação de membros já no grupo antes de enviar convite — erro `HouseHold.AlreadyMember`

- **Compras Cartão de Crédito** ✅
	- Seleção de categoria de despesa ao registrar compra — campo `categoryId` (opcional) no `POST /api/credit-cards/purchases`
	- Edição de compra existente — `PUT /api/credit-cards/purchases/{purchaseId}` (descrição e categoria)
	- Marcar parcela individual como paga — `POST /api/credit-cards/purchases/{purchaseId}/installments/{installmentId}/pay`