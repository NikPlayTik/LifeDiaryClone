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

    // Исправление бага с открытием из основого окна в Записи
    private void OnButtonTransitionMainPage(object sender, EventArgs e)
    {
        MainPage.IsButtonEntriesClicked = false;
        Navigation.PopAsync(); // Возвращаемся назад
    }
    protected override bool OnBackButtonPressed()
    {
        MainPage.IsButtonEntriesClicked = false;
        return base.OnBackButtonPressed();
    }

    // Выгрузка данных из окна добавить запись
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadEntries();
    }
    private async void LoadEntries()
    {
        EntriesStackLayout.Children.Clear(); // Очищаем текущие записи в UI
        var entries = await App.Database.GetEntriesAsync();
        foreach (var entry in entries) // Используем записи из SQLite
        {
            var entryFrame = new Frame
            {
                BackgroundColor = Color.FromHex("#1F1277"),
                Padding = 15,
                CornerRadius = 30,
                Margin = new Thickness(0, 0, 0, 20) // Добавляем нижний отступ для разделения записей
            };

            // Добавляем обработчик событий Tapped если 2 раза нажать
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += Frame_Tapped;
            entryFrame.GestureRecognizers.Add(tapGestureRecognizer);

            // Устанавливаем BindingContext для фрейма
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
                Text = entry.Date.ToString("dd.MM.yyyy HH:mm"), // Изменение формата даты и времени
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

            headerGrid.Children.Add(dateLabel); // Добавляем элемент без указания позиции
            Grid.SetRow(dateLabel, 0); // Устанавливаем строку для элемента
            Grid.SetColumn(dateLabel, 0); // Устанавливаем столбец для элемента

            headerGrid.Children.Add(titleLabel); // Аналогично добавляем второй элемент
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

    // Нажатие на фрейм чтобы появилось контекстное меню
    private async void Frame_Tapped(object sender, EventArgs e)
    {
        var frame = (Frame)sender;
        var entry = (DiaryEntryModel)frame.BindingContext;

        string action = await DisplayActionSheet(null, null, null, "Редактировать", "Удалить");
        switch (action)
        {
            case "Редактировать":
                // Действие для редактирования
                EditItem(entry);
                break;
            case "Удалить":
                // Действие для удаления
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
        // Реализация логики удаления
        var result = await App.Database.DeleteEntryAsync(entry);
        if (result == 1) // Если удаление прошло успешно
        {
            DisplayAlert("Удаление", "Запись успешно удалена", "OK");
            LoadEntries(); // Обновляем список записей
        }
        else
        {
            DisplayAlert("Ошибка", "Произошла ошибка при удалении записи", "OK");
        }
    }
    private async void AddEntries(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EPAddEntries());
    }
}