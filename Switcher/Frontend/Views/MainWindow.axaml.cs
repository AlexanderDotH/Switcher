using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Switcher.Backend.Handler;
using Switcher.Backend.Helper;
using Switcher.Backend.Structs;
using Switcher.Frontend.Controls.Model;
using Switcher.Utils;
using ComboBox = Avalonia.Controls.ComboBox;

namespace Switcher.Frontend.Views
{
    public partial class MainWindow : Window
    {
        private ToggleAnimation _toggleAnimation;
        private ToggleSwitch _toggleSwitch;
        private ComboBox _serverSelector;

        private bool _canBeChanged;
        
        public MainWindow()
        {
            InitializeComponent();

            this._canBeChanged = false;

            this._toggleAnimation = this.Get<ToggleAnimation>(nameof(CTRL_ToggleAnimation));
            this._toggleSwitch = this.Get<ToggleSwitch>(nameof(CTRL_Switch));

            bool isSet = AdGuardHelper.IsDNSSet();

            this._serverSelector = this.Get<ComboBox>(nameof(CMBO_Family));

            EnumServerType server = EnumServerType.Default;

            if (SettingsHandler.Instance.Settings.ServerType != null)
                server = SettingsHandler.Instance.Settings.ServerType;

            switch (server)
            {
                case EnumServerType.Family:
                    this._serverSelector.SelectedIndex = 1;
                    break;
                
                case EnumServerType.No_Filter:
                    this._serverSelector.SelectedIndex = 2;
                    break;
                
                default:
                    this._serverSelector.SelectedIndex = 0;
                    break;
            }
            
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