O módulo gerencia o ciclo de vida do usuário — cadastro, autenticação via JWT e consulta de perfil. É o pré-requisito de todos os outros módulos, pois todas as rotas do sistema exigem um token válido.

## Entidade User

| Campo | Tipo | Descrição |
|---|---|---|
| `Id` | `Guid` | Identificador único gerado no cadastro |
| `FullName` | `string` | Nome completo |
| `NickName` | `string` | Apelido exibido na interface |
| `Email` | `string` | E-mail único — utilizado para login |
| `BirthDate` | `DateOnly` | Data de nascimento |
| `PasswordHash` | `string` | Hash BCrypt da senha — nunca exposto na API |
| `RefreshToken` | `string?` | Token de renovação ativo |
| `RefreshTokenExpiresAt` | `DateTime?` | Expiração do RefreshToken (UTC) |

## Regras de Negócio

- O e-mail é único no sistema — tentativa de cadastro com e-mail já existente retorna erro `User.EmailAlreadyInUse`
- A senha é armazenada com hash BCrypt — nunca salva em texto plano
- O `PasswordHash`, `RefreshToken` e `RefreshTokenExpiresAt` **nunca são retornados pela API**
- Ao fazer login, um novo `RefreshToken` é gerado e o anterior é invalidado automaticamente
- Ao renovar o token, um novo par (`AccessToken` + `RefreshToken`) é gerado — o `RefreshToken` anterior é descartado

## Autenticação JWT

O sistema utiliza **JWT (JSON Web Token)** stateless com dois tokens:

| Token | Duração | Uso |
|---|---|---|
| `AccessToken` | 15 minutos | Enviado em todas as requisições via `Authorization: Bearer {token}` |
| `RefreshToken` | 7 dias | Usado exclusivamente para renovar o `AccessToken` sem novo login |

### Claims do AccessToken

| Claim | Valor |
|---|---|
| `sub` | `user.Id` (Guid do usuário) — usado internamente como `CurrentUserId` |
| `email` | E-mail do usuário |
| `name` | Apelido (`NickName`) do usuário |
| `jti` | UUID único por token (permite rastreabilidade) |

> O `CurrentUserId` em todos os controllers é extraído do claim `sub` do JWT via `BaseApiController`.

## Endpoints

### Cadastro e Autenticação

- `POST /api/auth/register` — Cadastra um novo usuário
- `POST /api/auth/login` — Autentica e retorna o par de tokens JWT
- `POST /api/auth/refresh` — Renova o `AccessToken` usando um `RefreshToken` válido

### Perfil

- `GET /api/users/me` — Retorna os dados do usuário autenticado (exige `Authorization: Bearer`)

## Fluxo de Cadastro (`POST /api/auth/register`)

**Request:**
```json
{
  "fullName": "João da Silva",
  "nickName": "João",
  "email": "joao@email.com",
  "birthDate": "1990-05-15",
  "password": "minhasenha123"
}
```

**Response 200:**
```json
{ "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6" }
```

**Erros:**
| Código | Descrição |
|---|---|
| `User.EmailAlreadyInUse` | E-mail já cadastrado no sistema |
| Validação | FullName, NickName, Email e Password são obrigatórios; Password mínimo 8 caracteres |

## Fluxo de Login (`POST /api/auth/login`)

**Request:**
```json
{
  "email": "joao@email.com",
  "password": "minhasenha123"
}
```

**Response 200:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64encodedtoken...",
  "accessTokenExpiresAt": "2026-03-23T14:30:00Z"
}
```

**Erros:**
| Código | Descrição |
|---|---|
| `Auth.InvalidCredentials` | E-mail ou senha incorretos (resposta propositalmente genérica) |

## Fluxo de Renovação (`POST /api/auth/refresh`)

**Request:**
```json
{
  "refreshToken": "base64encodedtoken..."
}
```

**Response 200:** mesmo schema do Login — novo par de tokens.

**Erros:**
| Código | Descrição |
|---|---|
| `Auth.InvalidRefreshToken` | RefreshToken não encontrado no banco |
| `Auth.RefreshTokenExpired` | RefreshToken expirado — usuário deve refazer o login |

## Perfil do Usuário (`GET /api/users/me`)

Retorna os dados do usuário autenticado sem campos sensíveis.

**Response 200:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "fullName": "João da Silva",
  "nickName": "João",
  "email": "joao@email.com",
  "birthDate": "1990-05-15"
}
```

## Convites para Grupos Familiares

O fluxo de convite é gerenciado pelo módulo [[Grupos - Residência - Família]], mas depende de usuário autenticado:

1. Administrador envia convite via `POST /api/house-holds/{id}/invites` com o e-mail do convidado
2. O sistema valida que o e-mail pertence a um usuário cadastrado
3. Um token de convite é gerado com validade de **72 horas**
4. O convidado autentica-se e acessa `POST /api/house-holds/invites/{token}/accept`
5. O sistema valida que o e-mail do token coincide com o e-mail do usuário autenticado

## Autorização

- Todas as rotas do sistema (exceto `/api/auth/register`, `/api/auth/login` e `/api/auth/refresh`) exigem `Authorization: Bearer {accessToken}`
- O middleware `[Authorize]` valida o token e rejeita com `401 Unauthorized` se ausente ou inválido
- O `CurrentUserId` é extraído do claim `sub` — se o claim não for um Guid válido, retorna `Guid.Empty`

## Situação Atual no Código

- Módulo completo e operacional: cadastro, login, renovação de token e consulta de perfil implementados
- O `JwtService` gera tokens com `HmacSha256` e os claims `sub`, `email`, `name` e `jti`
- As configurações de JWT (`SecretKey`, `Issuer`, `Audience`, expiração) ficam em `appsettings.json` sob a seção `JwtSettings`
- O `RefreshTokenExpirationDays` está fixo em 7 dias no handler — o campo `JwtSettings.RefreshTokenExpirationDays` existe mas não é consumido pelo handler atualmente
