namespace ESCenter.Application.Contract.Users;

public record AddressDto(List<CityDto> Cities);

public sealed class CityDto
{
    public string Id { get; set; } = String.Empty;
    public string Name { get; set; } = String.Empty;

    public List<DistrictDto> Districts { get; set; } = new List<DistrictDto>();
}

public sealed class DistrictDto
{
    public string Id { get; set; } = String.Empty;
    public List<WardDto> Wards { get; set; } = new List<WardDto>();
    public string Name { get; set; } = String.Empty;
    public string CityId { get; set; } = String.Empty;

    public string? EnglishName { get; set; } = String.Empty;
}

public sealed class WardDto
{
    public string Id { get; set; } = String.Empty;
    public string DistrictId { get; set; } = String.Empty;

    public string Name { get; set; } = String.Empty;
    public string WardLevel { get; set; } = String.Empty;
}