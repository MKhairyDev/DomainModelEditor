using InterviewAssessment.Data.Services;
using System;
using System.Threading.Tasks;

namespace DomainModelEditor.ViewModels
{
   public class AddAttributeDialogViewModel : BindableBase,IExtensible
    {
        IUnitOfWork _unitOfWork;
        private InterviewAssessment.Domain.Attribute _attribute;
        public InterviewAssessment.Domain.Attribute Attribute
        {
            get { return _attribute; }
            set { SetProperty(ref _attribute, value); }
        }
        public AddAttributeDialogViewModel(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _attribute = new InterviewAssessment.Domain.Attribute();
            _unitOfWork = unitOfWork;
            SaveAttributeCommand = new CommandHandler(async () => { await SaveAttributeAction(); });

        }

        public CommandHandler SaveAttributeCommand { get; private set; }

        private async Task SaveAttributeAction()
        {
            if (Attribute == null || string.IsNullOrEmpty(Attribute.AttributeName))
                return;
            await _unitOfWork.Attributes.AddAsync(Attribute);
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
        public Action Close { get; set; }
    }
}
