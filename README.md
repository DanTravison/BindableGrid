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

Unfortunately, I was not able to solve the text measuring issue.  Creating
a Label in code and using it to measure my desired width worked fine on Windows
but not at all on Android.  My attempts to use Skia to measure the text did 
provide a reasonable starting point and using DisplayDensity to scale it 
to Maui worked fine on Windows. Unfortunately, the results on the Android 
simulator were very different and proved just as unusable.

*As an aside, it would be GREAT if Maui provided a reusable MeasureText. This lack
has thwarted my efforts in a number of cases.*

After a couple of weeks going down these paths, I decided to investigate 
sub-classing Grid, adding an ItemsSource for data binding and an DataTemplate
for each column. It turned out that sub-classing was not an option, primarily
due to ColumnDefinition being sealed. Additionally, I could not rationalize
ColumnSpan and RowSpan with ItemsSource.

The final solution uses encapsulation. I define a custom ColumnDefinition
class that accepts a GridLength Width and an ItemTemplate.

Grid.RowDefinitions is replaced with a simple RowHeight of type GridLength.

RowSpacing and ColumnSpacing properties provide a relay to the encapsulated Grid
properties.

The UI presentation appears as follows:

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

- I still have the problem of MeasureText to solve. While ColumnWidth=Auto solved the alignment
problem with the color component names, it does not solve the problem if the column contains
dynamic text (e.g., the 'Value' column of the color slider).  You can see this issue
when you run the example and set all sliders to one or two digit values. The right most column
grows and shrinks as the value lengths grow and shrink.

When I solve the MeasureText problem, I intend to extend ColumnDefinition to support a 
logical character width to calculate at runtime a fixed column width based on 
fixed count of characters.

For example, to set the column width to 3 characters, the value might be '3@' 
or perhaps '3#'.  It will also need to support FontFamily and FontSize as a minimum
to ensure the column width is calculated using a consistent font metric.
 
