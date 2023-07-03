namespace BindableGrid;

/// <summary>
/// Provides utilities for measuring text.
/// </summary>
public static class TextUtilities
{
    static readonly object _lock = new object();
    static IStringSizeService _stringSizeService;

    static IStringSizeService StringSizeService
    {
        get
        {
            lock (_lock)
            {

                if (_stringSizeService == null)
                {
#if WINDOWS
                    _stringSizeService = new Microsoft.Maui.Graphics.Win2D.W2DStringSizeService();
#elif ANDROID || MACCATALYST || IOS
                    _stringSizeService = new Microsoft.Maui.Graphics.Platform.PlatformStringSizeService();
#else
                    throw new NotSupportedException(typeof(IStringSizeService).Name);
#endif
                }
            }
            return _stringSizeService;
        }
    }

    /// <summary>
    /// Gets the width needed to render the specified <paramref name="text"/>.
    /// </summary>
    /// <param name="text">The text to render.</param>
    /// <param name="fontFamily">The string font family name of the font to use to render the text.</param>
    /// <param name="fontAttributes">The <see cref="FontAttributes"/>.</param>
    /// <param name="fontSize">The font size.</param>
    /// <returns>The width of the <paramref name="text"/>.</returns>
    public static float TextWidth(string text, string fontFamily, FontAttributes fontAttributes, double fontSize)
    {
        IFont font = GetFont(fontFamily, fontAttributes);

        return StringSizeService.GetStringSize(text, font, (float)fontSize).Width;
    }

    /// <summary>
    /// Gets the <see cref="Size"/> needed to render the specified <paramref name="text"/>.
    /// </summary>
    /// <param name="text">The text to render.</param>
    /// <param name="fontFamily">The string font family name of the font to use to render the text.</param>
    /// <param name="fontAttributes">The <see cref="FontAttributes"/>.</param>
    /// <param name="fontSize">The font size.</param>
    /// <returns>The <see cref="Size"/> of the rectangle needed to render the text.</returns>
    public static Size Measure(string text, string fontFamily, FontAttributes fontAttributes, double fontSize)
    {
        IFont font = GetFont(fontFamily, fontAttributes);
        return StringSizeService.GetStringSize(text, font, (float)fontSize);
    }

    /// <summary>
    /// Gets the <see cref="Size"/> needed to render the specified <paramref name="text"/>.
    /// </summary>
    /// <param name="text">The text to render.</param>
    /// <param name="fontFamily">The string font family name of the font to use to render the text.</param>
    /// <param name="fontAttributes">The <see cref="FontAttributes"/>.</param>
    /// <param name="fontSize">The font size.</param>
    /// <param name="horizontalAlignment">The <see cref="HorizontalAlignment"/> of the text.</param>
    /// <param name="verticalAlignment">The <see cref="VerticalAlignment"/> of the text.</param>
    /// <returns>The <see cref="Size"/> of the rectangle needed to render the text.</returns>
    public static Size Measure
    (
        string text, 
        string fontFamily, 
        FontAttributes fontAttributes, 
        double fontSize,
        HorizontalAlignment horizontalAlignment,
        VerticalAlignment verticalAlignment
    )
    {
        IFont font = GetFont(fontFamily, fontAttributes);
        return StringSizeService.GetStringSize(text, font, (float)fontSize, horizontalAlignment, verticalAlignment);
    }


    /// <summary>
    /// Gets an <see cref="IFont"/> to use with <see cref="IStringSizeService.GetStringSize(string, IFont, float)"/>
    /// </summary>
    /// <param name="fontFamily">The font family name.</param>
    /// <param name="fontAttributes">The <see cref="FontAttributes"/>.</param>
    /// <returns>
    /// An <see cref="IFont"/> for the specified <paramref name="fontFamily"/> and <paramref name="fontAttributes"/>.
    /// </returns>
    static public IFont GetFont(string fontFamily, FontAttributes fontAttributes)
    {
        FontStyleType fontStyle = fontAttributes == FontAttributes.Italic
                      ? FontStyleType.Italic
                      : FontStyleType.Normal;

        int fontWeight = fontAttributes == FontAttributes.Bold 
            ? FontWeights.Bold 
            : FontWeights.Normal;

        return new Microsoft.Maui.Graphics.Font(fontFamily, fontWeight, fontStyle);
    }
}
