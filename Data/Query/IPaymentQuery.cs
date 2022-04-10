
namespace Data {
    public interface IPaymentQuery {
        Task<IEnumerable<PaymentSummary>> GetPaymentOfFrequentWorkersAsync(int days);
    }
}
