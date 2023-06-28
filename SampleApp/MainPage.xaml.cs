using SampleApp.ViewModels;

namespace SampleApp;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        ColorSlidersViewModel model = new ColorSlidersViewModel(Colors.Blue);

        BindingContext = model;
        InitializeComponent();
    }
}

