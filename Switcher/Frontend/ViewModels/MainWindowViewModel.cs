using System;
using System.Reactive;
using ReactiveUI;

namespace Switcher.Frontend.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ReactiveCommand<Unit, Unit> CloseButton { get; }

        public MainWindowViewModel()
        {
            CloseButton = ReactiveCommand.Create(CloseButtonFunction);
        }

        private void CloseButtonFunction()
        {
            Environment.Exit(0);
        }
    }
}