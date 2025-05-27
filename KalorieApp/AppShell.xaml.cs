using Koryto;

namespace Koryto
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

        }
        private async void OnCalorieHistoryClicked(object sender, EventArgs e)
        {
            int _loggedUserId = App.CurrentUserId;
            var localDb = new LocalDb(); // lub wyciągnij z DI
            int userId = _loggedUserId;  // musisz mieć go gdzieś zapisany (np. w App.Current.Properties)
            Shell.Current.FlyoutIsPresented = false; //to zamyka wysunięte menu
            await Shell.Current.Navigation.PushAsync(new CalorieHistoryPage(localDb, userId));
        }


    }
}
