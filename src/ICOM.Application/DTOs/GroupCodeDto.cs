using System.Collections.Generic;

namespace ICOM.Application.DTOs;

/// <summary>그룹 코드 응답 DTO</summary>
public class GroupCodeDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsUse { get; set; }
    public List<DetailCodeDto> Details { get; set; } = new();
}
