using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace DomainModelEditor.Navigation
{
    public class NavigationService : INavigationService
    {
        private Dictionary<string, Type> windows { get; } = new Dictionary<string, Type>();

        private readonly IServiceProvider serviceProvider;

        public void Configure(string key, Type windowType)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (windowType == null )
                throw new ArgumentNullException(nameof(key));
            if (windows.ContainsKey(key))
                throw new ArgumentException("Window with this key has been registered already");

            windows.Add(key, windowType);
        }

        public NavigationService(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));
            this.serviceProvider = serviceProvider;
        }

        public async Task ShowAsync(string windowKey, object parameter = null)
        {
            if (string.IsNullOrEmpty(windowKey))
                throw new ArgumentNullException(nameof(windowKey));

            var window = await GetAndActivateWindowAsync(windowKey, parameter);
            window.Show();
        }

        public async Task<bool?> ShowDialogAsync(string windowKey, object parameter = null)
        {
            if (string.IsNullOrEmpty(windowKey))
                throw new ArgumentNullException(nameof(windowKey));

            var window = await GetAndActivateWindowAsync(windowKey, parameter);
            return window.ShowDialog();
        }

        private async Task<Window> GetAndActivateWindowAsync(string windowKey, object parameter = null)
        {
            if (!windows.ContainsKey(windowKey))
                throw new ArgumentException("No window has been registered with this key, Please register it first");

            var window = serviceProvider.GetRequiredService(windows[windowKey]) as Window;

            if (window.DataContext is IExtensible loadable)
            {
                await loadable.LoadAsync(parameter);
                loadable.Close += () =>
                {
                    window.Close();
                };
            }
            return window;
        }
    }
}
