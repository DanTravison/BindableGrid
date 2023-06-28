namespace SampleApp.ViewModels;

/// <summary>
/// Provides a constraint for a double value.
/// </summary>
public readonly struct Constraint
{
    #region Fields

    /// <summary>
    /// Defines the minimum value for the constraint
    /// </summary>
    public readonly double Minimum;

    /// <summary>
    /// Defines the maximum value for the constraint
    /// </summary>
    public readonly double Maximum;

    /// <summary>
    /// Defines the interval value for the constraint
    /// </summary>
    public readonly double Interval;

    /// <summary>
    /// Defines the number of decimal digits to use in <see cref="Constrain"/>.
    /// </summary>
    public readonly int Digits;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of this struct.
    /// </summary>
    /// <param name="minimum">The  minimum value for the constraint.</param>
    /// <param name="maximum">The maximum value for the constraint.</param>
    /// <param name="interval">The interval value for the constraint.</param>
    /// <param name="digits">The number of decimal digits to use in <see cref="Constrain"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="interval"/> is less than or equal to zero
    /// -or-
    /// <paramref name="minimum"/> is greater than or equal to <paramref name="maximum"/>
    /// -or-
    /// <paramref name="digits"/> is less than zero.
    /// </exception>
    public Constraint(double minimum, double maximum, double interval, int digits)
    {
        if (interval <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(interval));
        }
        if (minimum >= maximum)
        {
            throw new ArgumentOutOfRangeException(nameof(minimum));
        }
        if (digits < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(digits));
        }
        Minimum = minimum;
        Maximum = maximum;
        Interval = interval;
        Digits = digits;
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Constrains the value.
    /// </summary>
    /// <param name="value">THe value to constrain.</param>
    /// <returns>The constrained value.</returns>
    public double Constrain(double value)
    {
        if (double.IsNaN(value) || value < Minimum)
        {
            value = Minimum;
        }
        else if (value > Maximum)
        {
            value = Maximum;
        }
        else
        {
            value = Math.Round(value, Digits, MidpointRounding.ToEven);
        }
        return value;
    }

    /// <summary>
    /// Determines if difference between two double values are within half the <see cref="Interval"/>.
    /// </summary>
    /// <param name="first">The first value to compare.</param>
    /// <param name="second">The second value to compare.</param>
    /// <returns>
    /// true if the difference between <paramref name="first"/> and <paramref name="second"/>
    /// is less than half the <see cref="Interval"/>; otherwise, false.
    /// </returns>
    public bool IsEqual(double first, double second)
    {
        return Math.Abs(first - second) < Interval / 2;
    }

    #endregion Methods
}