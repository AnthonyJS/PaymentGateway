using AutoMapper;
using PaymentGateway.Domain.AggregatesModel.PaymentAggregate;
using PaymentGateway.Domain.SeedWork;
using PaymentGateway.Infrastructure.Persistence.PaymentHistory;

namespace PaymentGateway.API.Mapping
{
  public class PersistenceToDomainProfile : Profile
  {
    public PersistenceToDomainProfile()
    {
      ShouldMapField = fieldInfo => true;
      ShouldMapProperty = propertyInfo => true;
      
      CreateMap<PaymentDTO, CardDetails>();
      CreateMap<PaymentDTO, Payment>()
        .ForMember(dest => dest.CardDetails, opt => opt.MapFrom(src => src))
        .ForMember(dest => dest.PaymentStatus,
          opt => opt.MapFrom(src => Enumeration.FromValue<PaymentStatus>(src.PaymentStatusId)));
    }
  }
}
