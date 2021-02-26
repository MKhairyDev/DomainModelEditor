using interview_assessment;
using Microsoft.Extensions.DependencyInjection;

namespace DomainModelEditor.ViewModels
{
    public class ViewModelLocator
    {
        public MainWindowViewModel MainViewModel => App.ServiceProvider.GetRequiredService<MainWindowViewModel>();
        public AddEntityDialogViewModel AddEntityViewModel => App.ServiceProvider.GetRequiredService<AddEntityDialogViewModel>();
        public AddAttributeDialogViewModel AttributeViewModel => App.ServiceProvider.GetRequiredService<AddAttributeDialogViewModel>();
        public EntityAttributeDialogViewModel EntityAttributeViewModel => App.ServiceProvider.GetRequiredService<EntityAttributeDialogViewModel>();

    }
}
