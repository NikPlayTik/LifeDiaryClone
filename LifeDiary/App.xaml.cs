using LifeDiary.PageProgram;

namespace LifeDiary;

public partial class App : Application
{
    public static DiaryEntryDatabase Database { get; set; }
    public static DiaryGoalsDatabase GoalsDatabase { get; set; }
    public static DiaryAchievementsDatabase AchievementsDatabase { get; set; }
    public App()
	{
		InitializeComponent();
        string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DiaryEntries.db3");
        Database = new DiaryEntryDatabase(dbPath);

        string goalsDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DiaryGoals.db3");
        GoalsDatabase = new DiaryGoalsDatabase(goalsDbPath);

        string achievementsDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DiaryAchievements.db3");
        AchievementsDatabase = new DiaryAchievementsDatabase(achievementsDbPath);

        MainPage = new AppShell();
	}
}
