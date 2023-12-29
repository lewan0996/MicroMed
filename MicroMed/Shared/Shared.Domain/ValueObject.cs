namespace Shared.Domain;

public abstract record ValueObject
{
    protected ValueObject()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Validate();
    }

    protected abstract void Validate();

    internal static Type GetUnproxiedType(object obj)
    {
        const string efCoreProxyPrefix = "Castle.Proxies.";

        var type = obj.GetType();
        var typeString = type.ToString();

        if (typeString.Contains(efCoreProxyPrefix))
            return type.BaseType!;

        return type;
    }
}