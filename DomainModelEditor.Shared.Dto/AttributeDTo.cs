using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DomainModelEditor.Shared.Dto
{
   public class AttributeDTo
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string AttributeName { get; set; }
        public AttributeTypeDto AttributeType { get; set; }

        [StringLength(50)]
        public string DefaultValue { get; set; }
        [StringLength(20)]
        public string MinValue { get; set; }
        [StringLength(50)]
        public string MaxValue { get; set; }
        public bool AllowNull { get; set; }
    }
}
