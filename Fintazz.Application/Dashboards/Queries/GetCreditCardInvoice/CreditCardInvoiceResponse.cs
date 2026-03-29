namespace Fintazz.Application.Dashboards.Queries.GetCreditCardInvoice;

/// <summary>
/// Detalhamento completo da fatura de um cartão de crédito em um mês específico.
/// </summary>
/// <param name="TotalAmount">Valor total (soma das parcelas e compras inteiras aprovadas nesta fatura).</param>
/// <param name="Month">Mês de vencimento da fatura.</param>
/// <param name="Year">Ano de vencimento da fatura.</param>
/// <param name="Items">Lista detalhada de compras e parcelas individuais na fatura.</param>
public record CreditCardInvoiceResponse(
    decimal TotalAmount,
    int Month,
    int Year,
    IEnumerable<InvoiceItemResponse> Items);

/// <summary>
/// Representa um item (compra ou parcela) listado dentro de uma fatura de cartão.
/// </summary>
/// <param name="PurchaseId">ID raiz da compra de cartão de crédito no banco de dados.</param>
/// <param name="InstallmentId">ID da parcela individual.</param>
/// <param name="Description">Identificador da compra (ex: Pneu Carro, Mercado).</param>
/// <param name="PurchaseDate">Data oficial da compra global.</param>
/// <param name="TotalPurchaseAmount">Valor bruto e cheio original da compra de cartão, antes dos parcelamentos.</param>
/// <param name="CurrentInstallment">Número identificador desta parcela perante o total. (ex: "1" de "3").</param>
/// <param name="TotalInstallments">Número inteiro representando a quantidade final de parcelas.</param>
/// <param name="InstallmentAmount">Valor contábil exato / faturado desta parcela individual.</param>
/// <param name="DueDate">Referência exata que determina o mês/ano que a fatura vence e cobra esse item.</param>
/// <param name="IsPaid">Status de adimplência da parcela.</param>
public record InvoiceItemResponse(
    Guid PurchaseId,
    Guid InstallmentId,
    string Description,
    DateTime PurchaseDate,
    decimal TotalPurchaseAmount,
    int CurrentInstallment,
    int TotalInstallments,
    decimal InstallmentAmount,
    DateTime DueDate,
    bool IsPaid);
