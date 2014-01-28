using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Common.UI.CustomControl
{
    /// <summary>
    /// Кастомный ContentControl с анимированным переключением контента.
    /// </summary>
    /// <remarks>
    /// Контрол позаимствован с http://www.codeproject.com/Articles/136786/Creating-an-Animated-ContentControl
    /// -- ВНИМАНИЕ ОГРАНИЧЕНИЯ В ИСПОЛЬЗОВАНИИ --
    /// ВНИМАНИЕ: контрол нормально работает только если к нему применён стиль из /Common.UI;component/View/Styles/AnimatedContentControlStyleDictionary.xaml.
    /// ВНИМАНИЕ: контрол не умеет отображать свой первоначальный контент при загрузке из-за того, 
    /// что OnContentChanged вызывается до OnApplyTemplate. 
    /// Поэтому для отображения первоначального контента необходимо после загрузки руками вызвать RaisePropertyChanged на поле привязки контента.
    /// Удобно использовать для вызов команды в EventTrigger'е(из System.Windows.Interactivity) на событие Loaded у View.
    /// ------------------------------------------
    /// Изначально контрол листал контент справа-налево: старый контент уходил за левый край, новый появлялся из-за правого края.
    /// Функциональность дополнена свойством AnimatedDirection для определения направления перелистывания.
    /// Теперь с помощью dependency property AnimatedDirectionProperty можно задавать направление перелистывания.
    /// AnimatedDirection.Forward - листает справа-налево.
    /// AnimatedDirection.Backward - листает слева-направо.
    /// Если ничего не привязывать к AnimatedDirection, то листать будет справа-налево.
    /// </remarks>
    [TemplatePart(Name = "PART_PaintArea", Type = typeof(Shape))]
    [TemplatePart(Name = "PART_MainContent", Type = typeof(ContentPresenter))]
    public class AnimatedContentControl : ContentControl
    {
        #region Generated static constructor

        /// <summary>
        /// Кастомный ContentControl с анимированным переключением контента.
        /// </summary>
        static AnimatedContentControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(AnimatedContentControl), new FrameworkPropertyMetadata(typeof(AnimatedContentControl)));
        }

        #endregion

        private Shape m_paintArea;

        private ContentPresenter m_mainContent;

        /// <summary>
        /// This gets called when the template has been applied and we have our visual tree
        /// </summary>
        public override void OnApplyTemplate()
        {
            this.m_paintArea = this.Template.FindName("PART_PaintArea", this) as Shape;
            this.m_mainContent = this.Template.FindName("PART_MainContent", this) as ContentPresenter;

            base.OnApplyTemplate();
        }

        /// <summary>
        /// This gets called when the content we're displaying has changed
        /// </summary>
        /// <param name="oldContent">The content that was previously displayed</param>
        /// <param name="newContent">The new content that is displayed</param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (this.m_paintArea != null && this.m_mainContent != null)
            {
                this.m_paintArea.Fill = this.CreateBrushFromVisual(this.m_mainContent);
                this.BeginAnimateContentReplacement();
            }
            base.OnContentChanged(oldContent, newContent);
        }

        /// <summary>
        /// Starts the animation for the new content
        /// </summary>
        private void BeginAnimateContentReplacement()
        {
            var newContentTransform = new TranslateTransform();
            var oldContentTransform = new TranslateTransform();
            this.m_paintArea.RenderTransform = oldContentTransform;
            this.m_mainContent.RenderTransform = newContentTransform;
            this.m_paintArea.Visibility = Visibility.Visible;

            // Определяем направление перелистывания контента
            int dirFactor = AnimatedDirection == AnimatedDirection.Forward ? 1 : -1;

            newContentTransform.BeginAnimation(TranslateTransform.XProperty, this.CreateAnimation(dirFactor * this.ActualWidth, 0));
            oldContentTransform.BeginAnimation(
                TranslateTransform.XProperty,
                this.CreateAnimation(0, (-dirFactor) * this.ActualWidth, (s, e) => this.m_paintArea.Visibility = Visibility.Hidden));
        }

        /// <summary>
        /// Creates the animation that moves content in or out of view.
        /// </summary>
        /// <param name="from">The starting value of the animation.</param>
        /// <param name="to">The end value of the animation.</param>
        /// <param name="whenDone">(optional) A callback that will be called when the animation has completed.</param>
        private AnimationTimeline CreateAnimation(double from, double to, EventHandler whenDone = null)
        {
            IEasingFunction ease = new BackEase { Amplitude = 0.1, EasingMode = EasingMode.EaseOut };
            var duration = new Duration(TimeSpan.FromSeconds(0.5));
            var anim = new DoubleAnimation(from, to, duration) { EasingFunction = ease };
            if (whenDone != null) anim.Completed += whenDone;
            anim.Freeze();
            return anim;
        }

        /// <summary>
        /// Creates a brush based on the current appearnace of a visual element. The brush is an ImageBrush and once created, won't update its look
        /// </summary>
        /// <param name="v">The visual element to take a snapshot of</param>
        private Brush CreateBrushFromVisual(Visual v)
        {
            if (v == null) throw new ArgumentNullException("v");
            var target = new RenderTargetBitmap(
                (int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Pbgra32);
            target.Render(v);
            var brush = new ImageBrush(target);
            brush.Freeze();
            return brush;
        }

        /// <summary>
        /// Свойство зависимости для привязки направления перелистывания контента
        /// </summary>
        public static readonly DependencyProperty AnimatedDirectionProperty = DependencyProperty.Register("AnimatedDirection", typeof(AnimatedDirection), typeof(AnimatedContentControl));

        /// <summary>
        /// Возвращает или устанваливает направление перелистывания контента
        /// </summary>
        public AnimatedDirection AnimatedDirection
        {
            get
            {
                return (AnimatedDirection)this.GetValue(AnimatedDirectionProperty);
            }
            set
            {
                this.SetValue(AnimatedDirectionProperty, value);
            }
        }
    }

    /// <summary>
    /// Перечисление направлений перелистывания
    /// </summary>
    public enum AnimatedDirection
    {
        /// <summary>
        /// Вперёд
        /// </summary>
        Forward,

        /// <summary>
        /// Назад
        /// </summary>
        Backward
    }
}
