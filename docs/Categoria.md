- É um cadastro para agrupar [[Transação]] e [[Transações Recorrentes]] em receitas e despesas
- As categorias pertencem a um [[Grupos - Residência - Família]], ou seja, cada grupo gerencia suas próprias categorias
- Uma categoria deverá ter os seguintes dados:
    - ID do [[Grupos - Residência - Família]]
    - Nome (ex: Alimentação, Salário, Moradia)
    - Tipo (Receita / Despesa)
    - Usuário que criou a categoria

## Regras de Negócio

- O nome da categoria deve ser único dentro do mesmo [[Grupos - Residência - Família]] e Tipo
    - Exemplo: pode existir "Alimentação" do tipo Despesa e "Alimentação" do tipo Receita, mas não dois "Alimentação" do tipo Despesa no mesmo grupo
- Uma categoria do tipo **Receita** só pode ser vinculada a transações do tipo Receita
- Uma categoria do tipo **Despesa** só pode ser vinculada a transações do tipo Despesa e a [[Transações Recorrentes]]
- Não deve ser possível excluir uma categoria que já esteja vinculada a alguma [[Transação]] ou [[Transações Recorrentes]]

## Endpoints

- `POST /api/categories` — Cadastra uma nova categoria
- `GET /api/categories/house-hold/{houseHoldId}` — Lista todas as categorias de um grupo familiar
- `GET /api/categories/{id}` — Retorna os dados de uma categoria
- `PUT /api/categories/{id}` — Renomeia uma categoria existente
- `DELETE /api/categories/{id}` — Remove uma categoria (apenas se não estiver em uso)

## Subcategorias

- Uma categoria pode ter uma **categoria pai** (campo `parentCategoryId` no cadastro)
- A subcategoria deve ter o mesmo tipo (Income/Expense) da categoria pai
- Não é permitido criar subcategorias de subcategorias (máximo 1 nível de hierarquia)
- Não é possível excluir uma categoria pai que ainda possui subcategorias
- A resposta inclui o campo `parentCategoryId` (null para categorias raiz)

## Categorias de Sistema

- Categorias comuns pré-cadastradas automaticamente na inicialização da API: `Alimentação`, `Moradia`, `Transporte`, `Saúde`, `Educação`, `Lazer`, `Vestuário`, `Assinaturas`, `Outros` (Despesa) e `Salário`, `Freelance`, `Investimentos`, `Outros` (Receita)
- Categorias de sistema têm `isSystem = true` na resposta e **não podem ser excluídas nem renomeadas**
- Categorias criadas pelo usuário pertencem ao grupo familiar e são visíveis a todos os membros do grupo

## Situação Atual no Código

- Módulo completo e operacional: criação, listagem, consulta por ID, renomeação e exclusão implementadas
- `Transaction` e `RecurringCharge` referenciam `Category` pelo `CategoryId` (Guid) — o campo `string?` livre não existe mais
- A exclusão é bloqueada se a categoria estiver em uso por alguma transação ou cobrança recorrente
- O tipo (`Income`/`Expense`) é serializado como string na API — os valores inteiros não são expostos
- Campo `isSystem` retornado na resposta — categorias de sistema são protegidas contra edição e exclusão
- Seed automático de categorias de sistema executa na inicialização da API (`SeedSystemCategoriesAsync`)