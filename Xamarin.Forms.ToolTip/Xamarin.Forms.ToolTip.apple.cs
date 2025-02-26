﻿using System;
using System.ComponentModel;

using UIKit;

using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ResolutionGroupName("Effects")]
[assembly: ExportEffect(typeof(Xamarin.Forms.ToolTip.ToolTipImplementation), nameof(Xamarin.Forms.ToolTip.ToolTipEffect))]
namespace Xamarin.Forms.ToolTip
{
    /// <summary>
    /// Interface for Xamarin.Forms.ToolTip
    /// </summary>
    public class ToolTipImplementation : PlatformEffect
    {
        EasyTipView.EasyTipView tooltip;
        UITapGestureRecognizer tapGestureRecognizer;

        public ToolTipImplementation()
        {
            tooltip = new EasyTipView.EasyTipView();
            tooltip.DidDismiss += OnDismiss;
        }

        void OnTap(object sender, EventArgs e)
        {
            var control = Control ?? Container;

            var text = ToolTipEffect.GetText(Element);

            if (!string.IsNullOrEmpty(text))
            {
                tooltip.BubbleColor = ToolTipEffect.GetBackgroundColor(Element).ToUIColor();
                tooltip.ForegroundColor = ToolTipEffect.GetTextColor(Element).ToUIColor();
                tooltip.Text = new Foundation.NSString(text);
                var heightArrow = ToolTipEffect.GetArrowHeight(Element);
                if (heightArrow > 0.0)
                    tooltip.ArrowHeight = Convert.ToSingle(heightArrow);
                var widthArrow = ToolTipEffect.GetArrowWidth(Element);
                if (widthArrow > 0.0)
                    tooltip.ArrowWidth = Convert.ToSingle(widthArrow);
                UpdatePosition();

                var window = UIApplication.SharedApplication.KeyWindow;
                var vc = window.RootViewController;
                while (vc.PresentedViewController != null)
                {
                    vc = vc.PresentedViewController;
                }


                tooltip?.Show(control, vc.View, true);
            }

        }

        void OnDismiss(object sender, EventArgs e)
        {
            // do something on dismiss
        }


        protected override void OnAttached()
        {
            var control = Control ?? Container;

            if (control is UIButton)
            {
                var btn = Control as UIButton;
                btn.TouchUpInside += OnTap;
            }
            else
            {
                tapGestureRecognizer = new UITapGestureRecognizer((UITapGestureRecognizer obj) => OnTap(obj, EventArgs.Empty));
                control.UserInteractionEnabled = true;
                control.AddGestureRecognizer(tapGestureRecognizer);
            }
        }

        protected override void OnDetached()
        {

            var control = Control ?? Container;

            if (control is UIButton)
            {
                var btn = Control as UIButton;
                btn.TouchUpInside -= OnTap;
            }
            else
            {
                if (tapGestureRecognizer != null)

                    control.RemoveGestureRecognizer(tapGestureRecognizer);

            }
            tooltip?.Dismiss();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);

            if (args.PropertyName == ToolTipEffect.BackgroundColorProperty.PropertyName)
            {
                tooltip.BubbleColor = ToolTipEffect.GetBackgroundColor(Element).ToUIColor();
            }
            else if (args.PropertyName == ToolTipEffect.TextColorProperty.PropertyName)
            {
                tooltip.ForegroundColor = ToolTipEffect.GetTextColor(Element).ToUIColor();
            }
            else if (args.PropertyName == ToolTipEffect.TextProperty.PropertyName)
            {
                tooltip.Text = new Foundation.NSString(ToolTipEffect.GetText(Element));
            }
            else if (args.PropertyName == ToolTipEffect.ArrowWidthProperty.PropertyName)
            {
                var widthArrow = ToolTipEffect.GetArrowWidth(Element);
                tooltip.ArrowWidth = Convert.ToSingle(widthArrow);
            }
            else if (args.PropertyName == ToolTipEffect.ArrowHeightProperty.PropertyName)
            {
                var heightArrow = ToolTipEffect.GetArrowHeight(Element);
                tooltip.ArrowWidth = Convert.ToSingle(heightArrow);
            }

            else if (args.PropertyName == ToolTipEffect.PositionProperty.PropertyName)
            {
                UpdatePosition();
            }
        }

        void UpdatePosition()
        {
            var position = ToolTipEffect.GetPosition(Element);
            switch (position)
            {
                case ToolTipPosition.Top:
                    tooltip.ArrowPosition = EasyTipView.ArrowPosition.Bottom;
                    break;
                case ToolTipPosition.Left:
                    tooltip.ArrowPosition = EasyTipView.ArrowPosition.Right;
                    break;
                case ToolTipPosition.Right:
                    tooltip.ArrowPosition = EasyTipView.ArrowPosition.Left;
                    break;
                default:
                    tooltip.ArrowPosition = EasyTipView.ArrowPosition.Top;
                    break;
            }
        }
    }
}
