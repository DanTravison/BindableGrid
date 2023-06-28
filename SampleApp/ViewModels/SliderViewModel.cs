using ObjectModel;
using SampleApp.Resources;
using System.ComponentModel;
using System.Globalization;

namespace SampleApp.ViewModels;

/// <summary>
/// Provides a view model for a <see cref="Slider"/>.
/// </summary>
public class SliderViewModel : ObservableObject
{
    #region Fields

    readonly Constraint _constraint;
    double _value = 0;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    /// <param name="name">The display name of the slider value.</param>
    /// <param name="constraint">The <see cref="Constraint"/> to use to constrain the <see cref="Value"/>.</param>
    public SliderViewModel(string name, Constraint constraint)
        : this(name, constraint.Minimum, constraint)
    {
    }

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    /// <param name="name">The display name of the slider value.</param>
    /// <param name="value">The initial <see cref="Value"/>.</param>
    /// <param name="constraint">The <see cref="Constraint"/> to use to constrain the <see cref="Value"/>.</param>
    public SliderViewModel(string name, double value, Constraint constraint)
        : base()
    {
        Name = name;
        _constraint = constraint;
        _value = _constraint.Constrain(value);
        IncrementCommand = new Command((command) =>
        {
            Increment();
        });

        DecrementCommand = new Command((command) =>
        {
            Decrement();
        });

        Description = string.Format
        (
            CultureInfo.CurrentCulture,
            Strings.SliderTooltipFormat,
            constraint.Minimum, constraint.Maximum, constraint.Interval
        );
    }

    #endregion Constructors

    #region Command Properties

    /// <summary>
    /// Gets the <see cref="Command"/> to invoke to increment the <see cref="Value"/>.
    /// </summary>
    public Command IncrementCommand
    {
        get;
    }

    /// <summary>
    /// Gets the <see cref="Command"/> to invoke to increment the <see cref="Value"/>.
    /// </summary>
    public Command DecrementCommand
    {
        get;
    }

    #endregion Command Properties

    #region Properties

    /// <summary>
    /// Gets the text to associate with the label.
    /// </summary>
    public string Name
    {
        get;
    }

    /// <summary>
    /// Gets the description for the slider.
    /// </summary>
    public string Description
    {
        get;
    }

    /// <summary>
    /// Gets the slider's value.
    /// </summary>
    public double Value
    {
        get => _value;
        set
        {
            if (double.IsNaN(value))
            {
                value = Minimum;
            }
            else
            {
                value = double.Clamp(value, Minimum, Maximum);
            }

            double newValue = Round(value, Interval);

            if (double.IsNaN(newValue))
            {
                newValue = value;
            }
            if (SetProperty(ref _value, newValue, ValueChangedEventArgs))
            {
                OnValueChanged();
            }
        }
    }

    /// <summary>
    /// Gets the slider's interval
    /// </summary>
    public double Interval
    {
        get => _constraint.Interval;
    }

    /// <summary>
    /// Gets the slider's minimum value.
    /// </summary>
    public double Minimum
    {
        get => _constraint.Minimum;
    }

    /// <summary>
    /// Gets the sliders maximum value.
    /// </summary>
    public double Maximum
    {
        get => _constraint.Maximum;
    }

    #endregion Properties

    #region Methods

    static internal double Round(double value, double interval)
    {
        if (interval > 0)
        {
            double remainder = value % interval;
            double threshold = interval / 2;
            if (remainder == 0)
            {
            }
            else if (remainder >= threshold)
            {
                value = value - remainder + interval;
            }
            else
            {
                value -= remainder;
            }
        }
        return value;
    }

    /// <summary>
    /// Increments the <see cref="Value"/> by the <see cref="Interval"/>.
    /// </summary>
    /// <returns>true if the value changed; otherwise, false if the value is already at the
    /// <see cref="Maximum"/>.
    /// </returns>
    public bool Increment()
    {
        if (Value <= Maximum - Interval)
        {
            Value += Interval;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Decrements the <see cref="Value"/> by the <see cref="Interval"/>.
    /// </summary>
    /// <returns>true if the value changed; otherwise, false if the value is already at the
    /// <see cref="Minimum"/>.
    /// </returns>
    public bool Decrement()
    {
        if (Value >= Minimum + Interval)
        {
            Value -= Interval;
            return true;
        }
        return false;
    }

    protected virtual void OnValueChanged()
    {
        IncrementCommand.IsEnabled = _value <= Maximum - Interval;
        DecrementCommand.IsEnabled = _value >= Minimum + Interval;
    }

    #endregion Methods

    #region Cached PropertyChangedEventArgs

    /// <summary>
    /// Provides <see cref="PropertyChangedEventArgs"/> passed to the <see cref="ObservableObject.PropertyChanged"/> event when <see cref="Value"/> changes.
    /// </summary>
    static internal readonly PropertyChangedEventArgs ValueChangedEventArgs = new PropertyChangedEventArgs(nameof(Value));

    #endregion Cached PropertyChangedEventArgs
}
