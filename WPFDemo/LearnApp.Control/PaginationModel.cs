using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace LearnApp.Control
{
    public class PaginationModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand NavCommand { get; set; }

        public ObservableCollection<PageNumberModel> PageNumList { get; set; } =
            new ObservableCollection<PageNumberModel>();

        private int _pageSize = 20;

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = value;
                NavCommand?.Execute(PageIndex);
            }
        }


        private int _pageIndex = 1;

        public int PageIndex
        {
            get { return _pageIndex; }
            set
            {
                if (value <= 0)
                    _pageIndex = 1;
                else
                    _pageIndex = value;
            }
        }



        private bool _isCanPrevious = true;
        public bool IsCanPrevious
        {
            get => _isCanPrevious;
            set
            {
                this._isCanPrevious = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsCanPrevious"));
            }
        }

        private bool _isCanNext = true;
        public bool IsCanNext
        {
            get => _isCanNext;
            set
            {
                this._isCanNext = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsCanNext"));
            }
        }

        private int _previousIndex;
        /// <summary>
        /// 前一条数据的Index     如果当前Index=2  1   3
        /// </summary>
        public int PreviousIndex
        {
            get => _previousIndex;
            set
            {
                this._previousIndex = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PreviousIndex"));
            }
        }

        private int _nextIndex;
        public int NextIndex
        {
            get => _nextIndex;
            set
            {
                this._nextIndex = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NextIndex"));
            }
        }

        /// <summary>
        /// 填充刷新页码
        /// </summary>
        /// <param name="sumCount">数据总条目数</param>
        public void FillPageNumbers(int sumCount)
        {
            // 总条目数：100    per  20     num=5
            // 第一次刷新：pageIndex=1
            // 第二次刷新：点击了页码：5
            // 第三次刷新：因为重新选择了每页数量   per 30   num=4
            // 简单点的话：当重新选择了每页数量后，直接从1页开始
            // 算法处理一下的话：页码超出范围，显示最后一页


            // 这里进行PageNumList的修改     3.0    2    =1.5   == 2
            int num_count = (int)Math.Ceiling(sumCount * 1.0 / PageSize);
            if (PageIndex > num_count) PageIndex = num_count;

            this.PreviousIndex = PageIndex - 1;
            this.NextIndex = PageIndex + 1;

            // 处理前一页和后一页按钮的可用性
            //if (PageIndex == 1)
            IsCanPrevious = PageIndex != 1;
            //if (PageIndex == num_count)
            IsCanNext = PageIndex != num_count;


            // 页面的显示
            // 20   30  40   导致页面显示不了
            // 1  2  3  4  5  6  ...  16
            // 1 ... 7 8 9 10 11 12 13 ...  16
            // 1 ... 11 12 13 14 15 16

            int min = PageIndex - 4;
            if (min <= 1) min = 1;
            else min = PageIndex - 3;

            int max = PageIndex + 4;
            if (PageIndex <= 5)
                max = Math.Min(9, num_count);
            else
            {
                if (max >= num_count) max = num_count;
                else max = PageIndex + 3;
            }

            if (PageIndex >= num_count - 4)
                min = Math.Max(1, num_count - 8);


            List<string> temp = new List<string>();
            if (min > 1)
            {
                temp.Add("1");
                temp.Add("···");
            }
            for (int i = min; i <= max; i++)
                temp.Add(i.ToString());
            if (max < num_count)
            {
                temp.Add("···");
                temp.Add(num_count.ToString());
            }

            PageNumList.Clear();
            foreach (string str in temp)
            {
                bool state = int.TryParse(str, out int index);
                PageNumList.Add(new PageNumberModel
                {
                    Index = str,
                    IsCurrent = (index == PageIndex),
                    IsEnabled = state
                });
            }
        }

        public class PageNumberModel
        {
            public string Index { get; set; }
            public bool IsEnabled { get; set; } = true;
            public bool IsCurrent { get; set; }
        }
    }
}
