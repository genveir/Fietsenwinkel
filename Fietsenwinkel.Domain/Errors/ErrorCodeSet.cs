﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fietsenwinkel.Domain.Errors;
public class ErrorCodeSet : IList<ErrorCodes>
{
    public List<ErrorCodes> Errors { get; } = [];

    public ErrorCodes this[int index]
    {
        get => ((IList<ErrorCodes>)Errors)[index];
        set => ((IList<ErrorCodes>)Errors)[index] = value;
    }

    public ErrorCodeSet() { }

    public ErrorCodeSet(IEnumerable<ErrorCodes> errors)
    {
        Errors = errors.ToList();
    }

    public int Count =>
        ((ICollection<ErrorCodes>)Errors).Count;

    public bool IsReadOnly =>
        ((ICollection<ErrorCodes>)Errors).IsReadOnly;

    public void Add(ErrorCodes item) =>
        ((ICollection<ErrorCodes>)Errors).Add(item);

    public void Clear() =>
        ((ICollection<ErrorCodes>)Errors).Clear();

    public bool Contains(ErrorCodes item) =>
        ((ICollection<ErrorCodes>)Errors).Contains(item);

    public void CopyTo(ErrorCodes[] array, int arrayIndex) =>
        ((ICollection<ErrorCodes>)Errors).CopyTo(array, arrayIndex);

    public IEnumerator<ErrorCodes> GetEnumerator() =>
        ((IEnumerable<ErrorCodes>)Errors).GetEnumerator();

    public int IndexOf(ErrorCodes item) =>
        ((IList<ErrorCodes>)Errors).IndexOf(item);

    public void Insert(int index, ErrorCodes item) =>
        ((IList<ErrorCodes>)Errors).Insert(index, item);

    public bool Remove(ErrorCodes item) =>
        ((ICollection<ErrorCodes>)Errors).Remove(item);

    public void RemoveAt(int index) =>
        ((IList<ErrorCodes>)Errors).RemoveAt(index);

    IEnumerator IEnumerable.GetEnumerator() =>
        ((IEnumerable)Errors).GetEnumerator();
}
