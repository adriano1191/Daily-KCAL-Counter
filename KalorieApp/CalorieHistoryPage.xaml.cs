using System;
using System.Collections.Generic;
using KalorieApp;
using Microsoft.Maui.Controls;

namespace KalorieApp
{
    public partial class CalorieHistoryPage : ContentPage
    {
        private readonly LocalDb _localDb;
        private readonly int _userId;

        public CalorieHistoryPage(LocalDb localDb, int userId)
        {
            InitializeComponent();
            _localDb = localDb;
            _userId = userId;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await Task.Delay(50);
            await LoadEntriesForDate(DateTime.Today);
        }


        private async void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            await LoadEntriesForDate(e.NewDate);
        }

        private async Task LoadEntriesForDate(DateTime date)
        {
            var entries = await _localDb.GetCalorieEntriesForDate(date.Date, _userId);
            calorieListView.ItemsSource = entries;

            int total = entries.Sum(e => e.Calories);
            totalCaloriesLabel.Text = $"Suma kalorii: {total:N0}";
        }

        private async void OnDeleteEntryClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.CommandParameter is CalorieEntry entry)
            {
                bool confirm = await DisplayAlert("Potwierdzenie", "Usun¹æ ten wpis?", "Tak", "Nie");
                if (!confirm)
                    return;

                // ANIMACJA znika elementu
                if (button.Parent is Grid grid)
                {
                    await grid.FadeTo(0, 300);
                }

                try
                {
                    await _localDb.DeleteCalorieEntry(entry);
                    await LoadEntriesForDate(datePicker.Date);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("B³¹d", $"Nie uda³o siê usun¹æ wpisu: {ex.Message}", "OK");
                }
            }
        }



    }
}
