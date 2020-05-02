namespace PaymentGateway.API.Contracts.V1
{
  public static class ApiRoutes
  {
    public const string Root = "api";
    public const string Version = "v1";
    public const string Base = Root + "/" + Version;

    public static class Payments
    {
      public const string Get = Base + "/payments/{Id}";
      public const string Create = Base + "/payments";
    }
  }
}

