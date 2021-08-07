// Decompiled with JetBrains decompiler
// Type: LoginWindow.MainWindow
// Assembly: LoginWindow, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 86830B4E-3982-459F-A1D6-87BC71DF3394
// Assembly location: E:\Work\Companies\Upwork\02-Smart Design Plug-in Updates (12.8$-hr)\Work\01\Smart Design Plug-in Updates\Smart Design Plug-in Updates\Smart Design Plug-in Updates\bin\Debug\LoginWindow.exe

using LoginWindow.Properties;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace LoginWindow
{
  public partial class MainWindow : Window
  {
    public string UserName;
    public string Password;

    public MainWindow()
    {
      InitializeComponent();
      if (Settings.Default.UserName != string.Empty)
      {
        this.LocalUserNameTextBox.Text = Settings.Default.UserName;
        this.LocalPasswordBox.Password = Settings.Default.Passswrod;
        this.UserName = Settings.Default.UserName;
        this.Password = Settings.Default.Passswrod;
        this.UserNameText.Visibility = Visibility.Hidden;
        this.PasswordText.Visibility = Visibility.Hidden;
      }
      this.RememberCheckBox.IsChecked = new bool?(true);
    }

    private void CheckBoxChanged(object sender, RoutedEventArgs e)
    {
      bool? isChecked1 = this.RememberCheckBox.IsChecked;
      bool flag1 = true;
      if (isChecked1.GetValueOrDefault() == flag1 & isChecked1.HasValue)
      {
        Settings.Default.UserName = this.LocalUserNameTextBox.Text;
        Settings.Default.Passswrod = this.LocalPasswordBox.Password;
        Settings.Default.Save();
      }
      bool? isChecked2 = this.RememberCheckBox.IsChecked;
      bool flag2 = false;
      if (!(isChecked2.GetValueOrDefault() == flag2 & isChecked2.HasValue))
        return;
      Settings.Default.UserName = "";
      Settings.Default.Passswrod = "";
      Settings.Default.Save();
    }

    private void BtnActionMinimize_OnClick(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;


    private void btnActionClose_Click(object sender, RoutedEventArgs e) => this.Close();

    private void LocalLoginButton_Click(object sender, RoutedEventArgs e)
    {
      bool? isChecked1 = this.ShowCheckBox.IsChecked;
      bool flag1 = true;
      string str = !(isChecked1.GetValueOrDefault() == flag1 & isChecked1.HasValue) ? this.LocalPasswordBox.Password : this.passwordTxtBox.Text;
      bool? isChecked2 = this.RememberCheckBox.IsChecked;
      bool flag2 = true;
      if (isChecked2.GetValueOrDefault() == flag2 & isChecked2.HasValue)
      {
        Settings.Default.UserName = this.LocalUserNameTextBox.Text;
        Settings.Default.Passswrod = str;
        Settings.Default.Save();
      }
      isChecked2 = this.RememberCheckBox.IsChecked;
      bool flag3 = false;
      if (isChecked2.GetValueOrDefault() == flag3 & isChecked2.HasValue)
      {
        Settings.Default.UserName = "";
        Settings.Default.Passswrod = "";
        Settings.Default.Save();
      }
      this.UserName = this.LocalUserNameTextBox.Text;
      this.Password = str;
      this.Close();
    }

    private void ShowCheckBox_Checked(object sender, RoutedEventArgs e)
    {
      this.passwordTxtBox.Text = this.LocalPasswordBox.Password;
      this.LocalPasswordBox.Visibility = Visibility.Collapsed;
      this.passwordTxtBox.Visibility = Visibility.Visible;
    }

    private void ShowCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
      this.LocalPasswordBox.Password = this.passwordTxtBox.Text;
      this.passwordTxtBox.Visibility = Visibility.Collapsed;
      this.LocalPasswordBox.Visibility = Visibility.Visible;
    }

    private void LocalPasswordBox_GotFocus(object sender, RoutedEventArgs e) => this.PasswordText.Visibility = Visibility.Hidden;

    private void PasswordText_GotFocus(object sender, RoutedEventArgs e) => this.PasswordText.Visibility = Visibility.Hidden;

    private void passwordTxtBox_GotFocus(object sender, RoutedEventArgs e) => this.PasswordText.Visibility = Visibility.Hidden;

    private void PasswordText_LostFocus(object sender, RoutedEventArgs e)
    {
      if (!string.IsNullOrEmpty(this.passwordTxtBox.Text) || !string.IsNullOrEmpty(this.LocalPasswordBox.Password))
        return;
      this.PasswordText.Visibility = Visibility.Visible;
    }

    private void LocalPasswordBox_LostFocus(object sender, RoutedEventArgs e)
    {
      if (!string.IsNullOrEmpty(this.passwordTxtBox.Text) || !string.IsNullOrEmpty(this.LocalPasswordBox.Password))
        return;
      this.PasswordText.Visibility = Visibility.Visible;
    }

    private void passwordTxtBox_LostFocus(object sender, RoutedEventArgs e)
    {
      if (!string.IsNullOrEmpty(this.passwordTxtBox.Text) || !string.IsNullOrEmpty(this.LocalPasswordBox.Password))
        return;
      this.PasswordText.Visibility = Visibility.Visible;
    }





  }
}
