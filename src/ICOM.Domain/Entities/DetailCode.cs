using System;

namespace ICOM.Domain.Entities;

/// <summary>
/// 상세 코드 엔티티 — 그룹 코드에 속하는 개별 코드 항목
/// </summary>
public class DetailCode
{
    /// <summary>상세 코드 (PK, 예: "BAR")</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>소속 그룹 코드 (FK)</summary>
    public string GroupCode { get; set; } = string.Empty;

    /// <summary>상세 코드명 (예: "바")</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>상세 설명</summary>
    public string? Description { get; set; }

    /// <summary>생성일시</summary>
    public DateTime CreateDate { get; set; }

    /// <summary>삭제일시 (Soft Delete)</summary>
    public DateTime? DeleteDate { get; set; }

    /// <summary>사용 여부</summary>
    public bool IsUse { get; set; } = true;

    /// <summary>소속 그룹 코드 네비게이션</summary>
    public GroupCode Group { get; set; } = null!;
}
