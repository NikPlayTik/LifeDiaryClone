namespace LifeDiary.PageProgram;

public partial class APAddAchievements : ContentPage
{
	public APAddAchievements(DiaryAchievementsModel achievement = null)
	{
		InitializeComponent();
        if (achievement == null)
        {
            // Создаем новое достижение, если не было передано существующего
            Achievement = new DiaryAchievementsModel
            {
                Date = DateTime.Now, // Установка текущей даты
            };
        }
        else
        {
            // Используем переданное достижение
            Achievement = achievement;
        }

        this.BindingContext = Achievement;
    }

    public DiaryAchievementsModel Achievement { get; set; }
    private void OnButtonTransitionMainPage(object sender, EventArgs e)
    {
        MainPage.IsButtonEntriesClicked = false;
        Navigation.PopAsync(); // Возвращаемся назад
    }
    async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Achievement.Description))
        {
            await DisplayAlert("Ошибка", "Описание не может быть пустым.", "OK");
            return;
        }
        await App.AchievementsDatabase.SaveAchievementAsync(Achievement); // Добавляем достижение в базу данных
        await Navigation.PopAsync();
    }
}