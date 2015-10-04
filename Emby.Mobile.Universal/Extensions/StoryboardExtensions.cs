using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Emby.Mobile.Universal.Extensions
{
    public static class StoryboardExtensions
    {
        public const double AnimationDuration = 500;

        public static void AddSlideAndFadeInAnimation(this Storyboard storyboard, UIElement element)
        {
            AddVisibleAnimation(storyboard, element);
            AddTransformXExponentialEaseAnim(storyboard, element, 500, 0);
            AddFadeAnim(storyboard, element, 1);
        }
        public static void AddSlideAndFadeOutAnimation(this Storyboard storyboard, UIElement element)
        {
            AddTransformXExponentialEaseAnim(storyboard, element, 0, -500);
            AddFadeAnim(storyboard, element, 0, 500);
            AddCollapseAnimation(storyboard, element);
        }

        public static void AddSlideAndFadeInReverseAnimation(this Storyboard storyboard, UIElement element)
        {
            AddVisibleAnimation(storyboard, element);
            AddTransformXExponentialEaseAnim(storyboard, element, -500, 0);
            AddFadeAnim(storyboard, element, 1);
        }

        public static void AddSlideAndFadeOutReverseAnimation(this Storyboard storyboard, UIElement element)
        {
            AddTransformXExponentialEaseAnim(storyboard, element, 0, 500);
            AddFadeAnim(storyboard, element, 0);
            AddCollapseAnimation(storyboard, element);
        }

        public static void AddFadeAnim(this Storyboard storyboard, UIElement element, double targetOpacity = 1, double duration = AnimationDuration)
        {
            var easing = new QuadraticEase { EasingMode = EasingMode.EaseIn };
            var anim = new DoubleAnimation
            {
                From = double.IsNaN(element.Opacity) ? 0 : element.Opacity,
                To = targetOpacity,
                Duration = TimeSpan.FromMilliseconds(duration),
                EasingFunction = easing,
                AutoReverse = false
            };

            Storyboard.SetTargetProperty(anim, "Opacity");
            Storyboard.SetTarget(anim, element);

            storyboard.Children.Add(anim);
        }

        public static void AddTransformXAnim(this Storyboard storyboard, UIElement element, double fromOffsetX, double toOffsetX, double duration = AnimationDuration)
        {
            AddTransformXAnim(storyboard, element, fromOffsetX, toOffsetX, null, duration);
        }

        public static void AddTransformXExponentialEaseAnim(this Storyboard storyboard, UIElement element, double fromOffsetX, double toOffsetX, double duration = AnimationDuration)
        {
            AddTransformXAnim(storyboard, element, fromOffsetX, toOffsetX, new ExponentialEase { EasingMode = EasingMode.EaseInOut }, duration);
        }

        public static void AddTransformXAnim(this Storyboard storyboard, UIElement element, double fromOffsetX, double toOffsetX, EasingFunctionBase easingFunction, double duration = AnimationDuration)
        {
            var trans = new TranslateTransform { X = 1, Y = 1 };
            element.RenderTransformOrigin = new Point(0.5, 0.5);
            element.RenderTransform = trans;

            var anim = new DoubleAnimation
            {
                From = fromOffsetX,
                To = toOffsetX,
                Duration = TimeSpan.FromMilliseconds(duration),
                EasingFunction = easingFunction,
                AutoReverse = false
            };

            Storyboard.SetTarget(anim, element);
            Storyboard.SetTargetProperty(anim, "(UIElement.RenderTransform).(TranslateTransform.X)");

            storyboard.Children.Add(anim);
        }

        public static void AddTransformYAnim(this Storyboard storyboard, UIElement element, double fromOffsetY, double toOffsetY, double duration = AnimationDuration)
        {
            AddTransformYAnim(storyboard, element, fromOffsetY, toOffsetY, null, duration);
        }

        public static void AddTransformYExponentialEaseAnim(this Storyboard storyboard, UIElement element, double fromOffsetY, double toOffsetY, double duration = AnimationDuration)
        {
            AddTransformYAnim(storyboard, element, fromOffsetY, toOffsetY, new ExponentialEase { EasingMode = EasingMode.EaseInOut }, duration);
        }

        public static void AddTransformYAnim(this Storyboard storyboard, UIElement element, double fromOffsetY, double toOffsetY, EasingFunctionBase easingFunction, double duration = AnimationDuration)
        {
            var trans = new TranslateTransform { X = 1, Y = 1 };
            element.RenderTransformOrigin = new Point(0.5, 0.5);
            element.RenderTransform = trans;

            var anim = new DoubleAnimation
            {
                From = fromOffsetY,
                To = toOffsetY,
                Duration = TimeSpan.FromMilliseconds(duration),
                EasingFunction = easingFunction,
                AutoReverse = false
            };

            Storyboard.SetTarget(anim, element);
            Storyboard.SetTargetProperty(anim, "(UIElement.RenderTransform).(TranslateTransform.Y)");

            storyboard.Children.Add(anim);
        }

        public static void AddVisibleAnimation(this Storyboard storyboard, DependencyObject element, double keyTime = 0)
        {
            var visibility = new ObjectAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.FromMilliseconds(keyTime),
                Duration = new Duration(TimeSpan.FromMilliseconds(0))
            };

            visibility.KeyFrames.Add(new DiscreteObjectKeyFrame
            {
                Value = Visibility.Visible,
                KeyTime = TimeSpan.FromMilliseconds(keyTime)
            });

            Storyboard.SetTargetProperty(visibility, "(FrameworkElement.Visibility)");
            Storyboard.SetTarget(visibility, element);

            storyboard.Children.Add(visibility);
        }

        public static void AddCollapseAnimation(this Storyboard storyboard, DependencyObject element, double keyTime = AnimationDuration)
        {
            var visibility = new ObjectAnimationUsingKeyFrames
            {
                BeginTime = TimeSpan.FromMilliseconds(keyTime),
                Duration = new Duration(TimeSpan.FromMilliseconds(0))
            };

            visibility.KeyFrames.Add(new DiscreteObjectKeyFrame
            {
                Value = Visibility.Collapsed,
                KeyTime = TimeSpan.FromMilliseconds(keyTime)
            });

            Storyboard.SetTargetProperty(visibility, "(FrameworkElement.Visibility)");
            Storyboard.SetTarget(visibility, element);

            storyboard.Children.Add(visibility);
        }

        public static void AddHeightAnim(this Storyboard storyboard, UIElement element, double from, double to, double duration = AnimationDuration)
        {
            var anim = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromMilliseconds(duration),
                EnableDependentAnimation = true,
                AutoReverse = false
            };

            Storyboard.SetTarget(anim, element);
            Storyboard.SetTargetProperty(anim, "(UIElement.Height)");

            storyboard.Children.Add(anim);
        }

        public static void AddWidthAnim(this Storyboard storyboard, UIElement element, double from, double to, double duration = AnimationDuration)
        {
            var anim = new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromMilliseconds(duration),
                EnableDependentAnimation = true,
                AutoReverse = false
            };

            Storyboard.SetTarget(anim, element);
            Storyboard.SetTargetProperty(anim, "(UIElement.Width)");

            storyboard.Children.Add(anim);
        }
    }
}
