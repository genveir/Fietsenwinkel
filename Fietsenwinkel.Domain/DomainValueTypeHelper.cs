using Fietsenwinkel.Domain.Errors;
using Fietsenwinkel.Shared.Results;
using System.Collections.Generic;
using System.Linq;

namespace Fietsenwinkel.Domain;

internal static class DomainValueTypeHelper
{
    internal static Result<TImplementationType[], ErrorCodeSet> CreateManyRecursive<TValueType, TImplementationType>(
        IEnumerable<TValueType> values)
        where TImplementationType : IDomainValueType<TValueType, TImplementationType> =>
            CreateManyRecursive<TValueType, TImplementationType>(values.ToArray(), 0, [], []);

    private static Result<TImplementationType[], ErrorCodeSet> CreateManyRecursive<TValueType, TImplementationType>(
        TValueType[] values,
        int index,
        List<TImplementationType> implementations,
        List<ErrorCodes> errors)
        where TImplementationType : IDomainValueType<TValueType, TImplementationType>
    {
        if (index == values.Length)
        {
            return errors.Count == 0 ?
                Result<TImplementationType[], ErrorCodeSet>.Succeed([.. implementations]) :
                Result<TImplementationType[], ErrorCodeSet>.Fail(new ErrorCodeSet(errors));
        }

        return TImplementationType.Create(values[index])
            .Switch(
                onSuccess: v => CreateManyRecursive(values, index + 1, implementations.Append(v).ToList(), errors),
                onFailure: v => CreateManyRecursive(values, index + 1, implementations, errors.Union(v).ToList()));
    }

    internal static Result<TImplementationType[], ErrorCodeSet> ParseManyRecursive<TValueType, TImplementationType>(
        IEnumerable<string> values)
        where TImplementationType : IDomainValueType<TValueType, TImplementationType> =>
            ParseManyRecursive<TValueType, TImplementationType>(values.ToArray(), 0, [], []);

    private static Result<TImplementationType[], ErrorCodeSet> ParseManyRecursive<TValueType, TImplementationType>(
        string[] values,
        int index,
        List<TImplementationType> implementations,
        List<ErrorCodes> errors)
        where TImplementationType : IDomainValueType<TValueType, TImplementationType>
    {
        if (index == values.Length)
        {
            return errors.Count == 0 ?
                Result<TImplementationType[], ErrorCodeSet>.Succeed([.. implementations]) :
                Result<TImplementationType[], ErrorCodeSet>.Fail(new ErrorCodeSet(errors));
        }

        return TImplementationType.Parse(values[index])
            .Switch(
                onSuccess: v => ParseManyRecursive<TValueType, TImplementationType>(values, index + 1, [.. implementations, v], errors),
                onFailure: v => ParseManyRecursive<TValueType, TImplementationType>(values, index + 1, implementations, errors.Union(v).ToList()));
    }
}