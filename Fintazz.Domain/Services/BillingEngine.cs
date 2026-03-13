namespace Fintazz.Domain.Services;

using Fintazz.Domain.Entities;

public static class BillingEngine
{
    public static List<Installment> GenerateInstallments(CreditCard creditCard, decimal totalAmount, DateTime purchaseDate, int totalInstallments)
    {
        if (totalInstallments <= 0)
            throw new ArgumentException("A quantidade de parcelas deve ser maior que zero.", nameof(totalInstallments));

        var installments = new List<Installment>();
        var installmentAmount = Math.Round(totalAmount / totalInstallments, 2);
        
        // Ajuste de centavos na primeira parcela
        var firstInstallmentAmount = installmentAmount + (totalAmount - (installmentAmount * totalInstallments));

        // Regra do BillingEngine:
        // Se a compra foi feita depois (>) do dia de fechamento do cartão, a primeira fatura vai pro próximo mês.
        // Considerando que a fatura "Mês X" engloba compras feitas antes do fechamento do "Mês X".
        
        var baseBillingMonth = new DateTime(purchaseDate.Year, purchaseDate.Month, 1);
        
        if (purchaseDate.Day > creditCard.ClosingDay)
        {
            baseBillingMonth = baseBillingMonth.AddMonths(1);
        }

        for (int i = 1; i <= totalInstallments; i++)
        {
            var amount = i == 1 ? firstInstallmentAmount : installmentAmount;
            
            // O BillingMonth é incrementado a partir do mês base calculado.
            // Para a parcela 1 (i=1), será o próprio baseBillingMonth.
            var billingMonth = baseBillingMonth.AddMonths(i - 1);

            installments.Add(new Installment(Guid.NewGuid(), amount, i, billingMonth));
        }

        return installments;
    }
}
