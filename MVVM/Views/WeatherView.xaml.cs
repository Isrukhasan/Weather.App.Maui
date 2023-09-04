using Weather.MVVM.Models;

namespace Weather.MVVM.Views;

public partial class WeatherView : ContentPage
{
    public WeatherView()
    {
        InitializeComponent();
        BindingContext = new WeatherViewModel();
    }
}