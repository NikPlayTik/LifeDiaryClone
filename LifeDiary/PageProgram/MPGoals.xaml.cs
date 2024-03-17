using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.ObjectModel;

namespace LifeDiary.PageProgram;

public partial class MPGoals : ContentPage
{
    private ObservableCollection<DiaryGoalsModel> goals = new ObservableCollection<DiaryGoalsModel>();
    public MPGoals()
	{
		InitializeComponent();
    }

    // Переход назад в окно
    private void OnButtonTransitionMainPage(object sender, EventArgs e)
    {
        MainPage.IsButtonGoalsClicked = false;
        Navigation.PopAsync(); // Возвращаемся назад
    }
    protected override bool OnBackButtonPressed()
    {
        MainPage.IsButtonGoalsClicked = false;
        return base.OnBackButtonPressed();
    }

    // Контекстное меню при нажатии на Frame
    private async void Frame_Tapped(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var goal = (DiaryGoalsModel)frame.BindingContext;

        string action = await DisplayActionSheet(null, null, null, "Редактировать", "Удалить", "Связать с достижением");
        switch (action)
        {
            case "Редактировать":
                // Действие для редактирования
                EditItem(goal);
                break;
            case "Удалить":
                // Действие для удаления
                DeleteItem(goal);
                break;
            case "Связать с достижением":
                LinkToAchievement(goal);
                break;
        }
    }
        private async void AddGoals(object sender, EventArgs e)
        {
        await Navigation.PushAsync(new GPAddGoals());
        }
        private async void EditItem(DiaryGoalsModel goal)
        {
        await Navigation.PushAsync(new GPAddGoals(goal));
        }
        private async void DeleteItem(DiaryGoalsModel goal)
        {
            // Реализация логики удаления
            var result = await App.GoalsDatabase.DeleteGoalAsync(goal);
            if (result == 1) // Если удаление прошло успешно
            {
                DisplayAlert("Удаление", "Цель успешно удалена", "OK");
                LoadGoals(); // Обновляем список целей
            }
            else
            {
                DisplayAlert("Ошибка", "Произошла ошибка при удалении цели", "OK");
            }
        }
        private async void LinkToAchievement(DiaryGoalsModel goal)
        {
            var achievement = new DiaryAchievementsModel
            {
                Title = goal.Description.Substring(goal.Description.Length / 2), // Заголовок достижения берется из середины описания цели
                Date = DateTime.Now, // Устанавливаем текущую дату
                GoalId = goal.ID, // Устанавливаем GoalId для достижения
            };

            // Сохраняем новое достижение в базе данных
            await App.AchievementsDatabase.SaveAchievementAsync(achievement);

            // Устанавливаем AchievementId для цели
            goal.AchievementId = achievement.ID;
            await App.GoalsDatabase.SaveGoalAsync(goal); // Обновляем цель в базе данных

            // Обновляем список достижений
            MPAchievements mPAchievements = new MPAchievements();
            mPAchievements.RefreshAchievements();
        }


    // Выгрузка данных целей
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadGoals();
    }
    private async void LoadGoals()
    {
        GoalsStackLayout.Children.Clear(); // Очищаем текущие цели в UI
        var goals = await App.GoalsDatabase.GetGoalsAsync();
        foreach (var goal in goals) // Используем цели из SQLite
        {
            var goalFrame = new Frame
            {
                BackgroundColor = Color.FromHex("#1F1277"),
                Padding = 15,
                CornerRadius = 30,
                Margin = new Thickness(0, 0, 0, 20) // Добавляем нижний отступ для разделения целей
            };

            // Добавляем обработчик событий Tapped если 2 раза нажать
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += Frame_Tapped;
            goalFrame.GestureRecognizers.Add(tapGestureRecognizer);

            // Устанавливаем BindingContext для фрейма
            goalFrame.BindingContext = goal;
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

            // Вычисляем количество дней до дедлайна
            var daysToDeadline = (goal.Deadline - DateTime.Now).TotalDays;

            var dateGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                },  
            };

            headerGrid.Children.Add(dateGrid);
            Grid.SetRow(dateGrid, 0);
            Grid.SetColumn(dateGrid, 0);

            var dateLabel = new Label
            {
                Text = goal.Deadline.ToString("dd.MM.yyyy HH:mm"), // Изменение формата даты
                FontAttributes = FontAttributes.Bold,
                FontSize = 17,
                TextColor = Color.FromHex("#FF8F62"),
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 18, 0, 0)
            };

            // Создаем метку для отображения оставшихся дней
            var daysLeftLabel = new Label
            {
                Text = daysToDeadline < 1 ? "Быстрее завершить!" : $"Осталось: {Math.Ceiling(daysToDeadline)} {GetDaysWord(Math.Ceiling(daysToDeadline))}",
                FontAttributes = FontAttributes.Bold,
                FontSize = 14,
                TextColor = Color.FromHex("#FF8F62"),
                Margin = new Thickness(0, 0, 0, 10)
            };

            // Создаем эллипс
            var deadlineEllipse = new Ellipse
            {
                WidthRequest = 35,
                HeightRequest = 35,
                HorizontalOptions = LayoutOptions.End,
            };

            // Устанавливаем цвет эллипса в зависимости от количества дней до дедлайна
            if (daysToDeadline > 7)
            {
                deadlineEllipse.Fill = Color.FromHex("#5AFD57"); // Зеленый
            }
            else if (daysToDeadline > 3)
            {
                deadlineEllipse.Fill = Color.FromHex("#FFA500"); // Оранжевый
            }
            else
            {
                deadlineEllipse.Fill = Color.FromHex("#FF0000"); // Красный
            }

            // Добавляем эллипс в Grid
            headerGrid.Children.Add(deadlineEllipse);
            Grid.SetRow(deadlineEllipse, 0);
            Grid.SetColumn(deadlineEllipse, 1);

            var descriptionLabel = new Label
            {
                Text = goal.Description,
                TextColor = Color.FromHex("#ffffff"),
                FontAttributes = FontAttributes.Bold,
                FontSize = 22,
                Margin = new Thickness(0, 10)
            };

            contentStackLayout.Children.Add(headerGrid);
            contentStackLayout.Children.Add(descriptionLabel);

            var progressBar = new ProgressBar
            {
                Progress = goal.Progress,
                Margin = new Thickness(0, 10)
            };

            contentStackLayout.Children.Add(progressBar);
            goalFrame.Content = contentStackLayout;

            var progressLabel = new Label
            {
                Text = $"Прогресс: {goal.Progress * 100}%", // Изменение формата прогресса
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                TextColor = Color.FromHex("#ffffff"),
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center
            };

            contentStackLayout.Children.Add(progressLabel); // Добавляем элемент в StackLayout

            dateGrid.Children.Add(dateLabel); // Добавляем элемент без указания позиции
            Grid.SetRow(dateLabel, 0); // Устанавливаем строку для элемента
            Grid.SetColumn(dateLabel, 0); // Устанавливаем столбец для элемента

            dateGrid.Children.Add(daysLeftLabel); // Добавляем элемент без указания позиции
            Grid.SetRow(daysLeftLabel, 1); // Устанавливаем строку для элемента
            Grid.SetColumn(daysLeftLabel, 0); // Устанавливаем столбец для элемента

            GoalsStackLayout.Children.Add(goalFrame);
        }
    }
    private string GetDaysWord(double days)
    {
        int lastDigit = (int)days % 10;
        if (days >= 11 && days <= 14 || lastDigit >= 5 && lastDigit <= 9 || lastDigit == 0)
        {
            return "дней";
        }
        else if (lastDigit >= 2 && lastDigit <= 4)
        {
            return "дня";
        }
        else
        {
            return "день";
        }
    }
}