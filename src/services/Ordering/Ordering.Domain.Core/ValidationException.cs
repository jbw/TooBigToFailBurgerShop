using System;
using System.Collections.Generic;
using System.Linq;

namespace Ordering.Domain.Core
{
    public class ValidationError
    {
        public ValidationError(string context, string message)
        {
            if (string.IsNullOrWhiteSpace(context))
                throw new ArgumentNullException(nameof(context));

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentNullException(nameof(message));

            Message = message;
            Context = context;
        }

        public string Context { get; }
        public string Message { get; }
    }

    public class ValidationException : Exception
    {
        public ValidationException(string message) : base(message)
        {

        }

        public ValidationException(string message, params ValidationError[] errors) : base(message)
        {
            Errors = errors ?? Enumerable.Empty<ValidationError>();
        }

        public IEnumerable<ValidationError>? Errors { get; private set; }
    }
}
