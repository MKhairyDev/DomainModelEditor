namespace DomainModelEditor.Shared.Dto
{
    public class EntitiesResourceParameters: QueryStringParameters
    {
        public bool? IsPersistent{ get; set; }
        public string SearchQuery { get; set; }
    }
}