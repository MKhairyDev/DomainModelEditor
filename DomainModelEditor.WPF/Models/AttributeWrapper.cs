
using DomainModelEditor.Models;
using System;
using System.Collections.Generic;

namespace DomainModelEditor.Domain
{
    public class AttributeWrapper : ModelWrapper<Attribute>
    {
        private bool _isSelected;
        private bool _isEnabled;
        public AttributeWrapper(Attribute attribute) : base(attribute)
        {
        }
        public int Id
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }
        public string AttributeName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public AttributeType AttributeType
        {
            get { return GetValue<AttributeType>(); }
            set { SetValue(value); }
        }
        public string DefaultValue
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public string MinValue
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public string MaxValue
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }
        public bool AllowNull
        {
            get { return GetValue<bool>(); }
            set { SetValue(value); }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetProperty(ref _isSelected, value);
            }

        }
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                SetProperty(ref _isEnabled, value);
            }

        }
        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            // This is for explanation sake.
            switch (propertyName)
            {
                case nameof(AttributeName):
                    if (string.Equals(AttributeName, "Test", StringComparison.OrdinalIgnoreCase))
                    {
                        yield return "Test is not valid attribute Name";
                    }
                    break;
            }
        }
    }
}
