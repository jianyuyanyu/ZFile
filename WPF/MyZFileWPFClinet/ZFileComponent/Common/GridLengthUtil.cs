﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ZFileComponent
{
    internal class GridLengthUtil
    {
        private static TypeConverter _tcGridLength;
        private static TypeConverter _tcDataGridLength;


        static GridLengthUtil()
        {
            _tcGridLength = TypeDescriptor.GetConverter(typeof(GridLength));
            _tcDataGridLength = TypeDescriptor.GetConverter(typeof(DataGridLength));
        }

        public static GridLength ConvertToGridLength(string widthOrHeight)
        {
            try
            {
                return (GridLength)_tcGridLength.ConvertFromString(widthOrHeight);
            }
            catch
            {
                return new GridLength(1, GridUnitType.Auto);
            }
        }

        public static DataGridLength ConvertToDataGridLength(string widthOrHeight)
        {
            try
            {
                return (DataGridLength)_tcDataGridLength.ConvertFromString(widthOrHeight);
            }
            catch (Exception ex)
            {
                return new DataGridLength(1, DataGridLengthUnitType.Auto);
            }
        }
    }
}
