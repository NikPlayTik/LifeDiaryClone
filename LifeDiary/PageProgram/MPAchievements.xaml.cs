using System.Collections.ObjectModel;

namespace LifeDiary.PageProgram;

public partial class MPAchievements : ContentPage
{
    private ObservableCollection<DiaryAchievementsModel> achievements = new ObservableCollection<DiaryAchievementsModel>();

    public MPAchievements()
    {
        InitializeComponent();
        LoadAchievements();
    }

    private void OnButtonTransitionMainPage(object sender, EventArgs e)
    {
        MainPage.IsButtonAchievementsClicked = false;
        Navigation.PopAsync(); // Возвращаемся назад
    }

    protected override bool OnBackButtonPressed()
    {
        MainPage.IsButtonAchievementsClicked = false;
        return base.OnBackButtonPressed();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadAchievements();
    }
    private async void Frame_Tapped(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var achievement = (DiaryAchievementsModel)frame.BindingContext;

        string action = await DisplayActionSheet(null, null, null, "Редактировать", "Удалить");
        switch (action)
        {
            case "Редактировать":
                // Действие для редактирования
                EditAchievement(achievement);
                break;
            case "Удалить":
                // Действие для удаления
                DeleteAchievement(achievement);
                break;
        }
    }
        private async void EditAchievement(DiaryAchievementsModel achievement)
    {
        await Navigation.PushAsync(new APAddAchievements(achievement));
    }
        private async void DeleteAchievement(DiaryAchievementsModel achievement)
    {
        // Реализация логики удаления
        var result = await App.AchievementsDatabase.DeleteAchievementAsync(achievement);
        if (result == 1) // Если удаление прошло успешно
        {
            await DisplayAlert("Удаление", "Достижение успешно удалено", "OK");
            LoadAchievements(); // Обновляем список достижений
        }
        else
        {
            await DisplayAlert("Ошибка", "Произошла ошибка при удалении достижения", "OK");
        }
    }

    private async void LoadAchievements()
    {
        AchievementsStackLayout.Children.Clear(); // Очищаем текущие достижения в UI
        var achievements = await App.AchievementsDatabase.GetAchievementsAsync();
        var goals = await App.GoalsDatabase.GetGoalsAsync(); // Получаем все цели
        foreach (var achievement in achievements) // Используем достижения из SQLite
        {
            var achievementFrame = new Frame
            {
                BackgroundColor = achievement.GoalId != 0 ? Color.FromHex("#1F576F") : Color.FromHex("#1F1277"),
                Padding = 15,
                CornerRadius = 30,
                Margin = new Thickness(0, 0, 0, 20) // Добавляем нижний отступ для разделения достижений
            };

            // Добавляем обработчик событий Tapped если 1 раз нажать
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += Frame_Tapped;
            achievementFrame.GestureRecognizers.Add(tapGestureRecognizer);

            // Устанавливаем BindingContext для фрейма
            achievementFrame.BindingContext = achievement;
            var contentStackLayout = new StackLayout();

            var headerGrid = new Grid
            {
                RowDefinitions =
            {
                new RowDefinition
                {
                    Height = new GridLength(70, GridUnitType.Absolute)
                }
        },
                ColumnDefinitions =
                {
                    new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Auto)
            },
                    new ColumnDefinition
                    {
                        Width = new GridLength(2, GridUnitType.Star)
                    }
    }
            };

            var dateLabel = new Label
            {
                Text = achievement.Date.ToString("dd.MM.yyyy"), // Изменение формата даты
                FontAttributes = FontAttributes.Bold,
                FontSize = 17,
                TextColor = Color.FromHex("#FF8F62"),
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 18, 0, 0)
            };

            var descriptionLabel = new Label
            {
                Text = achievement.Description,
                TextColor = Color.FromHex("#ffffff"),
                FontAttributes = FontAttributes.Bold,
                FontSize = 22,
                Margin = new Thickness(0, 10)
            };

            // Если GoalId достижения не равен нулю, добавляем метку с описанием связанной цели
            if (achievement.GoalId != 0)
            {
                var relatedGoal = goals.FirstOrDefault(g => g.ID == achievement.GoalId); // Находим связанную цель
                if (relatedGoal != null)
                {
                    var relatedGoalLabel = new Label
                    {
                        Text = $"Связанная цель: {relatedGoal.Description}",
                        TextColor = Color.FromHex("#ffffff"),
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 18,
                        Margin = new Thickness(0, 10)
                    };
                    contentStackLayout.Children.Add(relatedGoalLabel);
                }
            }

            contentStackLayout.Children.Add(headerGrid);
            contentStackLayout.Children.Add(descriptionLabel);
            achievementFrame.Content = contentStackLayout;

            headerGrid.Children.Add(dateLabel);
            Grid.SetRow(dateLabel, 0);
            Grid.SetColumn(dateLabel, 0);

            AchievementsStackLayout.Children.Add(achievementFrame);
        }
    }


    public void RefreshAchievements()
    {
        LoadAchievements();
    }
    private async void AddAchievement(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new APAddAchievements());
    }


}