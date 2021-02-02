﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace InterviewAssessment.Domain
{
    public class Entity
    {
        public Entity()
        {
            Attributes = new List<EntityAttributeValue>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public Coord Coordination { get; set; }
        public List<EntityAttributeValue> Attributes { get; set; }
    }
}
