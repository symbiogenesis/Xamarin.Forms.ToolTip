﻿using Android.Views;

using Com.Tooltip;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Effects")]
[assembly: ExportEffect(typeof(Xamarin.Forms.ToolTip.ToolTipImplementation), nameof(Xamarin.Forms.ToolTip.ToolTipEffect))]
namespace Xamarin.Forms.ToolTip
{
    /// <summary>
    /// Interface for Xamarin.Forms.ToolTip
    /// </summary>
    public class ToolTipImplementation : PlatformEffect
    {
        Tooltip.Builder builder;

        void OnTap(object sender, EventArgs e)
        {
            GetToolTip();
        }

        private void GetToolTip()
        {
            var control = Control ?? Container;

            var text = ToolTipEffect.GetText(Element);

            if (!string.IsNullOrEmpty(text))
            {
                var position = ToolTipEffect.GetPosition(Element);
                builder = new Tooltip.Builder(control);
                switch (position)
                {
                    case ToolTipPosition.Top:
                        builder.SetGravity((int)GravityFlags.Top);
                        break;
                    case ToolTipPosition.Left:
                        builder.SetGravity((int)GravityFlags.Left);
                        break;
                    case ToolTipPosition.Right:
                        builder.SetGravity((int)GravityFlags.Right);
                        break;
                    case ToolTipPosition.Bottom:
                        builder.SetGravity((int)GravityFlags.Bottom);
                        break;
                    default:
                        builder.SetGravity((int)GravityFlags.NoGravity);
                        break;
                }

                builder.SetText(text);
                builder.SetCornerRadius(Convert.ToSingle(ToolTipEffect.GetCornerRadius(Element)));

                builder.SetDismissOnClick(true);
                builder.SetBackgroundColor(ToolTipEffect.GetBackgroundColor(Element).ToAndroid());
                builder.SetTextColor(ToolTipEffect.GetTextColor(Element).ToAndroid());
                var heightArrow = ToolTipEffect.GetArrowHeight(Element);
                if (heightArrow > 0.0)
                    builder.SetArrowHeight(Convert.ToSingle(heightArrow));
                var widthArrow = ToolTipEffect.GetArrowWidth(Element);
                if (widthArrow > 0.0)
                    builder.SetArrowWidth(Convert.ToSingle(widthArrow));

                var textSize = ToolTipEffect.GetTextSize(Element);
                if (textSize > 0)
                    builder.SetTextSize(Convert.ToSingle(textSize));

                builder.SetMargin(Convert.ToSingle(ToolTipEffect.GetMargin(Element)));
                builder.SetPadding(ToolTipEffect.GetPadding(Element));
                builder.SetCancelable(true);

                builder.Build().Show();

                //  _toolTipsManager?.Show(toolTipView);
            }
        }

        protected override void OnAttached()
        {
            var control = Control ?? Container;
            control.Click += OnTap;
        }

        protected override void OnDetached()
        {
            var control = Control ?? Container;
            control.Click -= OnTap;
            builder?.Dispose();
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(args);
            if (args.PropertyName == "Text")
            {
                GetToolTip();
            }
        }
    }
}