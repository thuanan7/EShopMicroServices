using Ordering.Domain.Exceptions;

namespace Ordering.Domain.ValueObjects
{
    public record OrderName
    {
        private const int DefaltLength = 5;
        public string Value { get; } = default!;
        private OrderName(string value) => Value = value;
        public static OrderName Of(string value)
        {
            ArgumentNullException.ThrowIfNull(value, nameof(value));
            ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaltLength, nameof(value));

            return new OrderName(value);
        }
    }
}
