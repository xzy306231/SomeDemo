﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LearnApp.Control
{
    public class BaseDialogWindow : Window
    {
        public Action<object> Callback { get; set; }
    }
}
