using System;
using System.Threading.Tasks;
using DomainModelEditor.Data.Abstractions;
using DomainModelEditor.Domain;
using DomainModelEditor.Models;
using Microsoft.Extensions.Options;

namespace DomainModelEditor.WPF.ViewModels
{
    public class AddEntityDialogViewModel : BindableBase,IExtensible
    {
        private readonly IUnitOfWork _unitOfWork;
        private string _entityName;
        private readonly AppSettings _settings;
        public AddEntityDialogViewModel(IUnitOfWork unitOfWork, IOptions<AppSettings> options)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _unitOfWork = unitOfWork;
            AddNewEntityCommand = new CommandHandler(async () => await AddEntityAction(),()=> !string.IsNullOrEmpty(EntityName));
            CancelCommand = new CommandHandler(CancelAction);
            _settings = options.Value;
        }
        public string EntityName
        {
            get => _entityName;
            set { 
                SetProperty(ref _entityName, value);
                AddNewEntityCommand.RaiseCanExecuteChanged();
            }
        }
        public CommandHandler AddNewEntityCommand { get; private set; }
        public CommandHandler CancelCommand { get; private set; }
        private async Task AddEntityAction()

        {
            var randomNrGenerator = new Random();
            var coord = new Coord() { X = randomNrGenerator.Next(_settings.EditorCanvasWidth - 80), Y = randomNrGenerator.Next(_settings.EditorCanvasHight - 50) };

            await _unitOfWork.Entities.AddAsync(new Entity() { Name = EntityName, Coordination = coord });
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
        private void CancelAction()
        {
            Close?.Invoke();
        }

        public async Task LoadAsync(object parameter)
        {
            //Initialization code should be implemented here.
        }
        public Action Close { get; set; }
    }
}
