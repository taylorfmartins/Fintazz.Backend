namespace Fintazz.Application.Dashboards.Queries.GetCreditCardInvoice;

public record CreditCardInvoiceResponse(
    decimal TotalAmount,
    int Month,
    int Year,
    IEnumerable<InvoiceItemResponse> Items);

public record InvoiceItemResponse(
    Guid PurchaseId,
    string Description,
    DateTime PurchaseDate,
    decimal TotalPurchaseAmount,
    int CurrentInstallment,
    int TotalInstallments,
    decimal InstallmentAmount,
    DateTime DueDate,
    bool IsPaid);
