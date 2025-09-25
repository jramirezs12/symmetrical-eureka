using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RulesEngine.SharedKernel.Common
{
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;
        private readonly Error? _error;

        protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error) => _value = value;
        protected internal Result(Error error, bool isSuccess, Error error2) : base(isSuccess, error2) => _error = error;
        public TValue? Value => IsSuccess
            ? _value!
            : default;

        public static implicit operator Result<TValue>(TValue? value) => Create(value);
        public static Result<TValue> Create(TValue? value)
        {
            if (value is null)
            {
                return Failure<TValue>(new Error("Error.NullValue", "The specified result value is null"));
            }

            return Success(value);
        }
    }
}
