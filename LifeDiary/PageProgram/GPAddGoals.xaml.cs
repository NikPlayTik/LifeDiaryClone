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
            // ������� ����� ����, ���� �� ���� �������� ������������
            DiaryGoal = new DiaryGoalsModel
            {
                Deadline = DateTime.Now, // ��������� ������� ����
                Progress = 0.0 // ��������� ���������� ���������
            };
        }
        else
        {
            // ���������� ���������� ����
            DiaryGoal = goal;
        }

        this.BindingContext = DiaryGoal;
    }

    private void OnButtonTransitionMainPage(object sender, EventArgs e)
    {
        MainPage.IsButtonGoalsClicked = false;
        Navigation.PopAsync(); // ������������ �����
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(DiaryGoal.Description))
        {
            await DisplayAlert("������", "�������� �� ����� ���� ������.", "OK");
            return;
        }

        // ���������, ��� ��������� �������� �������� ������ �� 0 �� 100
        var regex = new Regex(@"^100$|^\d{1,2}$");
        if (!regex.IsMatch(GoalProgress.Text))
        {
            await DisplayAlert("������", "�������� ������ ���� ������ �� 0 �� 100.", "OK");
            return;
        }

        DiaryGoal.Progress = double.Parse(GoalProgress.Text) / 100; // ����������� �������� � �������� �� 0 �� 1

        await App.GoalsDatabase.SaveGoalAsync(DiaryGoal); // ��������� ���� � ���������
        await Navigation.PopAsync();
    }
}