using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DomainModelEditor.Data.Abstractions;
using DomainModelEditor.Domain;
using DomainModelEditor.Navigation;
using Attribute = DomainModelEditor.Domain.Attribute;

namespace DomainModelEditor.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase, IExtensible
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitOfWork _unitOfWork;
        private ObservableCollection<Attribute> _attributes;
        private ObservableCollection<Entity> _entities;

        public MainWindowViewModel(IUnitOfWork unitOfWork, INavigationService navigationService)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            AddNewEntityCommand = new CommandHandler(async () => { await AddEntityAction(); });
            AddNewAttributeCommand = new CommandHandler(async () => { await AddNewAttributeAction(); });
            AddAttributeCommand = new RelayCommand<int>(async entityId => await AddAttributeAction(entityId));
        }

        public ObservableCollection<Entity> Entities
        {
            get => _entities;
            set => SetProperty(ref _entities, value);
        }

        public ObservableCollection<Attribute> Attributes
        {
            get => _attributes;
            set => SetProperty(ref _attributes, value);
        }

        public CommandHandler AddNewEntityCommand { get; }
        public RelayCommand<int> AddAttributeCommand { get; }
        public CommandHandler AddNewAttributeCommand { get; }

        public async Task LoadAsync(object parameter = null)
        {
            await Initialization();
        }

        public Action Close { get; set; }

        private async Task Initialization()
        {
            var res = await _unitOfWork.Entities.GetEntitiesWithAttributesAsync();
            Entities = res != null ? new ObservableCollection<Entity>(res) : new ObservableCollection<Entity>();

            var attributesList = await _unitOfWork.Attributes.GetAllAsync();
            Attributes = attributesList != null
                ? new ObservableCollection<Attribute>(attributesList)
                : new ObservableCollection<Attribute>();
        }

        private async Task AddEntityAction()
        {
            await _navigationService.ShowDialogAsync(WindowsNames.AddEntity);

            var entitiesWithAttributes = await _unitOfWork.Entities.GetEntitiesWithAttributesAsync();
            Entities = new ObservableCollection<Entity>(entitiesWithAttributes);
        }

        private async Task AddNewAttributeAction()
        {
            await _navigationService.ShowDialogAsync(WindowsNames.AddAttribute);
            var attributesList = await _unitOfWork.Attributes.GetAllAsync();
            Attributes = attributesList != null
                ? new ObservableCollection<Attribute>(attributesList)
                : new ObservableCollection<Attribute>();
        }

        private async Task AddAttributeAction(int id)
        {
            await _navigationService.ShowDialogAsync(WindowsNames.AddEntityAttribute, id);

            var entitiesWithAttributes = await _unitOfWork.Entities.GetEntitiesWithAttributesAsync();
            Entities = new ObservableCollection<Entity>(entitiesWithAttributes);
        }
    }
}