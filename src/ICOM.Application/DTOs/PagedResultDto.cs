namespace ICOM.Application.DTOs;

/// <summary>
/// 페이지네이션이 적용된 목록 응답 공통 DTO
/// </summary>
public class PagedResultDto<T>
{
    public IEnumerable<T> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
