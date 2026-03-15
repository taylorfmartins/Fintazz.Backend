namespace Fintazz.Application.Dashboards.Queries.GetCreditCardInvoice;

using Fintazz.Application.Abstractions.Messaging;
using Fintazz.Domain.Repositories;
using Fintazz.Domain.Shared;

public class GetCreditCardInvoiceQueryHandler : IQueryHandler<GetCreditCardInvoiceQuery, CreditCardInvoiceResponse>
{
    private readonly ICreditCardRepository _creditCardRepository;
    private readonly ICreditCardPurchaseRepository _purchaseRepository;

    public GetCreditCardInvoiceQueryHandler(ICreditCardRepository creditCardRepository, ICreditCardPurchaseRepository purchaseRepository)
    {
        _creditCardRepository = creditCardRepository;
        _purchaseRepository = purchaseRepository;
    }

    public async Task<Result<CreditCardInvoiceResponse>> Handle(GetCreditCardInvoiceQuery request, CancellationToken cancellationToken)
    {
        var card = await _creditCardRepository.GetByIdAsync(request.CreditCardId, cancellationToken);
        if (card is null)
        {
            return Result<CreditCardInvoiceResponse>.Failure(new Error("CreditCard.NotFound", "Cartão de Crédito não localizado."));
        }

        // Buscar todas as compras daquele cartão
        var purchases = await _purchaseRepository.GetPurchasesByCardAsync(request.CreditCardId, cancellationToken);

        var invoiceItems = new List<InvoiceItemResponse>();
        decimal totalInvoiceAmount = 0;

        // Varrer cada transação/compra
        foreach (var purchase in purchases)
        {
            // Achamos as parcelas calculadas daquela compra que caem EXATAMENTE no vencimento solicitado (Mês/Ano)
            var targetInstallment = purchase.Installments.FirstOrDefault(i => 
                i.BillingMonth.Month == request.Month && 
                i.BillingMonth.Year == request.Year);

            if (targetInstallment != null)
            {
                invoiceItems.Add(new InvoiceItemResponse(
                    PurchaseId: purchase.Id,
                    Description: purchase.Description,
                    PurchaseDate: purchase.PurchaseDate,
                    TotalPurchaseAmount: purchase.TotalAmount,
                    CurrentInstallment: targetInstallment.Number,
                    TotalInstallments: purchase.Installments.Count,
                    InstallmentAmount: targetInstallment.Amount,
                    DueDate: targetInstallment.BillingMonth,
                    IsPaid: targetInstallment.IsPaid
                ));

                totalInvoiceAmount += targetInstallment.Amount;
            }
        }

        var response = new CreditCardInvoiceResponse(
            TotalAmount: totalInvoiceAmount,
            Month: request.Month,
            Year: request.Year,
            Items: invoiceItems.OrderBy(i => i.PurchaseDate)
        );

        return Result<CreditCardInvoiceResponse>.Success(response);
    }
}
