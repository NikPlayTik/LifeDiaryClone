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
        Navigation.PopAsync(); // ������������ �����
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

        string action = await DisplayActionSheet(null, null, null, "�������������", "�������");
        switch (action)
        {
            case "�������������":
                // �������� ��� ��������������
                EditAchievement(achievement);
                break;
            case "�������":
                // �������� ��� ��������
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
        // ���������� ������ ��������
        var result = await App.AchievementsDatabase.DeleteAchievementAsync(achievement);
        if (result == 1) // ���� �������� ������ �������
        {
            await DisplayAlert("��������", "���������� ������� �������", "OK");
            LoadAchievements(); // ��������� ������ ����������
        }
        else
        {
            await DisplayAlert("������", "��������� ������ ��� �������� ����������", "OK");
        }
    }

    private async void LoadAchievements()
    {
        AchievementsStackLayout.Children.Clear(); // ������� ������� ���������� � UI
        var achievements = await App.AchievementsDatabase.GetAchievementsAsync();
        var goals = await App.GoalsDatabase.GetGoalsAsync(); // �������� ��� ����
        foreach (var achievement in achievements) // ���������� ���������� �� SQLite
        {
            var achievementFrame = new Frame
            {
                BackgroundColor = achievement.GoalId != 0 ? Color.FromHex("#1F576F") : Color.FromHex("#1F1277"),
                Padding = 15,
                CornerRadius = 30,
                Margin = new Thickness(0, 0, 0, 20) // ��������� ������ ������ ��� ���������� ����������
            };

            // ��������� ���������� ������� Tapped ���� 1 ��� ������
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += Frame_Tapped;
            achievementFrame.GestureRecognizers.Add(tapGestureRecognizer);

            // ������������� BindingContext ��� ������
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
                Text = achievement.Date.ToString("dd.MM.yyyy"), // ��������� ������� ����
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

            // ���� GoalId ���������� �� ����� ����, ��������� ����� � ��������� ��������� ����
            if (achievement.GoalId != 0)
            {
                var relatedGoal = goals.FirstOrDefault(g => g.ID == achievement.GoalId); // ������� ��������� ����
                if (relatedGoal != null)
                {
                    var relatedGoalLabel = new Label
                    {
                        Text = $"��������� ����: {relatedGoal.Description}",
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