﻿using System.ComponentModel;
using System.Windows.Input;

namespace Emby.Mobile.Core.Interfaces
{
    public interface ICanLogin : INotifyPropertyChanged
    {
        string Username { get; set; }
        string Password { get; set; }
        bool CanSignIn { get; }
        ICommand SignInCommand { get; }
        bool IsEmbyConnect { get; }
    }
}
