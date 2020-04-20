using Xunit;

namespace PaymentGateway.API.Integration.Tests
{
  public static class CollectionName
  {
    public const string PaymentTestCollection = "Payment Test Collection";
  }

  public class CollectionDefinition
  {
    [CollectionDefinition(CollectionName.PaymentTestCollection)]
    public class GameStateCollection : ICollectionFixture<PaymentTestFixture> { }
  }
}
