using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TestGraphical
{
    public static class Extensions
    {
        public static UIElement GetUIEOfType(this UIElement userControl, Type type)
        {
            if (userControl == null)
                return new UIElement();

            //if (!type.IsAssignableFrom(typeof(UIElement)))
            //    return new UIElement();

            FrameworkElement currentElement = userControl as FrameworkElement;

            for (int i = 0; i < 100; i++)
            {

                currentElement = currentElement.Parent as FrameworkElement;

                if (currentElement.GetType() == type)
                    break;
            }

            return currentElement;
        }
    }
}
