using DomainModelEditor.Data.Services;
using DomainModelEditor.Domain;
using System;
using System.Threading.Tasks;

namespace DomainModelEditor.ViewModels
{
   public class AddAttributeDialogViewModel : BindableBase,IExtensible
    {
        IUnitOfWork _unitOfWork;
        private AttributeWrapper _attribute;
        public AttributeWrapper Attribute
        {
            get { return _attribute; }
            set { SetProperty(ref _attribute, value); }
        }
        public AddAttributeDialogViewModel(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _attribute = new AttributeWrapper(new DomainModelEditor.Domain.Attribute());
            _attribute.PropertyChanged += _attribute_PropertyChanged;
            _unitOfWork = unitOfWork;
            SaveAttributeCommand = new CommandHandler(async () => { await SaveAttributeAction(); },CanExecuteSave);
            SaveAttributeCommand.RaiseCanExecuteChanged();
        }
        public CommandHandler SaveAttributeCommand { get; private set; }

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
            else
            {
                //Show Message to the user that something wrong happend

            }

        }
        public async Task LoadAsync(object parameter)
        {
            //Initialization code should be implemented here.
        }
        private void _attribute_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SaveAttributeCommand.RaiseCanExecuteChanged();
        }

        public Action Close { get; set; }
        private bool CanExecuteSave()
        {
            return !string.IsNullOrEmpty(Attribute.AttributeName) && !Attribute.HasErrors;
        }
    }
}
