using System;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Graphics.Effects;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI.Xaml;

namespace Emby.Mobile.Universal.Controls
{
    public sealed class BlurredImageControl : Control
    {
        public static readonly DependencyProperty ImageUrlProperty = DependencyProperty.Register("ImageUrl", typeof(string), typeof(BlurredImageControl), new PropertyMetadata(string.Empty, SourceSet));
        public static readonly DependencyProperty BlurProperty = DependencyProperty.Register("Blur", typeof(float), typeof(BlurredImageControl), new PropertyMetadata(15.0f));

        private CanvasControl control;
        private bool imageLoaded;
        private CanvasBitmap image;
        private ContentPresenter contentPresenter;
        private ScaleEffect scaleEffect;
        private GaussianBlurEffect blurEffect;

        public BlurredImageControl()
        {
            this.DefaultStyleKey = typeof(BlurredImageControl);
        }

        public float Blur
        {
            get { return (float)GetValue(BlurProperty); }
            set { SetValue(BlurProperty, value); }
        }

        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            contentPresenter = GetTemplateChild("imagePresenter") as ContentPresenter;

            RenderImage();
        }

        private static void SourceSet(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.NewValue.ToString()))
            {
                var control = d as BlurredImageControl;
                control.RenderImage();
            }
        }

        private void RenderImage()
        {
            if (contentPresenter != null)
            {
                control = new CanvasControl();
                control.Draw += OnDraw;
                control.CreateResources += OnCreateResources;

                contentPresenter.Content = control;
            }
        }

        private async void OnCreateResources(CanvasControl sender, object args)
        {
            if (!string.IsNullOrWhiteSpace(ImageUrl))
            {
                scaleEffect = new ScaleEffect();
                blurEffect = new GaussianBlurEffect();

                image = await CanvasBitmap.LoadAsync(sender.Device,
                  new Uri(ImageUrl));

                imageLoaded = true;

                sender.Invalidate();
            }
        }

        private void OnDraw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            if (imageLoaded)
            {
                using (var session = args.DrawingSession)
                {
                    session.Units = CanvasUnits.Pixels;

                    double displayScaling = DisplayInformation.GetForCurrentView().LogicalDpi / 96.0;

                    double pixelWidth = sender.ActualWidth * displayScaling;

                    var scalefactor = pixelWidth / image.Size.Width;
                    var vector = new System.Numerics.Vector2((float)scalefactor, (float)scalefactor);

                    scaleEffect.Source = this.image;
                    scaleEffect.Scale = vector;
                    //scaleEffect.Scale = vector;

                    blurEffect.Source = scaleEffect;
                    blurEffect.BlurAmount = Blur;

                    session.DrawImage(blurEffect, 0.0f, 0.0f);
                }
            }
        }
    }
}
