namespace InterviewAssessment.Domain
{
    public class EntityAttributeValue
    {
        public int EntityId { get; set; }
        public int AttributeId { get; set; }
        public string Value { get; set; }
        public Entity Entity { get; set; }
        public Attribute Attribute { get; set; }
    }
}
