using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DawCommunication
{
    public static class Extensions
    {
        public static Action<string>Disp(this Dispatcher dispatcher, Action<string> action, string message)
        {
            return (s) => dispatcher.Invoke(() => action.Invoke(message));
        }
    }
}
