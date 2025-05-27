namespace KalorieApp
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDb _localDb;
        private readonly int _userId;

        public MainPage(LocalDb localDb)
        {
            InitializeComponent();
            _localDb = localDb;
            _userId = App.CurrentUserId;

            if (_userId == 0)
            {
                Dispatcher.Dispatch(async () =>
                {
                    await Navigation.PushAsync(new UserDataPage(_localDb));
                    await DisplayAlert("Brak użytkownika", "Wybierz lub dodaj użytkownika, zanim zaczniesz liczyć kalorie", "OK");
                });
            }
            else
            {
                Dispatcher.Dispatch(LoadActiveUserSummary);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_userId != 0)
            {
                await Task.Delay(50); // umożliwia płynniejsze przejście
                await LoadTodaysCalories();
            }
        }


        /// <summary>
        /// Dodaje 100kcl do licznika kalorii
        /// </summary>
        private void HundredClicked(object sender, EventArgs e)
        {
            int intKcl = string.IsNullOrWhiteSpace(AddKcl.Text) ? 0 : int.TryParse(AddKcl.Text, out var result) ? result : 0;
            intKcl += 100;
            AddKcl.Text = intKcl.ToString();
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            if (_userId == 0)
            {
                await DisplayAlert("Brak użytkownika", "Najpierw wybierz użytkownika!", "OK");
                return;
            }

            int.TryParse(AddKcl.Text, out int calories);
            double.TryParse(AddWater.Text, out double waterLiters);

            if (calories <= 0 && waterLiters <= 0)
            {
                await DisplayAlert("Błąd", "Wpisz kalorie lub ilość wypitej wody!", "OK");
                return;
            }

            var entry = new CalorieEntry
            {
                UserId = _userId,
                Calories = calories,
                Water = waterLiters,
                Date = DateTime.Today,
                EntryTime = DateTime.Now,
                MealType = "Inny",
                Note = string.Empty
            };

            await _localDb.CreateCalorieEntry(entry);

            AddKcl.Text = string.Empty;
            AddWater.Text = string.Empty;

            await LoadTodaysCalories();

            await DisplayAlert("Dodano!", $"Dodano 🐷 {calories} kcal i 💧 {waterLiters}L wody", "OK");

            var user = await _localDb.GetUserById(_userId);
            int total = await _localDb.GetTodaysCalories(_userId);

            if (total >= user.BMR * 1.0)
            {
                await DisplayAlert("Uspokój się", "Uspokój się!! 🐷", "OK");
            }
            else if (total > user.BMR * 0.75)
            {
                await DisplayAlert("Limit", "Uważaj świnio bo zbliżasz się do limitu koryta 🐷", "OK");
            }
            else if (total > user.BMR * 0.5)
            {
                await DisplayAlert("Połowa", "Jesteś już w połowie dzisiejszego limitu koryta 🐷", "OK");
            }
        }

        private void OnEntryTextChanged(object sender, EventArgs e) { }

        private void OnEntryCompleted(object sender, EventArgs e) { }

        private async void LoadActiveUserSummary()
        {
            var user = await _localDb.GetUserById(_userId);
            HelloWorld.Text = user.Gender switch
            {
                "M" => $"{user.UserName} knurze!",
                "F" => $"{user.UserName} świnio!",
                _ => $"{user.UserName}, {user.Gender} jesteś świnią czy knurem?"
            };

            //Label Limit Kalorii i wody dziennie
            LabelLimit.Text = $"Limit: {user.BMR-400} kcal i {user.DailyWaterToDrinkLiters} L wody";
        }

        private async Task LoadTodaysCalories()
        {
            if (_userId == 0)
            {
                LabelKcl.Text = "🐷 Brak aktywnego użytkownika 🐷";
                return;
            }

            var user = await _localDb.GetUserById(_userId);
            int total = await _localDb.GetTodaysCalories(_userId);
            double totalWater = await _localDb.GetTodaysWater(_userId);

            LabelKcl.Text = $"🐷 Zjadłeś: {total} kcal 🐷";
            LabelWater.Text = $"💧 Wypiłeś {totalWater:F2} litrów wody 💧";

            if (total >= user.BMR)
            {
                LabelKcl.TextColor = Colors.Red;
            }
            else if (total > user.BMR * 0.75)
            {
                LabelKcl.TextColor = Colors.OrangeRed;

            }
            else if (total > user.BMR * 0.5)
            {
                LabelKcl.TextColor = Colors.Orange;
            }
            else if (total > user.BMR * 0.25)
            {
                LabelKcl.TextColor = Colors.Gold;
            }
            else
            {
                LabelKcl.TextColor = Colors.White; // lub inny neutralny
            }
        }

        private void AddGlassWater(object sender, EventArgs e)
        {
            double doubleWater = string.IsNullOrWhiteSpace(AddWater.Text) ? 0 : double.TryParse(AddWater.Text, out var result) ? result : 0;
            doubleWater += 0.25;
            AddWater.Text = doubleWater.ToString();
        }

    }
}
