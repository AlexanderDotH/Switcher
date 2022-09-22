using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Switcher.Frontend.Controls.Model;

public class ToggleAnimation : TemplatedControl
{
    public static readonly StyledProperty<bool> ActivatedProperty =
        AvaloniaProperty.Register<ToggleAnimation, bool>(nameof(Activated));
    
    public static readonly StyledProperty<Brush> FillColorBrushProperty =
        AvaloniaProperty.Register<ToggleAnimation, Brush>(nameof(FillColorBrush));
    
    public static readonly StyledProperty<Brush> StrokeColorBrushProperty =
        AvaloniaProperty.Register<ToggleAnimation, Brush>(nameof(StrokeColorBrush));
    
    public bool Activated
    {
        get => GetValue(ActivatedProperty);
        set
        {
            SetValue(ActivatedProperty, value);
            
            if (Activated)
            {
                FillColorBrush = new SolidColorBrush(Color.FromArgb(180, 152, 255, 152));
                StrokeColorBrush = new SolidColorBrush(Color.FromArgb(255, 152, 255, 152));
            }
            else
            {
                FillColorBrush = new SolidColorBrush(Color.FromArgb(180, 255, 255, 255));
                StrokeColorBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
        }
    }
    
    public Brush FillColorBrush
    {
        get => GetValue(FillColorBrushProperty);
        set => SetValue(FillColorBrushProperty, value);
    }
    
    public Brush StrokeColorBrush
    {
        get => GetValue(StrokeColorBrushProperty);
        set => SetValue(StrokeColorBrushProperty, value);
    }
}