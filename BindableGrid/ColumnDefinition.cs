using ObjectModel;
using System.ComponentModel;

namespace BindableGrid;

/// <summary>
/// Provides a column definition for a <see cref="Grid"/>.
/// </summary>
public sealed class ColumnDefinition : ObservableObject
{
    #region Fields

    GridLength _width;
    DataTemplate _itemTemplate;

    #endregion Fields

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    public ColumnDefinition()
    {
        _width = GridLength.Auto;
    }

    /// <summary>
    /// Initializes a new instance of this class.
    /// </summary>
    /// <param name="width">The <see cref="GridLength"/> defining the width of the column.</param>
    public ColumnDefinition(GridLength width)
    {
        _width = width;
    }

    #region Properties

    /// <summary>
    /// Gets or sets width of the column.
    /// </summary>
    [TypeConverter(typeof(GridLengthTypeConverter))]
    public GridLength Width
    {
        get => _width;
        set
        {
            if (!_width.Equals(value))
            {
                _width = value;
                OnPropertyChanged(WidthChangedEventArgs);
            }
        }
    }

    /// <summary>
    /// Gets or sets <see cref="DataTemplate"/> for contents of the column.
    /// </summary>
    public DataTemplate ItemTemplate
    {
        get => _itemTemplate;
        set
        {
            if (!ReferenceEquals(_itemTemplate, value))
            {
                _itemTemplate = value;
                OnPropertyChanged(ItemTemplateChangedEventArgs);
            }
        }
    }

    #endregion Properties

    #region Cached PropertyChangedEventArgs

    /// <summary>
    /// Provides <see cref="PropertyChangedEventArgs"/> passed to the <see cref="ObservableObject.PropertyChanged"/> event when <see cref="Width"/> changes.
    /// </summary>
    public static readonly PropertyChangedEventArgs WidthChangedEventArgs = new PropertyChangedEventArgs(nameof(Width));

    /// <summary>
    /// Provides <see cref="PropertyChangedEventArgs"/> passed to the <see cref="ObservableObject.PropertyChanged"/> event when <see cref="ItemTemplate"/> changes.
    /// </summary>
    public static readonly PropertyChangedEventArgs ItemTemplateChangedEventArgs = new PropertyChangedEventArgs(nameof(ItemTemplate));

    #endregion Cached PropertyChangedEventArgs
}
