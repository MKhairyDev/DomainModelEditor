﻿using InterviewAssessment.Data.Services;
using InterviewAssessment.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DomainModelEditor.ViewModels
{
   public class EntityAttributeDialogViewModel : BindableBase,IExtensible
    {
        IUnitOfWork _unitOfWork;
        private ObservableCollection<AttributeWrapper> _attributes;
        private int _entityId;

        public EntityAttributeDialogViewModel(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));

            _unitOfWork = unitOfWork;
            SaveEntityAttribute = new CommandHandler(async () => { await SaveEntityAttributeAction(); });

        }
        public async Task LoadAsync(object parameter)
        {
            await Initializaion(parameter);
        }
        private async Task Initializaion(object parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            int.TryParse(parameter.ToString(), out _entityId);
            if (_entityId <= 0)
                throw new ArgumentOutOfRangeException("Invalid Entity ID");

            var attributesList = await _unitOfWork.Attributes?.GetAllAsync();
            Attributes = new ObservableCollection<AttributeWrapper>();

            //Filtering to disable Attributes that have been assigned already to the entity
            attributesList.ToList().ForEach((attribute) =>
            {
             bool isAssignedalready=   attribute.Entities.Any(X => X.EntityId == _entityId);
                Attributes.Add(new AttributeWrapper(attribute) { IsEnabled=!isAssignedalready });
            });
        }
        public ObservableCollection<AttributeWrapper> Attributes
        {
            get { return _attributes; }
            set { SetProperty(ref _attributes, value); }
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
                //Show Message to the user that something wrong happend

            }

        }

        public Action Close { get; set; }
    }
}
