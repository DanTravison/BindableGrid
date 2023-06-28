using ObjectModel;
using System.ComponentModel;

namespace SampleApp.ViewModels;

/// <summary>
/// Provides a <see cref="Color"/> view model for editing the individual RGBA values of a <see cref="Color"/>.
/// </summary>
public sealed class ColorSlidersViewModel : ObservableObject
{
    #region Fields

    string _argbText;
    Color _color;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    public ColorSlidersViewModel()
        : this(Colors.Black)
    {
    }

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    /// <param name="color">The initial <see cref="Color"/>.</param>
    public ColorSlidersViewModel(Color color)
    {
        _color = color;
        Sliders = new List<ColorSliderViewModel>()
        {
            new ColorSliderViewModel(ColorPart.Red, this),
            new ColorSliderViewModel(ColorPart.Green, this),
            new ColorSliderViewModel(ColorPart.Blue, this),
            new ColorSliderViewModel(ColorPart.Alpha, this)
        };
    }

    #region Properties

    /// <summary>
    /// Gets the <see cref="ColorSliderViewModel"/> for each component of a <see cref="Microsoft.Maui.Graphics.Color"/>.
    /// </summary>
    public IEnumerable<ColorSliderViewModel> Sliders
    {
        get;
    }

    /// <summary>
    /// Gets or sets the <see cref="Microsoft.Maui.Graphics.Color"/> value.
    /// </summary>
    public Color Color
    {
        get => _color;
        set
        {
            if (SetProperty(ref _color, value, ColorComparer.Comparer, ColorChangedEventArgs))
            {
                ARGB = Color.ToArgbHex(true);
            }
        }
    }

    #endregion Properties

    #region Properties

    /// <summary>
    /// Gets the <see cref="Color"/> as a ARGB hex string.
    /// </summary>
    public string ARGB
    {
        get => _argbText;
        private set => SetProperty(ref _argbText, value, StringComparer.Ordinal, ARGBChangedEventArgs);
    }

    /// <summary>
    /// Gets the <see cref="Color.Red"/> component.
    /// </summary>
    public float Red
    {
        get => Color.Red;
        set
        {
            if (value != Color.Red)
            {
                Color = new Color(value, Color.Green, Color.Blue, Color.Alpha);
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="Color.Green"/> component.
    /// </summary>
    public float Green
    {
        get => Color.Green;
        set
        {
            if (value != Color.Green)
            {
                Color = new Color(Color.Red, value, Color.Blue, Color.Alpha);
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="Color.Blue"/> component.
    /// </summary>
    public float Blue
    {
        get => Color.Blue;
        set
        {
            if (value != Color.Blue)
            {
                Color = new Color(Color.Red, Color.Green, value, Color.Alpha);
            }
        }
    }

    /// <summary>
    /// Gets the <see cref="Color.Alpha"/> component.
    /// </summary>
    public float Alpha
    {
        get => Color.Alpha;
        set
        {
            if (value != Color.Alpha)
            {
                Color = new Color(Color.Red, Color.Green, Color.Blue, value);
            }
        }
    }

    #endregion Properties

    #region Cached PropertyChangedEventArgs

    /// <summary>
    /// Provides <see cref="PropertyChangedEventArgs"/> passed to the <see cref="ObservableObject.PropertyChanged"/> event when <see cref="ARGB"/> changes.
    /// </summary>
    static public readonly PropertyChangedEventArgs ARGBChangedEventArgs = new PropertyChangedEventArgs(nameof(ARGB));

    /// <summary>
    /// Provides <see cref="PropertyChangedEventArgs"/> passed to the <see cref="ObservableObject.PropertyChanged"/> event when <see cref="Color"/> changes.
    /// </summary>
    static public readonly PropertyChangedEventArgs ColorChangedEventArgs = new PropertyChangedEventArgs(nameof(Color));

    #endregion Cached PropertyChangedEventArgs
}
