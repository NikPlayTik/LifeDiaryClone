namespace LifeDiary.PageProgram;

public partial class APAddAchievements : ContentPage
{
	public APAddAchievements(DiaryAchievementsModel achievement = null)
	{
		InitializeComponent();
        if (achievement == null)
        {
            // ������� ����� ����������, ���� �� ���� �������� �������������
            Achievement = new DiaryAchievementsModel
            {
                Date = DateTime.Now, // ��������� ������� ����
            };
        }
        else
        {
            // ���������� ���������� ����������
            Achievement = achievement;
        }

        this.BindingContext = Achievement;
    }

    public DiaryAchievementsModel Achievement { get; set; }
    private void OnButtonTransitionMainPage(object sender, EventArgs e)
    {
        MainPage.IsButtonEntriesClicked = false;
        Navigation.PopAsync(); // ������������ �����
    }
    async void Save_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(Achievement.Description))
        {
            await DisplayAlert("������", "�������� �� ����� ���� ������.", "OK");
            return;
        }
        await App.AchievementsDatabase.SaveAchievementAsync(Achievement); // ��������� ���������� � ���� ������
        await Navigation.PopAsync();
    }
}