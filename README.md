# BindableGrid
Provides an experimental Maui Grid control that is populated via ItemsSource and ItemTemplates.

## Background
I have a project that provides a color selector and palette editor component
and each uses a set of sliders to select each component of the color, Red, Green, Blue, 
and Alpha.   
For my first draft, I created a ContentView
with Grid as the root and started spelling out the details for each slider.  

Each slider needs a label for the Color component name, a glyph to decrease the value, 
the slider itself, a glyph to increase the value, and finally a label displaying 
the slider's value and the associated tool tips for the slider and the glyphs.

Conceptually, I wanted the sliders to appear as follows:

    Red   < ----------------- > 255
    Green < ----------------- > 255
    Blue  < ----------------- > 255
    Alpha < ----------------- > 255

This quickly became unwieldy, requiring spelling out 25 controls and exposing
a proliferation of properties on the view model. The resulting view and the view model
were quite complex and tedious to maintain.

I tossed this aside and went down an encapsulation route by defining a 'color slider'
view + view model for each component and a 'parent' view model containing the 
color and the set of slider view models.  The encapsulation, as expected
was much easier to manage and the code cleaned up nicely.

The one issue I found with this approach is the component name labels cause the sliders
to be unaligned.

    Red < --------------------- > 0
    Green < ----------------- > 255
    Blue < -------------------- > 0
    Alpha < ----------------- > 255

I made various attempts to solve this using a custom Label class that overrode
Measure to force all the label widths to match mirror the Grid Width=Auto behavior.

After a couple of weeks going down these paths, I decided to investigate 
sub-classing Grid, adding an ItemsSource for data binding and an DataTemplate
for each column. It turned out that sub-classing was not an option, primarily
due to ColumnDefinition being sealed. Additionally, I could not rationalize
ColumnSpan and RowSpan with ItemsSource.

The final solution uses encapsulation. I defined a custom ColumnDefinition
class that accepts a ColumnWidth and an ItemTemplate.

Grid.RowDefinitions is replaced with a simple RowHeight of type GridLength.

Grid.ColumnDefinitions replaces GridLength with a new ColumnWidth type. This type
supports the expected GridLength patterns as well as two new patterns for calculating
the width:

- Literal - a literal string is used to calculate the absolute width.
- Character Count - a count of characters is used to calculate the absolute width.

These patterns are intended to be used as a absolute width alternative using 
font metrics instead of device independent units.

The RowSpacing and ColumnSpacing properties provide a relay to the encapsulated Grid
properties.

## Literal Column Width - #literal

Syntax:
#literal [, font family] [, font size] [, font attributes]

The '#' character indicates a literal string and is followed immediately by the
string to use to measure the text.  Optionally, font family, size, and attributes 
may be provided to determine the font to use to measure the literal string.

Example: Use '255' as the literal string and measure with the OpenSansRegular font with 
a font size of 24pt.

#255, OpenSansRegular, 18

## Character Count Column Width - @count
Syntax:
@N [, font family] [, font size] [, font attributes]

The '@' character indicates a character count and is followed immediately by the
count of characters to use to measure the text. Optionally, font family, size, and attributes 
may be provided to determine the font to use to measure the characters.

Example: Define the width as 3 characters and measure with the OpenSansRegular font with 
a font size of 24pt and font attribute of Bold.

#255, OpenSansRegular, 18, Bold

The UI presentation appears as follows. The slider's value is presented as two columns. The
first uses the @255 literal while the second uses the @3 character count. When the sample is 
run, note that the 2 rightmost columns remain a fixed width regardless of the value displayed. 

![Image](./ScreenShot.png?raw=true)

# The Code
The repository contains two projects, SampleApp and BindableGrid.

## BindableGrid
The code for the bindable grid resides in the BindableGrid project and 
includes the following:

- Grid.xaml and Grid.xaml.cs - Grid control itself
- ColumnDefinition - the custom column definition class for the grid.
- ColumnDefinitionCollection - The collection of ColumnDefinition
- ObservableObject - a simple INotifyPropertyChanged implementation with a SetProperty<T>.
- TextUtilties - Encapsulates IStringSizeService to provide a text measurment using font attributes, horizontal and vertical alignment.

## SampleApp
Provides an example of using the BindableGrid to create a color editor for editing
the 4 components of a color and a Rectangle to display the resulting color.

The bindable Grid usage is completely described in MainPage.xaml illustrating how to use
ItemTemplates for each column, binding an ItemsSource, and configuring RowSpacing. If you 
are only interested in the bindable Grid usage, you need look no further.

The ViewModels directory contains the view models for the underlying color selection. 

- ColorSliderViewModel - Color slider view model for the individual color components.
- ColorSlidersViewModel - The ColorSliderViewModel collection that is used as the Grid.ItemSource.
- ColorComparer - color comparer requiring an exact match of the ARGB components.
- ColorPart - enum to define the color components (red, green, blue, alpha) and is used by ColorSliderViewModel.
- SliderViewModel - Slider view model supporting Minimum, Maximum, and Interval providing the base class for ColorSliderViewModel.
- Constraint - manages the constraints of the value in SliderViewModel.
- Command - ICommand implementation with Text and Description properties for Increase/Decrease commands and Tooltip properties.

# Caveats/Future

- The project has only been tested on Windows. Further testing on iOS/MacCatalyst and Android is planned.

- When using Character Width and Literal text for column widths, verify they meet your needs.

- Character Width column widths use 'W' as the repeating character and will produce a width that is greater than what Label.Measure returns for the average text of the same length.

- Literal column widths do not match what Label.Measure returns for the same text. Additional testing is needed to determine where the difference is occurring
 
