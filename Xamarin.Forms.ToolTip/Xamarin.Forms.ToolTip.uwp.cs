using System.ComponentModel;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ResolutionGroupName("Effects")]
[assembly: ExportEffect(typeof(Xamarin.Forms.ToolTip.ToolTipImplementation), nameof(Xamarin.Forms.ToolTip.ToolTipEffect))]
namespace Xamarin.Forms.ToolTip
{
    /// <summary>
    /// Interface for Xamarin.Forms.ToolTip
    /// </summary> 
    public class ToolTipImplementation : PlatformEffect
    {
        private static readonly ViewToRendererConverter _viewToRendererConverter = new();

        //Action Action;

        protected override void OnAttached()
        {
            //Action = ToolTipEffect.GetAction(Element);
            //if (Action == Action.OnClick)
            //{
            //    var control = Control ?? Container;

            //    if (control is Windows.UI.Xaml.Controls.Button btn)
            //    {
            //        btn.Click += OnClick;
            //    }
            //    else
            //    {
            //        control.AddHandler(UIElement.TappedEvent, new TappedEventHandler(OnClick), true);
            //    }
            //}
            //else
            ShowToolTip();
        }

        //private void OnClick(object sender, RoutedEventArgs e)
        //{
        //    Popup codePopup = new Popup();
        //    TextBlock popupText = new TextBlock();
        //    popupText.Text = ToolTipEffect.GetText(Element);

        //    popupText.Foreground = XamarinColorToNative(ToolTipEffect.GetTextColor(Element));
        //    Border border = new Border() { Background = XamarinColorToNative(ToolTipEffect.GetBackgroundColor(Element)) };
        //    border.Child = popupText;
        //    codePopup.Child = border;
        //    codePopup.IsOpen = true;

        //}
        Windows.UI.Xaml.Controls.ToolTip toolTip;
        private void ShowToolTip()
        {
            var control = Control ?? Container;

            if (control is not null)
            {
                object toolTipContent;
                var content = ToolTipEffect.GetContent(Element);

                if (content != null)
                {
                    toolTipContent = _viewToRendererConverter.Convert(content, null, null, null);
                }
                else
                {
                    toolTipContent = ToolTipEffect.GetText(Element);
                }
                toolTip = new Windows.UI.Xaml.Controls.ToolTip
                {
                    Background = XamarinColorToNative(ToolTipEffect.GetBackgroundColor(Element)),
                    Content = toolTipContent ?? "n/a",
                    Placement = GetPlacementMode()
                };

                var height = ToolTipEffect.GetHeight(Element);
                if (height > 0.0)
                    toolTip.Height = height;

                var width = ToolTipEffect.GetWidth(Element);
                if (width > 0.0)
                    toolTip.Width = width;

                ToolTipService.SetToolTip(control, toolTip);

                PlacementMode GetPlacementMode()
                {
                    switch (ToolTipEffect.GetPosition(Element))
                    {
                        case ToolTipPosition.Bottom:
                            return PlacementMode.Bottom;
                        case ToolTipPosition.Top:
                            return PlacementMode.Top;
                        case ToolTipPosition.Left:
                            return PlacementMode.Left;
                        case ToolTipPosition.Right:
                            return PlacementMode.Right;
                        default:
                            return PlacementMode.Mouse;
                    }
                }
            }
        }

        private void ToolTipTapped(object sender, TappedRoutedEventArgs e)
        {
            toolTip.IsOpen = false;
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == "Text")
            {
                if (toolTip != null)
                {
                    toolTip.Content = ToolTipEffect.GetText(Element);
                }
            }
        }

        protected override void OnDetached()
        {
            //if (Action == Action.OnClick)
            //{
            //    var control = Control ?? Container;
            //    control.RemoveHandler(UIElement.TappedEvent, new TappedEventHandler(OnClick));
            //}
        }

        private SolidColorBrush XamarinColorToNative(Xamarin.Forms.Color color)
        {
            var alpha = (byte)(color.A * 255);
            var red = (byte)(color.R * 255);
            var green = (byte)(color.G * 255);
            var blue = (byte)(color.B * 255);
            return new SolidColorBrush(Windows.UI.Color.FromArgb(alpha, red, green, blue));
        }
    }
}
