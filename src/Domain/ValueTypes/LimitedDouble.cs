using System;

namespace YAGO.World.Domain.ValueTypes
{
    public struct LimitedDouble : IEquatable<LimitedDouble>, IComparable<LimitedDouble>
    {
        private double _value;

        public LimitedDouble(double value, double minValue = 0, double maxValue = 100)
        {
            if (minValue > maxValue)
                throw new ArgumentException($"MinValue ({minValue}) cannot be greater than MaxValue ({maxValue})");

            MinValue = minValue;
            MaxValue = maxValue;
            _value = Clamp(value, minValue, maxValue);
        }

        public double Value
        {
            get => _value;
            set => _value = Clamp(value, MinValue, MaxValue);
        }

        public double MinValue { get; }
        public double MaxValue { get; }

        private static double Clamp(double value, double min, double max)
        {
            return value < min ? min : value > max ? max : value;
        }

        // Операторы сложения
        public static LimitedDouble operator +(LimitedDouble a, double b)
        {
            return new LimitedDouble(a.Value + b, a.MinValue, a.MaxValue);
        }

        public static LimitedDouble operator +(double a, LimitedDouble b)
        {
            return new LimitedDouble(a + b.Value, b.MinValue, b.MaxValue);
        }

        public static LimitedDouble operator +(LimitedDouble a, LimitedDouble b)
        {
            return a.MinValue != b.MinValue || a.MaxValue != b.MaxValue
                ? throw new InvalidOperationException("Cannot add LimitedDouble with different constraints")
                : new LimitedDouble(a.Value + b.Value, a.MinValue, a.MaxValue);
        }

        // Операторы вычитания
        public static LimitedDouble operator -(LimitedDouble a, double b)
        {
            return new LimitedDouble(a.Value - b, a.MinValue, a.MaxValue);
        }

        public static LimitedDouble operator -(double a, LimitedDouble b)
        {
            return new LimitedDouble(a - b.Value, b.MinValue, b.MaxValue);
        }

        public static LimitedDouble operator -(LimitedDouble a, LimitedDouble b)
        {
            return a.MinValue != b.MinValue || a.MaxValue != b.MaxValue
                ? throw new InvalidOperationException("Cannot subtract LimitedDouble with different constraints")
                : new LimitedDouble(a.Value - b.Value, a.MinValue, a.MaxValue);
        }

        // Операторы умножения
        public static LimitedDouble operator *(LimitedDouble a, double b)
        {
            return new LimitedDouble(a.Value * b, a.MinValue, a.MaxValue);
        }

        public static LimitedDouble operator *(double a, LimitedDouble b)
        {
            return new LimitedDouble(a * b.Value, b.MinValue, b.MaxValue);
        }

        public static LimitedDouble operator *(LimitedDouble a, LimitedDouble b)
        {
            return a.MinValue != b.MinValue || a.MaxValue != b.MaxValue
                ? throw new InvalidOperationException("Cannot multiply LimitedDouble with different constraints")
                : new LimitedDouble(a.Value * b.Value, a.MinValue, a.MaxValue);
        }

        // Операторы деления
        public static LimitedDouble operator /(LimitedDouble a, double b)
        {
            return b == 0 ? throw new DivideByZeroException() : new LimitedDouble(a.Value / b, a.MinValue, a.MaxValue);
        }

        public static LimitedDouble operator /(double a, LimitedDouble b)
        {
            return b.Value == 0 ? throw new DivideByZeroException() : new LimitedDouble(a / b.Value, b.MinValue, b.MaxValue);
        }

        public static LimitedDouble operator /(LimitedDouble a, LimitedDouble b)
        {
            return a.MinValue != b.MinValue || a.MaxValue != b.MaxValue
                ? throw new InvalidOperationException("Cannot divide LimitedDouble with different constraints")
                : b.Value == 0 ? throw new DivideByZeroException() : new LimitedDouble(a.Value / b.Value, a.MinValue, a.MaxValue);
        }

        // Операторы сравнения
        public static bool operator ==(LimitedDouble left, LimitedDouble right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(LimitedDouble left, LimitedDouble right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(LimitedDouble left, LimitedDouble right)
        {
            return left.Value < right.Value;
        }

        public static bool operator >(LimitedDouble left, LimitedDouble right)
        {
            return left.Value > right.Value;
        }

        public static bool operator <=(LimitedDouble left, LimitedDouble right)
        {
            return left.Value <= right.Value;
        }

        public static bool operator >=(LimitedDouble left, LimitedDouble right)
        {
            return left.Value >= right.Value;
        }

        // Неявное преобразование из double
        public static implicit operator LimitedDouble(double value)
        {
            return new LimitedDouble(value);
        }

        // Явное преобразование в double
        public static explicit operator double(LimitedDouble limited)
        {
            return limited.Value;
        }

        // Реализация интерфейсов
        public bool Equals(LimitedDouble other)
        {
            return Math.Abs(_value - other._value) < double.Epsilon &&
                   Math.Abs(MinValue - other.MinValue) < double.Epsilon &&
                   Math.Abs(MaxValue - other.MaxValue) < double.Epsilon;
        }

        public override bool Equals(object? obj)
        {
            return obj is LimitedDouble other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_value, MinValue, MaxValue);
        }

        public int CompareTo(LimitedDouble other)
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
