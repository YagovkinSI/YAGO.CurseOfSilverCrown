using System;

namespace YAGO.World.Domain.ValueTypes
{
    public struct LimitedInt : IEquatable<LimitedInt>, IComparable<LimitedInt>
    {
        private int _value;

        public LimitedInt(int value, int minValue = 0, int maxValue = 100)
        {
            if (minValue > maxValue)
                throw new ArgumentException($"MinValue ({minValue}) cannot be greater than MaxValue ({maxValue})");

            MinValue = minValue;
            MaxValue = maxValue;
            _value = Clamp(value, minValue, maxValue);
        }

        public int Value
        {
            get => _value;
            set => _value = Clamp(value, MinValue, MaxValue);
        }

        public int MinValue { get; }
        public int MaxValue { get; }

        private static int Clamp(int value, int min, int max)
        {
            return value < min ? min : value > max ? max : value;
        }

        // Операторы сложения
        public static LimitedInt operator +(LimitedInt a, int b)
        {
            return new LimitedInt(a.Value + b, a.MinValue, a.MaxValue);
        }

        public static LimitedInt operator +(int a, LimitedInt b)
        {
            return new LimitedInt(a + b.Value, b.MinValue, b.MaxValue);
        }

        public static LimitedInt operator +(LimitedInt a, LimitedInt b)
        {
            return a.MinValue != b.MinValue || a.MaxValue != b.MaxValue
                ? throw new InvalidOperationException("Cannot add LimitedInt with different constraints")
                : new LimitedInt(a.Value + b.Value, a.MinValue, a.MaxValue);
        }

        // Операторы вычитания
        public static LimitedInt operator -(LimitedInt a, int b)
        {
            return new LimitedInt(a.Value - b, a.MinValue, a.MaxValue);
        }

        public static LimitedInt operator -(int a, LimitedInt b)
        {
            return new LimitedInt(a - b.Value, b.MinValue, b.MaxValue);
        }

        public static LimitedInt operator -(LimitedInt a, LimitedInt b)
        {
            return a.MinValue != b.MinValue || a.MaxValue != b.MaxValue
                ? throw new InvalidOperationException("Cannot subtract LimitedInt with different constraints")
                : new LimitedInt(a.Value - b.Value, a.MinValue, a.MaxValue);
        }

        // Операторы умножения
        public static LimitedInt operator *(LimitedInt a, int b)
        {
            return new LimitedInt(a.Value * b, a.MinValue, a.MaxValue);
        }

        public static LimitedInt operator *(int a, LimitedInt b)
        {
            return new LimitedInt(a * b.Value, b.MinValue, b.MaxValue);
        }

        public static LimitedInt operator *(LimitedInt a, LimitedInt b)
        {
            return a.MinValue != b.MinValue || a.MaxValue != b.MaxValue
                ? throw new InvalidOperationException("Cannot multiply LimitedInt with different constraints")
                : new LimitedInt(a.Value * b.Value, a.MinValue, a.MaxValue);
        }

        // Операторы деления
        public static LimitedInt operator /(LimitedInt a, int b)
        {
            return b == 0 ? throw new DivideByZeroException() : new LimitedInt(a.Value / b, a.MinValue, a.MaxValue);
        }

        public static LimitedInt operator /(int a, LimitedInt b)
        {
            return b.Value == 0 ? throw new DivideByZeroException() : new LimitedInt(a / b.Value, b.MinValue, b.MaxValue);
        }

        public static LimitedInt operator /(LimitedInt a, LimitedInt b)
        {
            return a.MinValue != b.MinValue || a.MaxValue != b.MaxValue
                ? throw new InvalidOperationException("Cannot divide LimitedInt with different constraints")
                : b.Value == 0 ? throw new DivideByZeroException() : new LimitedInt(a.Value / b.Value, a.MinValue, a.MaxValue);
        }

        // Операторы сравнения
        public static bool operator ==(LimitedInt left, LimitedInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LimitedInt left, LimitedInt right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(LimitedInt left, LimitedInt right)
        {
            return left.Value < right.Value;
        }

        public static bool operator >(LimitedInt left, LimitedInt right)
        {
            return left.Value > right.Value;
        }

        public static bool operator <=(LimitedInt left, LimitedInt right)
        {
            return left.Value <= right.Value;
        }

        public static bool operator >=(LimitedInt left, LimitedInt right)
        {
            return left.Value >= right.Value;
        }

        // Неявное преобразование из int
        public static implicit operator LimitedInt(int value)
        {
            return new LimitedInt(value);
        }

        // Явное преобразование в int
        public static explicit operator int(LimitedInt limited)
        {
            return limited.Value;
        }

        // Реализация интерфейсов
        public bool Equals(LimitedInt other)
        {
            return _value == other._value &&
                   MinValue == other.MinValue &&
                   MaxValue == other.MaxValue;
        }

        public override bool Equals(object? obj)
        {
            return obj is LimitedInt other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value, MinValue, MaxValue);
        }

        public int CompareTo(LimitedInt other)
        {
            return _value.CompareTo(other._value);
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public string ToString(string? format)
        {
            return _value.ToString(format);
        }
    }
}