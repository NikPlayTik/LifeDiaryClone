using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Maui.Views;
using System.Collections.ObjectModel;

namespace LifeDiary.PageProgram;

public partial class MPEntries : ContentPage
{
    private ObservableCollection<DiaryEntry> entries = new ObservableCollection<DiaryEntry>();
    public MPEntries()
	{
		InitializeComponent();
	}

    // ����������� ���� � ��������� �� �������� ���� � ������
    private void OnButtonTransitionMainPage(object sender, EventArgs e)
    {
        MainPage.IsButtonEntriesClicked = false;
        Navigation.PopAsync(); // ������������ �����
    }
    protected override bool OnBackButtonPressed()
    {
        MainPage.IsButtonEntriesClicked = false;
        return base.OnBackButtonPressed();
    }

    // �������� ������ �� ���� �������� ������
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadEntries();
    }
    private async void LoadEntries()
    {
        EntriesStackLayout.Children.Clear(); // ������� ������� ������ � UI
        var entries = await App.Database.GetEntriesAsync();
        foreach (var entry in entries) // ���������� ������ �� SQLite
        {
            var entryFrame = new Frame
            {
                BackgroundColor = Color.FromHex("#1F1277"),
                Padding = 15,
                CornerRadius = 30,
                Margin = new Thickness(0, 0, 0, 20) // ��������� ������ ������ ��� ���������� �������
            };

            // ��������� ���������� ������� Tapped ���� 2 ���� ������
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += Frame_Tapped;
            entryFrame.GestureRecognizers.Add(tapGestureRecognizer);

            // ������������� BindingContext ��� ������
            entryFrame.BindingContext = entry;
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
                        Width = new GridLength(1, GridUnitType.Star) 
                    },
                    new ColumnDefinition 
                    { 
                        Width = new GridLength(2, GridUnitType.Star) 
                    }
                }
            };

            var dateLabel = new Label
            {
                Text = entry.Date.ToString("dd.MM.yyyy HH:mm"), // ��������� ������� ���� � �������
                FontAttributes = FontAttributes.Bold,
                FontSize = 17,
                TextColor = Color.FromHex("#FF8F62"),
                HorizontalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 18, 0, 0)
            };

            var titleLabel = new Label
            {
                Text = entry.Title,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                TextColor = Color.FromHex("#ffffff"),
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center
            };

            headerGrid.Children.Add(dateLabel); // ��������� ������� ��� �������� �������
            Grid.SetRow(dateLabel, 0); // ������������� ������ ��� ��������
            Grid.SetColumn(dateLabel, 0); // ������������� ������� ��� ��������

            headerGrid.Children.Add(titleLabel); // ���������� ��������� ������ �������
            Grid.SetRow(titleLabel, 0);
            Grid.SetColumn(titleLabel, 1);

            var descriptionLabel = new Label
            {
                Text = entry.Description,
                TextColor = Color.FromHex("#ffffff"),
                FontAttributes = FontAttributes.Bold,
                FontSize = 22,
                Margin = new Thickness(0, 10)
            };

            contentStackLayout.Children.Add(headerGrid);
            contentStackLayout.Children.Add(descriptionLabel);

            var imagesGrid = new Grid
            {
                ColumnDefinitions =
            {
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
            },
                Margin = new Thickness(0, 10)
            };
            entryFrame.Content = contentStackLayout;
            EntriesStackLayout.Children.Add(entryFrame);
        }
    }

    // ������� �� ����� ����� ��������� ����������� ����
    private async void Frame_Tapped(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var entry = (DiaryEntryModel)frame.BindingContext;

        string action = await DisplayActionSheet(null, null, null, "�������������", "�������");
        switch (action)
        {
            case "�������������":
                // �������� ��� ��������������
                EditItem(entry);
                break;
            case "�������":
                // �������� ��� ��������
                DeleteItem(entry);
                break;
        }
    }
    private async void EditItem(DiaryEntryModel entry)
    {
        await Navigation.PushAsync(new EPAddEntries(entry));
    }
    private async void DeleteItem(DiaryEntryModel entry)
    {
        // ���������� ������ ��������
        var result = await App.Database.DeleteEntryAsync(entry);
        if (result == 1) // ���� �������� ������ �������
        {
            DisplayAlert("��������", "������ ������� �������", "OK");
            LoadEntries(); // ��������� ������ �������
        }
        else
        {
            DisplayAlert("������", "��������� ������ ��� �������� ������", "OK");
        }
    }
    private async void AddEntries(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EPAddEntries());
    }
}