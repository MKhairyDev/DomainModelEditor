namespace DomainModelEditor.Api.Rest.Services
{
    /// <summary>
    /// With Shaping data technique we need to check if each field passed through query string exists in the source DTO model
    /// </summary>
    public interface IPropertyCheckerService
    {
        bool TypeHasProperties<T>(string fields);
    }
}