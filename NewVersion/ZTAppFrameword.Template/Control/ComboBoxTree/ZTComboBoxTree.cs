﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ZTAppFrameword.Template.Control.ComboBoxTree
{
    [TemplatePart(Name = "ParentPanel", Type = typeof(Grid))]
    [TemplatePart(Name = "ActualTreeView", Type = typeof(TreeView))]
    public class ZTComboBoxTree: ItemsControl
    {
    }
}
