namespace DomainModelEditor.Data.ResourceParameters
{
    public class EntitiesResourceParameters: QueryStringParameters
    {
        public bool? IsPersistent{ get; set; }
        public string SearchQuery { get; set; }
    }
}