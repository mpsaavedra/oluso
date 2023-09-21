namespace Oluso.Data.AutoMapping;

/// <summary>
/// Mark the DTO to be mapped used some provided T type
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMapFrom<T>
{
    /// <summary>
    /// Register the mapping object into the autoMapper profile, it uses
    /// the T class as source for mapping and it maps into this object
    /// </summary>
    /// <param name="profile"></param>
    void Mapping(AutoMapper.Profile profile) =>
        profile.CreateMap(typeof(T), GetType()).ReverseMap();
}