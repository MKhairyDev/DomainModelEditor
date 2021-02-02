using DomainModelEditor.Navigation;
using InterviewAssessment.Data.Services;
using InterviewAssessment.Domain;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Attribute = InterviewAssessment.Domain.Attribute;

namespace DomainModelEditor.ViewModels
{
   public class MainWindowViewModel: BindableBase,IExtensible
    {
        IUnitOfWork _unitOfWork;
        private ObservableCollection<Entity> _entities;
        private ObservableCollection<Attribute> _attributes;
        private readonly INavigationService _navigationService;

        public MainWindowViewModel(IUnitOfWork unitOfWork, INavigationService navigationService)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            if (navigationService == null)
                throw new ArgumentNullException(nameof(navigationService));

            _unitOfWork = unitOfWork;
            _navigationService = navigationService;

            AddNewEntityCommand = new CommandHandler(async () => { await AddEntityAction(); });
            AddNewAttributeCommand = new CommandHandler(async () => { await AddNewAttributeAction(); });
            AddAttributeCommand = new RelayCommand<int>(async entityId => await AddAttributeAction(entityId));
        }
        public async Task LoadAsync(object parameter=null)
        {
          await  Initializaion();
        }
        private async Task Initializaion()
        {
            var res = await _unitOfWork.Entities?.GetEntitiesWithAttributesAsync();
            Entities = res != null ? new ObservableCollection<Entity>(res) : new ObservableCollection<Entity>();

            var attributesList =await _unitOfWork.Attributes?.GetAllAsync();
            Attributes = attributesList != null ? new ObservableCollection<Attribute>(attributesList) : new ObservableCollection<Attribute>();
        }
        public ObservableCollection<Entity> Entities
        {
            get { return _entities; }
            set { SetProperty(ref _entities, value); }
        }
        public ObservableCollection<Attribute> Attributes
        {
            get { return _attributes; }
            set { SetProperty(ref _attributes, value); }
        }

        public CommandHandler AddNewEntityCommand { get; private set; }
        public RelayCommand<int> AddAttributeCommand { get; private set; }
        public CommandHandler AddNewAttributeCommand { get; private set; }
        private async Task AddEntityAction()
        {
            await _navigationService.ShowDialogAsync(WindowsNames.AddEntity);

            var entitiesWithAttributes =  await _unitOfWork.Entities.GetEntitiesWithAttributesAsync();
            Entities = new ObservableCollection<Entity>(entitiesWithAttributes);

        }
        private async Task AddNewAttributeAction()
        {
            await _navigationService.ShowDialogAsync(WindowsNames.AddAttribute);
            var attributesList = await _unitOfWork.Attributes.GetAllAsync();
            Attributes = attributesList != null ? new ObservableCollection<Attribute>(attributesList) : new ObservableCollection<Attribute>();

        }
        private async Task AddAttributeAction(int id)
        {
            await _navigationService.ShowDialogAsync(WindowsNames.AddEntityAttribute, id);

            var entitiesWithAttributes = await _unitOfWork.Entities.GetEntitiesWithAttributesAsync();
            Entities = new ObservableCollection<Entity>(entitiesWithAttributes);
        }
        public Action Close { get; set; }

    }
}
