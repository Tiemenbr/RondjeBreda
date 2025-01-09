using RondjeBreda.Domain.Interfaces;
using RondjeBreda.ViewModels;

namespace RondjeBreda.Pages
{
    /// <summary>
    /// The class for the mainpage with the viewmodel for it.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private HomePageViewModel homePageViewModel;
        private IDatabase database;

        public MainPage(HomePageViewModel homePageViewModel, IDatabase database) {
            InitializeComponent();
            this.homePageViewModel = homePageViewModel;
            this.database = database;
            BindingContext = homePageViewModel;
        }

        protected override async void OnAppearing() {
            base.OnAppearing();
            await database.Init();
        }
    }

}
