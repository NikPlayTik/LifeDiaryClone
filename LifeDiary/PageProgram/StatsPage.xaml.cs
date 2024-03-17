using Microcharts;
using Microcharts.Maui;
using SkiaSharp;
using System.Globalization;
using System.Linq;

namespace LifeDiary.PageProgram;

public partial class StatsPage : ContentPage
{
	public StatsPage()
	{
		InitializeComponent();
    }


    public async Task EntriesCharts()
    {
        // Получаем данные из базы данных
        var entries = await App.Database.GetEntriesAsync();

        // Проверяем, есть ли записи
        if (!entries.Any())
        {
            // Если нет записей, отображаем сообщение
            NoDataLabel.IsVisible = true;
            BarChartView.IsVisible = false;
            ActivityChartView.IsVisible = false;
        }
        else
        {
            // Создаем данные для диаграмм
            var entriesPerMonth = entries
            .GroupBy(e => e.Date.ToString("MMMM"))
            .ToDictionary(g => g.Key, g => g.Count());

            // Добавляем вторую диаграмму
            var dayOfWeekToShortName = new Dictionary<DayOfWeek, string>
            {
                { DayOfWeek.Monday, "Пн." },
                { DayOfWeek.Tuesday, "Вт." },
                { DayOfWeek.Wednesday, "Ср." },
                { DayOfWeek.Thursday, "Чт." },
                { DayOfWeek.Friday, "Пт." },
                { DayOfWeek.Saturday, "Сб." },
                { DayOfWeek.Sunday, "Вс." }
            };

            var entriesActivity = entries
                .GroupBy(e => new { DayOfWeek = dayOfWeekToShortName[e.Date.DayOfWeek], TimeSlot = GetTimeSlot(e.Date.Hour) })
                .ToDictionary(g => g.Key, g => g.Count());
            // Создаем диаграммы
            var chartEntries = entriesPerMonth.Select(e => new ChartEntry(e.Value)
            {
                Label = e.Key,
                ValueLabel = e.Value.ToString(),
                ValueLabelColor = SKColor.Parse("#ffffff"),

                Color = SKColor.Parse("#D6FD57")
            }).ToList();
            var lineChart = new LineChart
            {
                Entries = chartEntries,
                LabelTextSize = 50f, // размер шрифта для меток
                LabelColor = SKColor.Parse("#ffffff"),
                AnimationDuration = TimeSpan.Zero,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                Margin = 50,
                BackgroundColor = SKColors.Transparent
            };
            BarChartView.Chart = lineChart;

            var chartEntriesActivity = entriesActivity.Select(e => new ChartEntry(e.Value)
            {
                Label = $"{e.Key.DayOfWeek} {e.Key.TimeSlot}",
                ValueLabel = e.Value.ToString(),
                ValueLabelColor = SKColor.Parse("#ffffff"),
                Color = SKColor.Parse("#D6FD57")
            }).ToList();

            var barChartActivity = new BarChart
            {
                Entries = chartEntriesActivity,
                LabelTextSize = 50f, // размер шрифта для меток
                LabelColor = SKColor.Parse("#ffffff"),
                CornerRadius = 30,
                AnimationDuration = TimeSpan.Zero,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                Margin = 50,
                BackgroundColor = SKColors.Transparent
            };

            ActivityChartView.Chart = barChartActivity;
        }
    }
    public async Task GoalsCharts()
    {
        // Получаем данные из базы данных
        var goals = await App.GoalsDatabase.GetGoalsAsync();

        // Проверяем, есть ли цели
        if (!goals.Any())
        {
            // Если нет целей, отображаем сообщение
            NoDataLabel.IsVisible = true;
            GoalsChartView.IsVisible = false;
            GoalsChartViewPlanned.IsVisible = false;
            GoalsChartViewAchieved.IsVisible = false;
            ProgressGoalsChartView.IsVisible = false;
        }
        else
        {
            // Создаем данные для диаграммы
            var goalsPerMonth = goals
            .GroupBy(g => g.StartDate.ToString("MMMM"))
            .ToDictionary(g => g.Key, g => g.Count());

            // Создаем диаграмму 1 
            var chartEntries = goalsPerMonth.Select(g => new ChartEntry(g.Value)
            {
                Label = g.Key,
                ValueLabel = g.Value.ToString(),
                ValueLabelColor = SKColor.Parse("#ffffff"),
                Color = SKColor.Parse("#D6FD57")
            }).ToList();

            var lineChart = new LineChart
            {
                Entries = chartEntries,
                LabelTextSize = 50f, // размер шрифта для меток
                LabelColor = SKColor.Parse("#ffffff"),
                AnimationDuration = TimeSpan.Zero,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                Margin = 50,
                BackgroundColor = SKColors.Transparent
            };

            GoalsChartView.Chart = lineChart;





            var cultureInfo = new System.Globalization.CultureInfo("ru-RU");

            // Создаем данные для первой диаграммы (Запланировано)
            var goalsPerMonthPlanned = goals
                .GroupBy(g => $"{cultureInfo.DateTimeFormat.GetMonthName(g.StartDate.Month)} {g.StartDate.Year}")
                .ToDictionary(g => g.Key, g => g.Count());

            // Создаем первую диаграмму
            var chartEntriesPlanned = goalsPerMonthPlanned.Select(g => new ChartEntry(g.Value)
            {
                Label = g.Key,
                ValueLabel = g.Value.ToString(),
                ValueLabelColor = SKColor.Parse("#ffffff"),
                Color = SKColor.Parse("#D6FD57")
            }).ToList();

            var barChartPlanned = new BarChart
            {
                Entries = chartEntriesPlanned,
                LabelTextSize = 50f, // размер шрифта для меток
                LabelColor = SKColor.Parse("#ffffff"),
                CornerRadius = 30,
                AnimationDuration = TimeSpan.Zero,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                Margin = 50,
                BackgroundColor = SKColors.Transparent
            };

            GoalsChartViewPlanned.Chart = barChartPlanned;

            // Создаем данные для второй диаграммы (Достигнуто)
            var goalsPerMonthAchieved = goals
                .Where(g => g.Progress == 1) // Фильтруем цели, которые были достигнуты (выполнены на 100%)
                .GroupBy(g => $"{cultureInfo.DateTimeFormat.GetMonthName(g.StartDate.Month)} {g.StartDate.Year}")
                .ToDictionary(g => g.Key, g => g.Count());

            // Создаем вторую диаграмму
            var chartEntriesAchieved = goalsPerMonthAchieved.Select(g => new ChartEntry(g.Value)
            {
                Label = g.Key,
                ValueLabel = g.Value.ToString(),
                ValueLabelColor = SKColor.Parse("#ffffff"),
                Color = SKColor.Parse("#D6FD57")
            }).ToList();

            var barChartAchieved = new BarChart
            {
                Entries = chartEntriesAchieved,
                LabelTextSize = 50f, // размер шрифта для меток
                LabelColor = SKColor.Parse("#ffffff"),
                CornerRadius = 30,
                AnimationDuration = TimeSpan.Zero,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                Margin = 50,
                BackgroundColor = SKColors.Transparent
            };

            GoalsChartViewAchieved.Chart = barChartAchieved;




            // Создаем данные для диаграммы
            var goalsPerProgressLevel = new Dictionary<string, int>
        {
            {"0-25", 0},
            {"26-50", 0},
            {"51-75", 0},
            {"76-100", 0}
        };

            foreach (var goal in goals)
            {
                if (goal.Progress >= 0 && goal.Progress <= 0.25)
                    goalsPerProgressLevel["0-25"]++;
                else if (goal.Progress >= 0.26 && goal.Progress <= 0.50)
                    goalsPerProgressLevel["26-50"]++;
                else if (goal.Progress >= 0.51 && goal.Progress <= 0.75)
                    goalsPerProgressLevel["51-75"]++;
                else if (goal.Progress >= 0.76 && goal.Progress <= 1)
                    goalsPerProgressLevel["76-100"]++;
            }

            // Создаем диаграмму
            var chartEntries3 = goalsPerProgressLevel.Select(g => new ChartEntry(g.Value)
            {
                Label = g.Key,
                ValueLabel = g.Value.ToString(),
                ValueLabelColor = SKColor.Parse("#ffffff"),
                Color = SKColor.Parse("#D6FD57")
            }).ToList();

            var barChart = new BarChart
            {
                Entries = chartEntries3,
                LabelTextSize = 50f, // размер шрифта для меток
                LabelColor = SKColor.Parse("#ffffff"),
                AnimationDuration = TimeSpan.Zero,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                Margin = 50,
                BackgroundColor = SKColors.Transparent
            };

            ProgressGoalsChartView.Chart = barChart;
        }
    }
    public async Task AchievementsCharts()
    {
        // Получаем данные из базы данных
        var achievements = await App.AchievementsDatabase.GetAchievementsAsync();

        // Создаем данные для второй диаграммы
        var relatedAchievementsPerMonth = achievements
            .Where(a => a.GoalId != 0) // Фильтруем достижения, которые связаны с целью
            .GroupBy(a => a.Date.ToString("MMMM"))
            .ToDictionary(g => g.Key, g => g.Count());

        // Если есть достижения, создаем диаграмму
        NoDataLabel.IsVisible = false;
        AchievementsChartView.IsVisible = true;

        // Создаем данные для диаграммы
        var achievementsPerMonth = achievements
            .GroupBy(a => a.Date.ToString("MMMM"))
            .ToDictionary(g => g.Key, g => g.Count());

        // Создаем диаграмму
        var chartEntries = achievementsPerMonth.Select(a => new ChartEntry(a.Value)
        {
            Label = a.Key,
            ValueLabel = a.Value.ToString(),
            ValueLabelColor = SKColor.Parse("#ffffff"),
            Color = SKColor.Parse("#D6FD57")
        }).ToList();

        var lineChart = new LineChart
        {
            Entries = chartEntries,
            LabelTextSize = 50f, // размер шрифта для меток
            LabelColor = SKColor.Parse("#ffffff"),
            AnimationDuration = TimeSpan.Zero,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            Margin = 50,
            BackgroundColor = SKColors.Transparent
        };

        AchievementsChartView.Chart = lineChart;

        // Если есть связанные достижения, создаем диаграмму
        NoDataLabel.IsVisible = false;
        // Замените следующую строку на имя вашего ChartView для второй диаграммы
        YourSecondChartView.IsVisible = true;

        var chartEntries2 = relatedAchievementsPerMonth.Select(a => new ChartEntry(a.Value)
        {
            Label = a.Key,
            ValueLabel = a.Value.ToString(),
            ValueLabelColor = SKColor.Parse("#ffffff"),
            Color = SKColor.Parse("#D6FD57")
        }).ToList();

        var lineChart2 = new LineChart
        {
            Entries = chartEntries2,
            LabelTextSize = 50f, // размер шрифта для меток
            LabelColor = SKColor.Parse("#ffffff"),
            AnimationDuration = TimeSpan.Zero,
            LabelOrientation = Orientation.Horizontal,
            ValueLabelOrientation = Orientation.Horizontal,
            Margin = 50,
            BackgroundColor = SKColors.Transparent
        };

        // Замените следующую строку на имя вашего ChartView для второй диаграммы
        YourSecondChartView.Chart = lineChart2;

    }

    // Общая реализация
    private string GetTimeSlot(int hour)
    {
        if (hour >= 0 && hour < 3) return "0:00";
        if (hour >= 3 && hour < 6) return "3:00";
        if (hour >= 6 && hour < 9) return "6:00";
        if (hour >= 9 && hour < 12) return "9:00";
        if (hour >= 12 && hour < 15) return "12:00";
        if (hour >= 15 && hour < 18) return "18:00";
        return "21:00";
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        App.Database = new DiaryEntryDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DiaryEntries.db3"));
        App.GoalsDatabase = new DiaryGoalsDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DiaryGoals.db3"));
        App.AchievementsDatabase = new DiaryAchievementsDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DiaryAchievements.db3"));

        await EntriesCharts();
        await GoalsCharts();
        await AchievementsCharts();
    }
}