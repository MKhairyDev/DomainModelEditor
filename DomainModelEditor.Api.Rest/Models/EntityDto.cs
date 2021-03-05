
using System.ComponentModel.DataAnnotations;

namespace DomainModelEditor.Api.Rest.Models
{
    public class EntityDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
