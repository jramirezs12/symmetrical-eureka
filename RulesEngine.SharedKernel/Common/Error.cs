namespace RulesEngine.SharedKernel.Common
{
    public class Error : IEquatable<Error>
    {
        public static readonly Error None = new(string.Empty, string.Empty);
        public static readonly Error NullValue = new("Error.NullValue", "The specified result value is null");

        public string Code { get; }
        public string Message { get; set; }
        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public static implicit operator string(Error error) => error.Code;
        public static bool operator !=(Error? a, Error? b) => !(a == b);
        public static bool operator ==(Error? a, Error? b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Code == b.Code && a.Message == b.Message;
        }
        public bool Equals(Error? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Code == other.Code && Message == other.Message;
        }

        public override bool Equals(object? obj) => Equals(obj as Error);
        public override int GetHashCode() => HashCode.Combine(Code, Message);
    }
}
