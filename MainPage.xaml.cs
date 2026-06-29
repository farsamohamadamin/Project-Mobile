using System.Collections.ObjectModel;

namespace farsa
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<string> _tasks = new();

        private string FilePath =>
            Path.Combine(FileSystem.AppDataDirectory, "todo.txt");

        public MainPage()
        {
            InitializeComponent();

            TasksCollection.ItemsSource = _tasks;

            LoadTasks();
        }

        private async void AddTask_Clicked(object sender, EventArgs e)
        {
            string task = TaskEntry.Text?.Trim();

            if (string.IsNullOrEmpty(task))
                return;

            _tasks.Add(task);

            TaskEntry.Text = string.Empty;

            await SaveTasks();
        }

        private async Task SaveTasks()
        {
            await File.WriteAllLinesAsync(FilePath, _tasks);
        }

        private void LoadTasks()
        {
            if (!File.Exists(FilePath))
                return;

            var lines = File.ReadAllLines(FilePath);

            foreach (var line in lines)
            {
                _tasks.Add(line);
            }
        }

        private async void DeleteTask_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var task = button.CommandParameter?.ToString();

            if (task != null)
            {
                _tasks.Remove(task);
                await SaveTasks();
            }
        }
    }
}
