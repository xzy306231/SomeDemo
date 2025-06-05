using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LearnApp.Shared.Base
{
    public class BaseBindable : ObservableObject
    {
        public ICommand GetDataCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        public ICommand DeleteDataCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public BaseBindable()
        {
            GetDataCommand = new RelayCommand(GetData);

            UpdateCommand = new RelayCommand<object?>(UpdateData);

            DeleteDataCommand = new RelayCommand<object?>(DeleteData);

            AddCommand = new RelayCommand<object?>(AddData);
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }
        public virtual void GetData() { }
        public virtual void UpdateData(object? model) { }
        public virtual void DeleteData(object? model) { }
        public virtual void AddData(object? model) { }
    }
}
