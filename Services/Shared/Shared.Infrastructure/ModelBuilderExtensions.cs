using System.Linq.Expressions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain;

namespace Shared.Infrastructure;

public static class ModelBuilderExtensions
{
    public static ComplexTypePropertyBuilder<TStringValueObject> HasStringValueObject<TEntity, TStringValueObject>(
        this ComplexPropertyBuilder<TEntity> builder,
        Expression<Func<TEntity, TStringValueObject>> navigationExpression,
        string? columnName = null) where TStringValueObject : StringValueObject
    {
        var ctor = typeof(TStringValueObject).GetConstructor(new[] { typeof(string) })!;

        return builder
            .Property(navigationExpression)
            .HasConversion(x => x.Value, x => (TStringValueObject)ctor.Invoke(new object[] { x }))
            .HasColumnName(columnName ?? GetExpressionMemberName(navigationExpression));
    }

    public static PropertyBuilder<TStringValueObject> HasStringValueObject<TEntity, TStringValueObject>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TStringValueObject>> navigationExpression,
        string? columnName = null) where TStringValueObject : StringValueObject where TEntity: class
    {
        var ctor = typeof(TStringValueObject).GetConstructor(new[] { typeof(string) })!;

        return builder
            .Property(navigationExpression)
            .HasConversion(x => x.Value, x => (TStringValueObject)ctor.Invoke(new object[] { x }))
            .HasColumnName(columnName ?? GetExpressionMemberName(navigationExpression));
    }

    private static string GetExpressionMemberName<TEntity, TStringValueObject>(Expression<Func<TEntity, TStringValueObject>> expression)
    {
        return ((MemberExpression)expression.Body).Member.Name;
    }

    public static ModelBuilder AddMassTransitOutbox(this ModelBuilder builder)
    {
        builder.AddInboxStateEntity();
        builder.AddOutboxMessageEntity();
        builder.AddOutboxStateEntity();

        return builder;
    }
}