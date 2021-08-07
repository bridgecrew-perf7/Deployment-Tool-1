// Decompiled with JetBrains decompiler
// Type: LoginWindow.PasswordBoxMonitor
// Assembly: LoginWindow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 86830B4E-3982-459F-A1D6-87BC71DF3394
// Assembly location: E:\Work\Companies\Upwork\02-Smart Design Plug-in Updates (12.8$-hr)\Work\01\Smart Design Plug-in Updates\Smart Design Plug-in Updates\Smart Design Plug-in Updates\bin\Debug\LoginWindow.exe

using System.Windows;
using System.Windows.Controls;

namespace LoginWindow
{
  public class PasswordBoxMonitor : DependencyObject
  {
    public static readonly DependencyProperty IsMonitoringProperty = DependencyProperty.RegisterAttached("IsMonitoring", typeof (bool), typeof (PasswordBoxMonitor), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(PasswordBoxMonitor.OnIsMonitoringChanged)));
    public static readonly DependencyProperty PasswordLengthProperty = DependencyProperty.RegisterAttached("PasswordLength", typeof (int), typeof (PasswordBoxMonitor), (PropertyMetadata) new UIPropertyMetadata((object) 0));

    public static bool GetIsMonitoring(DependencyObject obj) => (bool) obj.GetValue(PasswordBoxMonitor.IsMonitoringProperty);

    public static void SetIsMonitoring(DependencyObject obj, bool value) => obj.SetValue(PasswordBoxMonitor.IsMonitoringProperty, (object) value);

    public static int GetPasswordLength(DependencyObject obj) => (int) obj.GetValue(PasswordBoxMonitor.PasswordLengthProperty);

    public static void SetPasswordLength(DependencyObject obj, int value) => obj.SetValue(PasswordBoxMonitor.PasswordLengthProperty, (object) value);

    private static void OnIsMonitoringChanged(
      DependencyObject d,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(d is PasswordBox passwordBox))
        return;
      if ((bool) e.NewValue)
        passwordBox.PasswordChanged += new RoutedEventHandler(PasswordBoxMonitor.PasswordChanged);
      else
        passwordBox.PasswordChanged -= new RoutedEventHandler(PasswordBoxMonitor.PasswordChanged);
    }

    private static void PasswordChanged(object sender, RoutedEventArgs e)
    {
      if (!(sender is PasswordBox passwordBox))
        return;
      PasswordBoxMonitor.SetPasswordLength((DependencyObject) passwordBox, passwordBox.Password.Length);
    }
  }
}
