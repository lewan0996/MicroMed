﻿namespace Shared.Domain.Exceptions;

public class ObjectNotFoundException : Exception
{
    public ObjectNotFoundException(string message) : base(message) { }
}