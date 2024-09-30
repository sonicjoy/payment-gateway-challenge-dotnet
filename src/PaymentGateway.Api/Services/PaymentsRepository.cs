using PaymentGateway.Api.Models.PaymentService;

namespace PaymentGateway.Api.Services;

public interface IPaymentsRepository
{
    Task Add(PaymentEntity payment);
    Task<PaymentEntity?> Get(Guid id);
}

public class PaymentsRepository : IPaymentsRepository
{
    public List<PaymentEntity> Payments = [];
    
    public async Task Add(PaymentEntity payment)
    {
        payment.SetId(Guid.NewGuid());
        Payments.Add(payment);
    }

    public async Task<PaymentEntity?> Get(Guid id)
    {
        return Payments.FirstOrDefault(p => p.Id == id);
    }

}