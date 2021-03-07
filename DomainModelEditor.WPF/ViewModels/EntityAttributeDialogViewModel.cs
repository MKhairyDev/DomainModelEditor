using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DomainModelEditor.Data.Abstractions;
using DomainModelEditor.Domain;

namespace DomainModelEditor.WPF.ViewModels
{
   public class EntityAttributeDialogViewModel : BindableBase,IExtensible
    {
        readonly IUnitOfWork _unitOfWork;
        private ObservableCollection<AttributeWrapper> _attributes;
        private int _entityId;
        private readonly List<int> _addedAttributes;
        public EntityAttributeDialogViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            SaveEntityAttribute = new CommandHandler(async () => { await SaveEntityAttributeAction(); }, CanExecuteSaveEntityAttribute);
            _addedAttributes = new List<int>();
        }

        public async Task LoadAsync(object parameter)
        {
            await Initialization(parameter);
        }
        private async Task Initialization(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            int.TryParse(parameter.ToString(), out _entityId);
            if (_entityId <= 0)
                throw new ArgumentOutOfRangeException("Invalid Entity ID");

            var attributesList = await _unitOfWork.Attributes.GetAllAsync();
            Attributes = new ObservableCollection<AttributeWrapper>();

            //Filtering to disable Attributes that have been assigned already to the entity
            attributesList.ToList().ForEach((attribute) =>
            {
             bool assignedBefore=   attribute.Entities.Any(x => x.EntityId == _entityId);
                var attr = new AttributeWrapper(attribute) { IsEnabled = !assignedBefore };
                attr.PropertyChanged += Attr_PropertyChanged;
                Attributes.Add(attr);
            });
        }
        public ObservableCollection<AttributeWrapper> Attributes
        {
            get => _attributes;
            set => SetProperty(ref _attributes, value);
        }
        public CommandHandler SaveEntityAttribute { get; private set; }
        private async Task SaveEntityAttributeAction()
        {
            List<EntityAttributeValue> entityAttributeValues = new List<EntityAttributeValue>();
            foreach (var item in Attributes)
            {
                if (item.IsSelected)
                {
                    entityAttributeValues.Add(new EntityAttributeValue() { EntityId = _entityId, AttributeId = item.Id });
                }
            }
            if (entityAttributeValues.Count == 0)
                return;
            await _unitOfWork.EntityAttributesValues.AddRangeAsync(entityAttributeValues);
            var res = await _unitOfWork.SaveAsync();
            if (res > 0)
            {
                Close?.Invoke();
            }
            else
            {
                //Show Message to the user that something wrong happens

            }

        }

        public Action Close { get; set; }
        private bool CanExecuteSaveEntityAttribute()
        {

            return _addedAttributes.Count > 0;
                
        }
        private void Attr_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var attr = sender as AttributeWrapper;
            if(attr==null)
                return;
            if (e.PropertyName == nameof(attr.IsSelected))
            {
                if (_addedAttributes.Contains(attr.Id))
                {
                    if (!attr.IsSelected)
                        _addedAttributes.Remove(attr.Id);
                }
                else
                {
                    _addedAttributes.Add(attr.Id);
                }
                SaveEntityAttribute.RaiseCanExecuteChanged();
            }
        }
    }
}
