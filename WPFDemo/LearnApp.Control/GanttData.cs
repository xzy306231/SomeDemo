// GanttData.cs
using System;
using System.Collections.ObjectModel;

namespace LearnApp.Control
{
    public enum TaskStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Delayed
    }

    public class GanttTask : NotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        private double _progress;
        public double Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        private TaskStatus _status;
        public TaskStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        private string _assignedTo;
        public string AssignedTo
        {
            get => _assignedTo;
            set => SetProperty(ref _assignedTo, value);
        }

        public TimeSpan Duration => EndDate - StartDate;
    }

    public class GanttData
    {
        public ObservableCollection<GanttTask> Tasks { get; set; }
        public DateTime ProjectStart { get; set; }
        public DateTime ProjectEnd { get; set; }

        public GanttData()
        {
            Tasks = new ObservableCollection<GanttTask>();
        }
    }

    public abstract class NotifyPropertyChanged : System.ComponentModel.INotifyPropertyChanged
    {
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, string propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}