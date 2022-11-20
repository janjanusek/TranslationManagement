using System;
using System.Linq.Expressions;

namespace TranslationManagement.Api.Exceptions;

public class NotFoundException : ApplicationException
{
    private readonly object _value;

    public NotFoundException(object value)
    {
        _value = value;
    }

    public virtual object GetDescriptiveValue() => _value;
}