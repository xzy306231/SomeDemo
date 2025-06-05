using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LearnApp.Shared.Enum
{
    public enum ArchiveState
    {
        [Description("未归档")]
        NotArchive = 0,
        [Description("已归档")]
        HasArchive = 1,
    }
}
