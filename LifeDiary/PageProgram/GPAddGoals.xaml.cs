using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace LifeDiary.PageProgram;

public static class DataStoreGoals
{
    public static ObservableCollection<DiaryGoalsModel> Goals { get; } = new ObservableCollection<DiaryGoalsModel>();
}

public partial class GPAddGoals : ContentPage
{
    public DiaryGoalsModel DiaryGoal { get; set; }

    public GPAddGoals(DiaryGoalsModel goal = null)
    {
        InitializeComponent();

        if (goal == null)
        {
            // Создаем новую цель, если не было передано существующей
            DiaryGoal = new DiaryGoalsModel
            {
                Deadline = DateTime.Now, // Установка текущей даты
                Progress = 0.0 // Установка начального прогресса
            };
        }
        else
        {
            // Используем переданную цель
            DiaryGoal = goal;
        }

        this.BindingContext = DiaryGoal;
    }

    private void OnButtonTransitionMainPage(object sender, EventArgs e)
    {
        MainPage.IsButtonGoalsClicked = false;
        Navigation.PopAsync(); // Возвращаемся назад
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(DiaryGoal.Description))
        {
            await DisplayAlert("Ошибка", "Описание не может быть пустым.", "OK");
            return;
        }

        // Проверяем, что введенное значение является числом от 0 до 100
        var regex = new Regex(@"^100$|^\d{1,2}$");
        if (!regex.IsMatch(GoalProgress.Text))
        {
            await DisplayAlert("Ошибка", "Прогресс должен быть числом от 0 до 100.", "OK");
            return;
        }

        DiaryGoal.Progress = double.Parse(GoalProgress.Text) / 100; // Преобразуем проценты в диапазон от 0 до 1

        await App.GoalsDatabase.SaveGoalAsync(DiaryGoal); // Добавляем цель в коллекцию
        await Navigation.PopAsync();
    }
}