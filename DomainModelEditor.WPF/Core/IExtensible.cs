using System;
using System.Threading.Tasks;

namespace DomainModelEditor
{
    public interface IExtensible
    {
        /// <summary>
        /// Should be implemented by ViewModels that needs to load data during the intialization to avoid making expensive call in constructor.
        /// </summary>
        /// <returns></returns>
        Task LoadAsync(object parameter);
        /// <summary>
        /// Handles closing window from the ViewModel.
        /// </summary>
        Action Close { get; set; }

    }
}