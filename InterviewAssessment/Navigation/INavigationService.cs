using System;
using System.Threading.Tasks;

namespace DomainModelEditor.Navigation
{
    public interface INavigationService
    {
        void Configure(string key, Type windowType);
        Task ShowAsync(string windowKey, object parameter = null);
        Task<bool?> ShowDialogAsync(string windowKey, object parameter = null);
    }
}