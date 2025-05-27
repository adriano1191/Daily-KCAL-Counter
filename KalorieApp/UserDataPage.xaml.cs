using System.Diagnostics;
using System.Reflection;

namespace KalorieApp;

public partial class UserDataPage : ContentPage
{
    private readonly LocalDb _localDb;
    private int _editUserId;
    public UserDataPage(LocalDb localDb)
    {
        InitializeComponent();
        _localDb = localDb;
        Task.Run(async () => listView.ItemsSource = await _localDb.GetUsers());
        Debug.WriteLine(Path.Combine(FileSystem.AppDataDirectory, "local_kalorie_db.db3"));

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await Task.Delay(50);
        await LoadUsers();
    }

    private async Task LoadUsers()
    {
        listView.ItemsSource = await _localDb.GetUsers();
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        // Zamieniamy opis z Pickera ("Mê¿czyzna"/"Kobieta") na "M"/"F"
        var genderDisplay = genderPicker.SelectedItem?.ToString();
        string gender = genderDisplay == "Kobieta" ? "F" : "M";

        var user = new User
        {
            Id = _editUserId,
            UserName = nameEntryField!.Text?.Trim(),
            Height = ConvertEnterToInt(heightEntryField.Text),
            Weight = ConvertEnterToInt(weightEntryField.Text),
            Age = ConvertEnterToInt(ageEntryField.Text),
            Gender = gender
        };

        if (_editUserId == 0)
        {
            await _localDb.Create(user);
        }
        else
        {
            await _localDb.Update(user);
            _editUserId = 0;
        }

        nameEntryField.Text = string.Empty;
        heightEntryField.Text = string.Empty;
        weightEntryField.Text = string.Empty;
        ageEntryField.Text = string.Empty;
        genderPicker.SelectedIndex = -1;

        await LoadUsers();
    }

    private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        var user = e.Item as User;
        if (user == null) return;

        var action = await DisplayActionSheet("Wybierz akcjê", "Anuluj", null, "Edytuj", "Usuñ", "Ustaw jako aktywnego");


        switch (action)
        {
            case "Edytuj":
                _editUserId = user.Id;
                nameEntryField.Text = user.UserName;
                heightEntryField.Text = user.Height.ToString();
                weightEntryField.Text = user.Weight.ToString();
                ageEntryField.Text = user.Age.ToString();
                genderPicker.SelectedItem = user.Gender == "F" ? "Kobieta" : "Mê¿czyzna";

                break;

            case "Usuñ":
                bool confirm = await DisplayAlert("PotwierdŸ", $"Czy na pewno chcesz usun¹æ {user.UserName}?", "Tak", "Nie");
                if (confirm)
                {
                    await _localDb.Delete(user);
                    await LoadUsers();
                }
                break;

            case "Ustaw jako aktywnego":
                App.CurrentUserId = user.Id;
                await DisplayAlert("U¿ytkownik aktywny", $"{user.UserName} zosta³ ustawiony jako aktywny", "OK");
                break;

        }
    }

    public int ConvertEnterToInt(string entryTextValue)
    {
        if (int.TryParse(entryTextValue, out int convertedValue))
            return convertedValue;

        DisplayAlert("B³¹d", "WprowadŸ liczbê ca³kowit¹.", "OK");
        return 0; // lub mo¿esz rzuciæ wyj¹tek, jeœli wolisz
    }

}