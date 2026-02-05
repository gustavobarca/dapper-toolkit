namespace Lithe;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class TransformAttribute(string expression) : Attribute
{
  public string Expression { get; } = expression;
}
