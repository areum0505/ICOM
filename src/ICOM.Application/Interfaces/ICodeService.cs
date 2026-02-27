using System.Collections.Generic;
using System.Threading.Tasks;
using ICOM.Application.DTOs;

namespace ICOM.Application.Interfaces;

/// <summary>코드 조회 서비스 인터페이스</summary>
public interface ICodeService
{
    /// <summary>특정 그룹의 사용 중인 상세 코드 목록 반환</summary>
    Task<IEnumerable<DetailCodeDto>> GetDetailCodesAsync(string groupCode);
}
