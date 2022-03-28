
namespace Data.Query
{
    public interface IPaymentQuery
    {
        Task<IEnumerable<PaymentSummary>> GetPaymentOfFrequentWorkersAsync(int days);
    }
}
