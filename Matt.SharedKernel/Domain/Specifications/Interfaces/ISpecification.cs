using System.Linq.Expressions;

namespace Matt.SharedKernel.Domain.Specifications.Interfaces;

public interface IRootSpecification;

/// <summary>
/// https://stackoverflow.com/questions/63082758/ef-core-specification-pattern-add-all-column-for-sorting-data-with-custom-specif
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISpecification<T> : IRootSpecification
{
    Expression<Func<T, bool>>? Criteria { get; init; }
    List<Expression<Func<T, object>>> IncludeExpressions { get; }
    List<string> IncludeStrings { get; }

    int Take { get; }
    int Skip { get; }
    bool IsPagingEnabled { get; }
}