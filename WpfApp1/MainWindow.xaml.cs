using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Task> alltask = new List<Task>();
        ObservableCollection<Task> curtask = new ObservableCollection<Task>();
        public MainWindow()
        {
            InitializeComponent();
            for (int i = 1; i <= 7; i++)
            {
                ld.Items.Add(i);
                dfiltercombobox.Items.Add(i);
            }
            l1.ItemsSource = curtask;
            sfiltercombobox.Items.Add(TaskStatus.Processing);
            sfiltercombobox.Items.Add(TaskStatus.Ended);
            sfiltercombobox.Items.Add(TaskStatus.Overdue);
        }

        void RefreshTasks(List<Task> filter)
        {
            curtask.Clear();
            foreach (Task item in filter)
            {
                curtask.Add(item);
            }
        }
        private void addbutton_Click(object sender, RoutedEventArgs e)
        {
            Task t = new Task(tTextBox.Text.ToString(), (int)ld.SelectedItem);
            alltask.Add(t);
            RefreshTasks(alltask);
        }

        private void nfiltertextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            var filter = alltask.Where(s => s.Name.ToLower().Contains(nfiltertextbox.Text.ToLower()));
            RefreshTasks(filter.ToList());
        }

        private void dfiltercombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (dfiltercombobox.SelectedIndex != -1)
            {
                var filter = alltask.Where(s => s.Time == (int)dfiltercombobox.SelectedItem);
                RefreshTasks(filter.ToList());
            }
        }

        public void ResetFilters()
        {
            nfiltertextbox.Clear();
            dfiltercombobox.SelectedIndex = -1;
            sfiltercombobox.SelectedIndex = -1;
            RefreshTasks(alltask);
        }

        private void sfiltercombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sfiltercombobox.SelectedIndex != -1)
            {
                var filter = alltask.Where(s => s.statusenum == (TaskStatus)sfiltercombobox.SelectedItem);
                RefreshTasks(filter.ToList());
            }
        }

        private void compbutton_Click(object sender, RoutedEventArgs e)
        {
            if (l1.SelectedIndex != -1)
            {
                if ((l1.Items[l1.SelectedIndex] as Task).statusenum == TaskStatus.Overdue)
                {
                    MessageBox.Show("задачу нельзя завершить так как она уже просрочена");
                }
                else
                (l1.Items[l1.SelectedIndex] as Task).statusenum = TaskStatus.Ended;
                RefreshTasks(alltask);
            }
        }

        private void resbutton_Click(object sender, RoutedEventArgs e)
        {
            ResetFilters();
        }
    }

    class Task
    {
        private string name;
        public int time;
        public TaskStatus statusenum;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public int Time
        {
            get
            {
                return time;
            }
        }

        public string Status
        {
            get
            {
                switch (statusenum)
                {
                    case TaskStatus.Ended:
                        return "завершено";
                    case TaskStatus.Processing:
                        return "в процессе";
                    case TaskStatus.Overdue:
                        return "просрочено";
                }
                return "неопределено";
            }
        }

        public Task(string name, int time)
        {
            this.name = name;
            this.time = time;
            if ((int)DateTime.Now.DayOfWeek > time)
            {
                statusenum = TaskStatus.Overdue;
            }
            else
            {
                statusenum = TaskStatus.Processing;
            }
        }
    }

    enum TaskStatus
    {
        Ended,
        Processing,
        Overdue
    }
}
