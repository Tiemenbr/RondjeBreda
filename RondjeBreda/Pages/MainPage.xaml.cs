using RondjeBreda.ViewModels;

namespace RondjeBreda
{
    /// <summary>
    /// The class for the mainpage with the viewmodel for it.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private HomePageViewModel homePageViewModel;

        public MainPage(HomePageViewModel homePageViewModel)
        {
            InitializeComponent();
            this.homePageViewModel = homePageViewModel;
        }
    }

}
