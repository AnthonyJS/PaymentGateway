using System.Threading.Tasks;
using PaymentGateway.Application.AcquiringBank.Models;

namespace PaymentGateway.Application.Common.Interfaces
{
  public interface IAcquiringBank
  {
    Task<AcquiringBankResponse> ProcessPayment(AcquiringBankRequest request);
  }
}