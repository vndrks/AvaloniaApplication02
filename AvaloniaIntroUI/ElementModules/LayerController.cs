using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using System;
using System.Configuration;
using AdornerLayer = Avalonia.Controls.Primitives.AdornerLayer;

namespace AvaloniaIntroUI.ElementModules
{
    public class LayerController : Control
    {
        private double _LeftOffset;
        private double _TopOffset;

        private readonly Rectangle _Child;

        public double LeftOffset
        {
            get { return _LeftOffset;  }
            set
            {
                _LeftOffset = value;
                UpdatePosition();
            }
        }

        public double TopOffset
        {
            get { return _TopOffset; }
            set
            {
                _TopOffset = value;
                UpdatePosition();
            }
        }

        public LayerController(Control control)
        {
            var brush = new VisualBrush(control);
            
            _Child = new Rectangle
            {
                Width = control.Bounds.Width,
                Height = control.Bounds.Height
            };

            var animation = new Animation
            {
                Duration = TimeSpan.FromSeconds(1),
                PlaybackDirection = PlaybackDirection.Reverse,
                IterationCount = IterationCount.Infinite,

                //Children =
                //{
                //    new KeyFrame
                //    {
                //        Cue = new Cue(0),
                //        Setters = { new Setter(Button.OpacityProperty, 0.3) }
                //    },
                //    new KeyFrame
                //    {
                //        Cue = new Cue(1),
                //        Setters = { new Setter(Button.OpacityProperty, 1) }
                //    }
                //}
            };

            // Get the new composition visual
            CompositionVisual compositionVisual = ElementComposition.GetElementVisual(control);
            Compositor compositor = compositionVisual.Compositor;

            // var scale = compositor.CreateVector3KeyFrameAnimation();

            var scale = compositor.CreateDoubleKeyFrameAnimation();

            scale.Duration = TimeSpan.FromSeconds(1);
            scale.Direction = PlaybackDirection.Reverse;
            scale.IterationBehavior = AnimationIterationBehavior.Forever;

            scale.InsertKeyFrame((float)0.2, 1);

            //scale.InsertKeyFrame(0, new Vector3(1, 1, 0));
            //scale.InsertKeyFrame(0.5f, new Vector3(1.5f, 1.5f, 0));
            //scale.InsertKeyFrame(1, new Vector3(1, 1, 0));

            //compositionVisual.StartAnimation("Opacity", scale);

            _Child.Fill = brush;
        }

        private void UpdatePosition()
        {

            //var adornerLayer = Parent as AdornerLayer;
            //adornerLayer?.Update(AdornedElement);
        }
    }
}
