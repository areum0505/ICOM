namespace ICOM.Application.DTOs;

/// <summary>상세 코드 응답 DTO</summary>
public class DetailCodeDto
{
    public string Code { get; set; } = string.Empty;
    public string GroupCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsUse { get; set; }
}
