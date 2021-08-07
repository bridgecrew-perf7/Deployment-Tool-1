// Decompiled with JetBrains decompiler
// Type: LoginWindow.ValueConverter
// Assembly: LoginWindow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 86830B4E-3982-459F-A1D6-87BC71DF3394
// Assembly location: E:\Work\Companies\Upwork\02-Smart Design Plug-in Updates (12.8$-hr)\Work\01\Smart Design Plug-in Updates\Smart Design Plug-in Updates\Smart Design Plug-in Updates\bin\Debug\LoginWindow.exe

using System;
using System.Globalization;
using System.Windows.Data;

namespace LoginWindow
{
  internal class ValueConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is bool flag ? (object) !flag : value;

    public object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return value is bool flag ? (object) !flag : value;
    }
  }
}
