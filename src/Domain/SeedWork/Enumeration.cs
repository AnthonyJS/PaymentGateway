using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PaymentGateway.Domain.SeedWork
{
  public class Enumeration
  {
    public int Id { get; }
    public string Name { get; }
    
    public Enumeration(int id, string name)
    {
      Id = id;
      Name = name;
    }
    
    // From https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Services/Ordering/Ordering.Domain/SeedWork/Enumeration.cs
    public static T FromDisplayName<T>(string displayName) where T : Enumeration
    {
      var matchingItem = Parse<T, string>(displayName, "display name", item => item.Name == displayName);
      return matchingItem;
    }
    
    private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
    {
      var matchingItem = GetAll<T>().FirstOrDefault(predicate);

      if (matchingItem == null)
        throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

      return matchingItem;
    }
    
    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
      var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

      return fields.Select(f => f.GetValue(null)).Cast<T>();
    }
  }
}
