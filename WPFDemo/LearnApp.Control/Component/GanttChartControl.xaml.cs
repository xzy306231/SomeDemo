using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace LearnApp.Control.Component
{
    /// <summary>
    /// GanttChartControl.xaml 的交互逻辑
    /// </summary>
    public partial class GanttChartControl : UserControl
    {
        public static readonly DependencyProperty TasksProperty =
           DependencyProperty.Register("Tasks", typeof(ObservableCollection<GanttTask>),
               typeof(GanttChartControl), new PropertyMetadata(null, OnTasksChanged));

        public ObservableCollection<GanttTask> Tasks
        {
            get => (ObservableCollection<GanttTask>)GetValue(TasksProperty);
            set => SetValue(TasksProperty, value);
        }

        public static readonly DependencyProperty ProjectStartDateProperty =
            DependencyProperty.Register("ProjectStartDate", typeof(DateTime),
                typeof(GanttChartControl), new PropertyMetadata(DateTime.Now));

        public DateTime ProjectStartDate
        {
            get => (DateTime)GetValue(ProjectStartDateProperty);
            set => SetValue(ProjectStartDateProperty, value);
        }

        public static readonly DependencyProperty ProjectEndDateProperty =
            DependencyProperty.Register("ProjectEndDate", typeof(DateTime),
                typeof(GanttChartControl), new PropertyMetadata(DateTime.Now.AddDays(30)));

        public DateTime ProjectEndDate
        {
            get => (DateTime)GetValue(ProjectEndDateProperty);
            set => SetValue(ProjectEndDateProperty, value);
        }

        private const double RowHeight = 30;
        private const double HeaderHeight = 50;
        private const double TimeScaleHeight = 30;
        private const double TaskLabelWidth = 200;
        private const double DayWidth = 30;

        public GanttChartControl()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            DrawGanttChart();
        }

        private static void OnTasksChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is GanttChartControl control)
            {
                control.DrawGanttChart();
            }
        }
        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            // 同步垂直滚动
            if (e.VerticalChange != 0)
            {
                taskLabelsScrollViewer.ScrollToVerticalOffset(mainScrollViewer.VerticalOffset);
            }

            // 同步水平滚动
            if (e.HorizontalChange != 0)
            {
                timelineScrollViewer.ScrollToHorizontalOffset(mainScrollViewer.HorizontalOffset);
            }
        }
        private void DrawGanttChart()
        {
            if (Tasks == null || Tasks.Count == 0)
                return;

            mainCanvas.Children.Clear();
            timelineCanvas.Children.Clear();
            taskLabelsPanel.Children.Clear();

            DrawTimeline();
            DrawGridLines();
            DrawTasks();
        }

        private void DrawTimeline()
        {
            var totalDays = (ProjectEndDate - ProjectStartDate).TotalDays;
            var currentDate = ProjectStartDate;

            for (int i = 0; i <= totalDays; i++)
            {
                if (i % 7 == 0) // 每周绘制主要刻度
                {
                    var x = i * DayWidth;
                    DrawTimeTick(x, currentDate.ToString("MM/dd"), true);
                }
                else if (i % 1 == 0) // 每天绘制次要刻度
                {
                    var x = i * DayWidth;
                    DrawTimeTick(x, currentDate.Day.ToString(), false);
                }

                currentDate = currentDate.AddDays(1);
            }
        }

        private void DrawTimeTick(double x, string label, bool isMajor)
        {
            var line = new Line
            {
                X1 = x,
                Y1 = 0,
                X2 = x,
                Y2 = isMajor ? TimeScaleHeight : TimeScaleHeight / 2,
                Stroke = Brushes.Gray,
                StrokeThickness = isMajor ? 1 : 0.5
            };
            timelineCanvas.Children.Add(line);

            var textBlock = new TextBlock
            {
                Text = label,
                FontSize = isMajor ? 10 : 8,
                Foreground = Brushes.Black,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Canvas.SetLeft(textBlock, x - 10);
            Canvas.SetTop(textBlock, isMajor ? 5 : 15);
            timelineCanvas.Children.Add(textBlock);
        }

        private void DrawGridLines()
        {
            var totalDays = (ProjectEndDate - ProjectStartDate).TotalDays;
            var totalWidth = totalDays * DayWidth;

            // 绘制垂直网格线
            for (int i = 0; i <= totalDays; i++)
            {
                if (i % 7 == 0) // 每周一条主要网格线
                {
                    var line = new Line
                    {
                        X1 = i * DayWidth,
                        Y1 = 0,
                        X2 = i * DayWidth,
                        Y2 = Tasks.Count * RowHeight,
                        Stroke = Brushes.LightGray,
                        StrokeThickness = 0.5
                    };
                    mainCanvas.Children.Add(line);
                }
            }

            // 绘制水平网格线
            for (int i = 0; i <= Tasks.Count; i++)
            {
                var line = new Line
                {
                    X1 = 0,
                    Y1 = i * RowHeight,
                    X2 = totalWidth,
                    Y2 = i * RowHeight,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.5
                };
                mainCanvas.Children.Add(line);
            }
        }

        private void DrawTasks()
        {
            for (int i = 0; i < Tasks.Count; i++)
            {
                var task = Tasks[i];
                var rowTop = i * RowHeight;

                // 绘制任务条
                var taskStartX = (task.StartDate - ProjectStartDate).TotalDays * DayWidth;
                var taskWidth = task.Duration.TotalDays * DayWidth;

                var taskRect = new Border
                {
                    Width = taskWidth,
                    Height = RowHeight * 0.6,
                    CornerRadius = new CornerRadius(3),
                    Background = GetTaskColor(task.Status),
                    BorderBrush = Brushes.DarkGray,
                    BorderThickness = new Thickness(1),
                    ToolTip = CreateTaskToolTip(task)
                };

                Canvas.SetLeft(taskRect, taskStartX);
                Canvas.SetTop(taskRect, rowTop + (RowHeight * 0.2));
                mainCanvas.Children.Add(taskRect);

                // 绘制进度条
                if (task.Progress > 0)
                {
                    var progressWidth = taskWidth * (task.Progress / 100.0);
                    var progressRect = new Border
                    {
                        Width = progressWidth,
                        Height = RowHeight * 0.4,
                        CornerRadius = new CornerRadius(2),
                        Background = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0)),
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    Canvas.SetLeft(progressRect, taskStartX);
                    Canvas.SetTop(progressRect, rowTop + (RowHeight * 0.3));
                    mainCanvas.Children.Add(progressRect);
                }

                // 添加任务标签
                var taskLabel = new Border
                {
                    Height = RowHeight,
                    Background = i % 2 == 0 ? Brushes.White : Brushes.AliceBlue,
                    BorderBrush = Brushes.LightGray,
                    BorderThickness = new Thickness(0, 0, 1, 0),
                    Child = new TextBlock
                    {
                        Text = task.Name,
                        Padding = new Thickness(10, 0, 10, 0),
                        VerticalAlignment = VerticalAlignment.Center,
                        TextTrimming = TextTrimming.CharacterEllipsis
                    }
                };
                taskLabelsPanel.Children.Add(taskLabel);
            }
        }

        private SolidColorBrush GetTaskColor(TaskStatus status)
        {
            return status switch
            {
                TaskStatus.NotStarted => new SolidColorBrush(Color.FromArgb(255, 221, 221, 221)),
                TaskStatus.InProgress => new SolidColorBrush(Color.FromArgb(255, 91, 155, 213)),
                TaskStatus.Completed => new SolidColorBrush(Color.FromArgb(255, 112, 173, 71)),
                TaskStatus.Delayed => new SolidColorBrush(Color.FromArgb(255, 255, 192, 0)),
                _ => Brushes.Gray
            };
        }

        private object CreateTaskToolTip(GanttTask task)
        {
            var stackPanel = new StackPanel
            {
                Background = Brushes.White,
                //Padding = new Thickness(10)
            };

            stackPanel.Children.Add(new TextBlock
            {
                Text = task.Name,
                FontWeight = FontWeights.Bold,
                FontSize = 12
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"开始时间: {task.StartDate:yyyy-MM-dd}"
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"结束时间: {task.EndDate:yyyy-MM-dd}"
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"进度: {task.Progress}%"
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"负责人: {task.AssignedTo}"
            });

            return stackPanel;
        }

    }
}
