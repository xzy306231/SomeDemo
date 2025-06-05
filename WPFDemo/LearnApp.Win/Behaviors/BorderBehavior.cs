using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;
using System.Windows.Media;

namespace LearnApp.Win.Behaviors
{
    public class BorderBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DragEnter += AssociatedObject_DragEnter;
            AssociatedObject.DragLeave += AssociatedObject_DragLeave;
        }

        private void AssociatedObject_DragLeave(object sender, DragEventArgs e)
        {
            var element = sender as FrameworkElement;
            element.Effect = (Effect)new DropShadowEffect() { Color = Colors.Transparent, ShadowDepth = 0 };
        }

        private void AssociatedObject_DragEnter(object sender, DragEventArgs e)
        {
            var element = sender as FrameworkElement;
            element.Effect = (Effect)new DropShadowEffect() { Color = Colors.Red, ShadowDepth = 0 };
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DragEnter -= AssociatedObject_DragEnter;
            AssociatedObject.DragLeave -= AssociatedObject_DragLeave;
        }
    }
}
