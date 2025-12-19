using LearnApp.Control;
using LearnApp.Shared.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.ViewModel
{
    public class UserGanttControlViewModel : BaseBindable
    {
        public UserGanttControlViewModel()
        {
            InitializeData();
        }
        public GanttData Data { get; set; }
        private void InitializeData()
        {
            Data = new GanttData
            {
                ProjectStart = new DateTime(2024, 1, 1),
                ProjectEnd = new DateTime(2024, 3, 31)
            };

            for (int i = 0; i < 50; i++)
            {
                Data.Tasks.Add(new GanttTask
                {
                    Name = $"T{i}#",
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 1, 15),
                    Progress = i/2,
                    AssignedTo = "",
                    Status = Control.TaskStatus.Completed
                });
            }


        }
    }
}
