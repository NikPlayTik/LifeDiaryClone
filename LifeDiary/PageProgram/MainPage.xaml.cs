namespace LifeDiary.PageProgram;

public partial class MainPage : ContentPage
{
    // Поля для исправления повторного нажатия
    public static bool IsButtonEntriesClicked = false;
    public static bool IsButtonGoalsClicked = false;
    public static bool IsButtonAchievementsClicked = false;

    public MainPage()
	{
		InitializeComponent();
        UpdateButtonState();
    }

    // Переход в окно ЗАПИСИ
    private void OnButtonEntriesClicked(object sender, EventArgs e)
    {
        // проверка на переход если кнопка уже была нажата
        if (!IsButtonEntriesClicked)
        {
            IsButtonEntriesClicked = true;
            // переход к MPEntries и обновление состояния кнопки там
            Navigation.PushAsync(new MPEntries());
        }
    }
    private void OnButtonMediaClicked(object sender, EventArgs e)
    {
        if (!IsButtonEntriesClicked)
        {
            IsButtonEntriesClicked = true; 
            Navigation.PushAsync(new MPEntries());
        }
    }

    // Загрузка недавних добавленных ЗАПИСЕЙ
    private async Task LoadLastEntry()
    {
        var entries = await App.Database.GetEntriesAsync();
        var lastEntry = entries.LastOrDefault();
        if (lastEntry != null)
        {
            LastEntryDate.Text = lastEntry.Date.ToString("dd.MM.yyyy HH:mm");
            ButtonEntries.Text = lastEntry.Title;
            LastEntryDescription.Text = TrimDescription(lastEntry.Description, 60);
        }
    }


    // Переход в окно ЦЕЛИ
    private void OnButtonGoalsClicked(object sender, EventArgs e)
    {
        if (!IsButtonGoalsClicked)
        {
            IsButtonGoalsClicked = true;
            Navigation.PushAsync(new MPGoals());
        }
    }

    // Загрузка недавних добавленных ЦЕЛЕЙ
    private async Task LoadLastGoal()
    {
        var goals = await App.GoalsDatabase.GetGoalsAsync();
        var lastGoal = goals.LastOrDefault();
        if (lastGoal != null)
        {
            LastGoalDate.Text = lastGoal.Deadline.ToString("dd.MM.yyyy HH:mm");
            LastGoalDescription.Text = TrimDescription(lastGoal.Description, 60);
            LastGoalProgress.Progress = lastGoal.Progress;
            LastGoalProgressPercent.Text = $"Прогресс: {lastGoal.Progress * 100}%";

            // Вычисляем количество дней до дедлайна
            var daysToDeadline = (lastGoal.Deadline - DateTime.Now).TotalDays;

            // Устанавливаем текст для DaysLeftLabel в зависимости от количества дней
            if (daysToDeadline < 1)
            {
                DaysLeftLabel.Text = "Быстрее завершить! День окончания цели";
            }
            else
            {
                DaysLeftLabel.Text = $"Осталось: {Math.Ceiling(daysToDeadline)} {GetDaysWord(Math.Ceiling(daysToDeadline))}";
            }

            // Устанавливаем цвет эллипса в зависимости от количества дней до дедлайна
            if (daysToDeadline > 7)
            {
                LastGoalEllipse.Fill = Color.FromHex("#5AFD57"); // Зеленый
            }
            else if (daysToDeadline > 3)
            {
                LastGoalEllipse.Fill = Color.FromHex("#FFA500"); // Оранжевый
            }
            else
            {
                LastGoalEllipse.Fill = Color.FromHex("#FF0000"); // Красный
            }
        }
    }


    // Переход в окно ДОСТИЖЕНИЯ
    private void OnButtonAchievementsClicked(object sender, EventArgs e)
    {
        if (!IsButtonAchievementsClicked)
        {
            IsButtonAchievementsClicked = true;
            Navigation.PushAsync(new MPAchievements());
        }
    }

    // Загрузка недавних добавленных ДОСТИЖЕНИЙ
    private async Task LoadLastAchievement()
    {
        var achievements = await App.AchievementsDatabase.GetAchievementsAsync();
        var lastAchievement = achievements.LastOrDefault();
        if (lastAchievement != null)
        {
            AchievementDateLabel.Text = lastAchievement.Date.ToString("dd.MM.yyyy HH:mm");
            AchievementDescriptionLabel.Text = TrimDescription(lastAchievement.Description, 60);
        }
    }


    // Общие методы
    private void UpdateButtonState()
    {
        ButtonEntries.IsEnabled = !IsButtonEntriesClicked;
        ButtonGoals.IsEnabled = !IsButtonGoalsClicked;
        ButtonAchievements.IsEnabled = !IsButtonAchievementsClicked;

    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        UpdateButtonState();
        await LoadLastEntry();
        await LoadLastGoal();
        await LoadLastAchievement();
    }
    private string TrimDescription(string description, int maxLength = 100)
    {
        if (string.IsNullOrEmpty(description)) return description;
        return description.Length <= maxLength ? description : $"{description.Substring(0, maxLength)}...";
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

