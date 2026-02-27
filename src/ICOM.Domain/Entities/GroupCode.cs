using System;
using System.Collections.Generic;

namespace ICOM.Domain.Entities;

/// <summary>
/// 그룹 코드 엔티티 — 상품 분류 등 코드 그룹을 관리
/// </summary>
public class GroupCode
{
    /// <summary>그룹 코드 (PK, 예: "CATEGORY")</summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>그룹 코드명 (예: "상품 분류")</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>그룹 설명</summary>
    public string? Description { get; set; }

    /// <summary>생성일시</summary>
    public DateTime CreateDate { get; set; }

    /// <summary>삭제일시 (Soft Delete)</summary>
    public DateTime? DeleteDate { get; set; }

    /// <summary>사용 여부</summary>
    public bool IsUse { get; set; } = true;

    /// <summary>하위 상세 코드 목록</summary>
    public ICollection<DetailCode> Details { get; set; } = new List<DetailCode>();
}
