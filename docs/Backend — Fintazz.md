## Stack

- **Plataforma:** .NET 10
- **Linguagem:** C#
- **Banco de Dados:** MongoDB Atlas
- **Driver MongoDB:** MongoDB.Driver 3.x
- **Mediator:** MediatR 14
- **Validação:** FluentValidation 12
- **Jobs Agendados:** Hangfire com storage no MongoDB
- **Documentação da API:** Swagger (Swashbuckle) + ReDoc

## Projetos da Solution

A solution `Fintazz.Backend.slnx` é dividida em quatro projetos seguindo os princípios da Clean Architecture:

- **Fintazz.Domain** — Entidades, interfaces de repositório e domain services. Não possui dependência de nenhum outro projeto
- **Fintazz.Application** — Casos de uso (Commands e Queries via CQRS), validators e behaviors do pipeline. Depende apenas do Domain
- **Fintazz.Infrastructure** — Implementação dos repositórios com MongoDB e configuração do contexto. Depende do Domain e Application
- **Fintazz.Api** — Controllers REST, tratamento global de exceções e configuração do Swagger. Depende do Application e Infrastructure
- **Fintazz.Worker** — Background service com Hangfire responsável pelos jobs agendados. Depende do Application e Infrastructure

## Arquitetura

O projeto segue **Clean Architecture** combinada com o padrão **CQRS** (Command Query Responsibility Segregation) via MediatR.

### Fluxo de uma Requisição

```
Controller → Command/Query → ValidationBehavior (pipeline) → Handler → Repository → MongoDB
```

1. O Controller recebe a requisição e monta o Command ou Query
2. O MediatR intercepta via `ValidationBehavior` e executa todos os validators do FluentValidation antes de chegar no Handler
3. Se houver falha de validação, uma `ValidationException` é lançada e capturada pelo `GlobalExceptionHandler`
4. O Handler executa a regra de negócio, utiliza os repositórios e retorna um `Result<T>`
5. O Controller verifica `result.IsFailure` e retorna `BadRequest` ou `Ok` adequadamente

### Padrão Result

Todos os Handlers retornam `Result` ou `Result<T>` — nunca lançam exceções para fluxos esperados de negócio. Erros de negócio são representados pela classe `Error(Code, Message)`.

```
Result.Success()
Result.Failure(new Error("HouseHold.NotFound", "HouseHold não encontrado."))
Result<Guid>.Success(entity.Id)
Result<Guid>.Failure(new Error("CreditCard.InsufficientLimit", "Limite insuficiente."))
```

### Tratamento de Erros

O `GlobalExceptionHandler` centraliza o tratamento de todas as exceções não tratadas:

- `BadHttpRequestException` → 400 com mensagem de formato inválido
- `ValidationException` (FluentValidation) → 400 com lista de campos e mensagens
- Qualquer outra exceção → 500 com mensagem genérica

## Banco de Dados

- **MongoDB Atlas** em produção, **MongoDB local** em desenvolvimento
- A conexão é configurada via `MongoDbSettings` no `appsettings.json`
- Em produção utiliza o formato `mongodb+srv://` com autenticação; em desenvolvimento usa `mongodb://localhost`
- Convenções globais configuradas no `MongoContext`:
    - Nomes de campos em `camelCase`
    - Enums serializados como `string`
    - Campos desconhecidos ignorados na desserialização
    - GUIDs no formato `Standard` (UUID)
- Herança de `Transaction` (IncomeTransaction, ExpenseTransaction, SubscriptionTransaction) é mapeada via discriminador do BSON

## Coleções do MongoDB

|Coleção|Entidade|Descrição|
|---|---|---|
|`HouseHolds`|`HouseHold`|Grupos familiares|
|`BankAccounts`|`BankAccount`|Contas bancárias|
|`Transactions`|`Transaction`|Lançamentos de receita e despesa|
|`CreditCards`|`CreditCard`|Cartões de crédito|
|`CreditCardPurchases`|`CreditCardPurchase`|Compras e parcelamentos|
|`RecurringCharges`|`RecurringCharge`|Cobranças recorrentes|
|`Establishments`|`Establishment`|Estabelecimentos comerciais (CNPJ)|
|`Products`|`Product`|Produtos identificados por EAN|
|`FintazzHangfire`|—|Banco separado para metadados do Hangfire|

## Domain Services

### BillingEngine

Responsável por calcular e distribuir as parcelas de uma compra no cartão de crédito.

- Regra de fechamento: se a compra for feita **após** o `ClosingDay` do cartão, a primeira parcela cai na fatura do **mês seguinte**
- O ajuste de centavos é aplicado na primeira parcela para garantir que a soma das parcelas seja exatamente igual ao valor total

### BalanceEngine

Responsável por aplicar o impacto de uma transação no saldo da conta bancária.

- Só altera o saldo se `IsPaid = true`
- `IncomeTransaction` soma ao saldo
- `ExpenseTransaction` subtrai do saldo

## Worker e Jobs Agendados

O `Fintazz.Worker` é um projeto separado que roda como background service usando **Hangfire** com storage no MongoDB.

### ProcessRecurringChargesJob

- Executa **todo dia à meia-noite e 1 minuto** (UTC)
- Busca todas as `RecurringCharge` ativas cujo `BillingDay` é igual ao dia atual
- Para cobranças com `AutoApprove = false`, apenas loga e pula — aguarda aprovação manual
- Para cobranças com `AutoApprove = true`:
    - Se vinculada a conta bancária → cria uma `ExpenseTransaction` com `IsPaid = true`
    - Se vinculada a cartão de crédito → cria uma `CreditCardPurchase` com 1 parcela

## Módulos Implementados

- [[Cadastro de Usuário e Autenticação]] — Registro, login, refresh de token (JWT), perfil do usuário (`GET /api/users/me`)
- [[Grupos - Residência - Família]] — CRUD completo, controle de Administrador, membros e fluxo de convites por e-mail
- [[Contas Bancária]] — CRUD completo com saldo, edição, exclusão em cascata
- [[Cartão de Crédito]] — CRUD completo, compras parceladas, estorno, pagamento de fatura, fatura por mês/ano
- [[Transação]] — Lançamento de receitas e despesas, exclusão, marcar como paga, extrato paginado
- [[Transações Recorrentes]] — Cadastro, edição, cancelamento (soft delete), reativação, aprovação manual
- [[Categoria]] — CRUD completo por grupo familiar, validação de uso antes de excluir
- **Dashboards** — Balanço mensal consolidado e detalhamento de fatura por cartão

## Módulos Pendentes

- **Marcar parcela como paga** — Endpoint para dar baixa em uma parcela de cartão individualmente