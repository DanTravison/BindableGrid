using System.ComponentModel;

namespace SampleApp.ViewModels;

/// <summary>
/// Provides a view model <see cref="Color"/> slider
/// </summary>
public sealed class ColorSliderViewModel : SliderViewModel
{
    /// <summary>
    /// Defines the <see cref="Constraint"/> for RGBA color components as integer values.
    /// </summary>
    static readonly Constraint RGBAConstraint = new(0, 255, 1, 0);

    #region Fields

    readonly ColorSlidersViewModel _model;
    readonly ColorPart _part;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    /// <param name="part">The <see cref="ColorPart"/> to bind to the slider.</param>
    /// <param name="model">The <see cref="ColorSlidersViewModel"/> to update.</param>
    public ColorSliderViewModel(ColorPart part, ColorSlidersViewModel model)
        : base(part.ToString(), RGBAConstraint)
    {
        _model = model;
        _model.PropertyChanged += OnModelPropertyChanged;
        _part = part;
        SetValue();
    }

    #region Property Changed Handlers

    void SetValue()
    {
        switch (_part)
        {
            case ColorPart.Red:
                Value = _model.Red * 255f;
                break;
            case ColorPart.Green:
                Value = _model.Green * 255f;
                break;
            case ColorPart.Blue:
                Value = _model.Blue * 255f;
                break;
            case ColorPart.Alpha:
                Value = _model.Alpha * 255f;
                break;
        }
    }

    private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (object.ReferenceEquals(e, ColorSlidersViewModel.ColorChangedEventArgs))
        {
            SetValue();
        }
    }

    protected override void OnValueChanged()
    {
        base.OnValueChanged();
        switch (_part)
        {
            case ColorPart.Red:
                _model.Red = ToFloat(Value);
                break;
            case ColorPart.Green:
                _model.Green = ToFloat(Value);
                break;
            case ColorPart.Blue:
                _model.Blue = ToFloat(Value);
                break;
            case ColorPart.Alpha:
                _model.Alpha = ToFloat(Value);
                break;
        }
    }

    static internal float ToFloat(double value)
    {
        byte trim = (byte)(value);
        return (float)((double)trim / 255);
    }

    #endregion Property Changed Handlers
}
