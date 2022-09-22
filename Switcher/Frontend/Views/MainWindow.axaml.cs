using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Switcher.Backend.Helper;
using Switcher.Frontend.Controls.Model;
using Switcher.Utils;

namespace Switcher.Frontend.Views
{
    public partial class MainWindow : Window
    {
        private ToggleAnimation _toggleAnimation;
        private ToggleSwitch _toggleSwitch;

        private bool _canBeChanged;
        
        public MainWindow()
        {
            InitializeComponent();

            this._canBeChanged = false;

            this._toggleAnimation = this.Get<ToggleAnimation>(nameof(CTRL_ToggleAnimation));
            this._toggleSwitch = this.Get<ToggleSwitch>(nameof(CTRL_Switch));

            bool isSet = AdGuardHelper.IsDNSSet();

            this._toggleSwitch.IsChecked = isSet;
            this._toggleAnimation.Activated = isSet;
            
            this._canBeChanged = true;
        }

        private void ToggleButton_OnChecked(object? sender, RoutedEventArgs e)
        {
            if (!this._canBeChanged)
                return;
            
            Task task = Task.Factory.StartNew(async () =>
            {
                await AdGuardHelper.SetDNSServer();
            });
            this._toggleAnimation.Activated = true;
        }

        private void ToggleButton_OnUnchecked(object? sender, RoutedEventArgs e)
        {
            if (!this._canBeChanged)
                return;
            
            Task task = Task.Factory.StartNew(async () =>
            {
                await AdGuardHelper.DeleteDNSServer();
            });
            this._toggleAnimation.Activated = false;
        }

        private void InputElement_OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            this.BeginMoveDrag(e);
        }
    }
}