﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component
{

    #region LaodingStyleEnum
    public enum LoadingStyle
    {
        Standard,
        Wave,
        Ring,
        Ring2,
        Classic
    }
    #endregion

    public enum MenuStyle
    {
        Standard,
        Modern,
    }

    #region CheckBox
    public enum CheckBoxStyle
    {
        Standard,
        Switch,
        Switch2,
        Button,
        Selector,
    }
    #endregion

    #region ProgressBarStyle
    public enum ProgressBarStyle
    {
        Standard,
        Ring
    }
    #endregion

    #region MessageBoxStyle
    public enum MessageBoxStyle
    {
        Standard,
        Modern,
        Classic,
        //Poster
    }

    public enum MessageBoxIcon
    {
        None,
        Info,
        Success,
        Error,
        Warning,
        Question,
    }

    public enum DefaultButton
    {
        None,
        YesOK,
        NoOrCancel,
        CancelOrNo,
    }
    #endregion

    #region Button
    public enum ButtonStyle
    {
        Standard,
        Hollow,
        Outline,
        Link,
    }

    public enum ClickStyle
    {
        None,
        Sink,
    }
    #endregion

    #region TreeView
    public enum TreeViewStyle
    {
        Standard,
        Classic,
        Modern,
        Chain,
    }

    public enum SelectMode
    {
        Any,
        ChildOnly,
        Disabled
    }

    public enum ExpandMode
    {
        DoubleClick,
        SingleClick
    }

    public enum ExpandBehaviour
    {
        Any,
        OnlyOne,
    }
    #endregion
}
