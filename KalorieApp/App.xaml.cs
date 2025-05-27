namespace KalorieApp
{
    public partial class App : Application
    {
        public static int CurrentUserId
        {
            get => Preferences.Get(nameof(CurrentUserId), 0);
            set => Preferences.Set(nameof(CurrentUserId), value);
        }

        public App()
        {
            InitializeComponent();


        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }



    }
}