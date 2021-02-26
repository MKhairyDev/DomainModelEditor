
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainModelEditor.Domain
{
    public class Attribute
    {
        public Attribute()
        {
            Entities = new List<EntityAttributeValue>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string AttributeName { get; set; }
        public AttributeType AttributeType { get; set; }

        [StringLength(50)]
        public string DefaultValue { get; set; }
        [StringLength(20)]
        public string MinValue { get; set; }
        [StringLength(50)]
        public string MaxValue { get; set; }
        public bool AllowNull { get; set; }
        public List<EntityAttributeValue> Entities { get; set; }
    }
}
