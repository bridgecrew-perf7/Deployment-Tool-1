// Decompiled with JetBrains decompiler
// Type: LoginWindow.TextInputToVisibilityConverter
// Assembly: LoginWindow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 86830B4E-3982-459F-A1D6-87BC71DF3394
// Assembly location: E:\Work\Companies\Upwork\02-Smart Design Plug-in Updates (12.8$-hr)\Work\01\Smart Design Plug-in Updates\Smart Design Plug-in Updates\Smart Design Plug-in Updates\bin\Debug\LoginWindow.exe

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LoginWindow
{
  public class TextInputToVisibilityConverter : IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      if (values[0] is bool && values[1] is bool)
      {
        bool flag = !(bool) values[0];
        if ((bool) values[1] | flag)
          return (object) Visibility.Collapsed;
      }
      return (object) Visibility.Visible;
    }

    public object[] ConvertBack(
      object value,
      Type[] targetTypes,
      object parameter,
      CultureInfo culture)
    {
      throw new NotImplementedException();
    }
  }
}
