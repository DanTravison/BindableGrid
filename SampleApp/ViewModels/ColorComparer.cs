using System.Diagnostics.CodeAnalysis;

namespace SampleApp.ViewModels;

/// <summary>
/// Provides a custom Color <see cref="IEqualityComparer{Color}"/>.
/// </summary>
public sealed class ColorComparer : EqualityComparer<Color>
{
    /// <summary>
    /// Gets a singleton instance of this class.
    /// </summary>
    public static readonly ColorComparer Comparer = new();

    private ColorComparer()
    { }

    /// <summary>
    /// Determines whether the <see cref="Color"/> objects are equal.
    /// </summary>
    /// <param name="x">The first <see cref="Color"/> to compare.</param>
    /// <param name="y">The second <see cref="Color"/> to compare.</param>
    /// <returns>true if the specified objects are equal; otherwise, false.</returns>
    public override bool Equals(Color x, Color y)
    {
        if (x != null && y != null)
        {
            return x.ToInt() == y.ToInt();
        }
        return false;
    }

    /// <summary>
    /// Returns a hash code for the specified object.
    /// </summary>
    /// <param name="color">The <see cref="Color"/> for which a hash code is to be returned.</param>
    /// <returns>A hash code for the specified object.</returns>
    public override int GetHashCode([DisallowNull] Color color)
    {
        return HashCode.Combine(color.Red, color.Green, color.Blue, color.Alpha);
    }
}
