using System;
using System.ComponentModel;
using System.Threading.Tasks;
using DomainModelEditor.Data.Abstractions;
using DomainModelEditor.Domain;
using Attribute = DomainModelEditor.Domain.Attribute;

namespace DomainModelEditor.WPF.ViewModels
{
    public class AddAttributeDialogViewModel : BindableBase, IExtensible
    {
        private readonly IUnitOfWork _unitOfWork;
        private AttributeWrapper _attribute;

        public AddAttributeDialogViewModel(IUnitOfWork unitOfWork)
        {
            _attribute = new AttributeWrapper(new Attribute());
            _attribute.PropertyChanged += _attribute_PropertyChanged;
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            SaveAttributeCommand = new CommandHandler(async () => { await SaveAttributeAction(); }, CanExecuteSave);
            SaveAttributeCommand.RaiseCanExecuteChanged();
        }

        public AttributeWrapper Attribute
        {
            get => _attribute;
            set => SetProperty(ref _attribute, value);
        }

        public CommandHandler SaveAttributeCommand { get; }

        public async Task LoadAsync(object parameter)
        {
            //Initialization code should be implemented here.
        }

        public Action Close { get; set; }

        private async Task SaveAttributeAction()
        {
            if (Attribute == null || string.IsNullOrEmpty(Attribute.AttributeName))
                return;
            await _unitOfWork.Attributes.AddAsync(Attribute.Model);
            var res = await _unitOfWork.SaveAsync();
            if (res > 0)
            {
                Close?.Invoke();
            }
        }

        private void _attribute_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SaveAttributeCommand.RaiseCanExecuteChanged();
        }

        private bool CanExecuteSave()
        {
            return !string.IsNullOrEmpty(Attribute.AttributeName) && !Attribute.HasErrors;
        }
    }
}