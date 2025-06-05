using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.Shared.BaseBusinessDto
{
    public class OilsDto
    {
        public List<OilDto> Oils { get; set; }
    }
    public class OilDto
    {
        public string OilCode { get; set; }
        public string OilName { get; set; }

        public bool IsChecked { get; set; }

        public bool IsSelect { get; set; }

        public decimal Percent { get; set; }

    }
}
