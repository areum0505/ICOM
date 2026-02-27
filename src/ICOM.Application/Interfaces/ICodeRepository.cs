using System.Collections.Generic;
using System.Threading.Tasks;
using ICOM.Domain.Entities;

namespace ICOM.Application.Interfaces;

/// <summary>코드 테이블 조회 저장소 인터페이스</summary>
public interface ICodeRepository
{
    /// <summary>특정 그룹의 사용 중인 상세 코드 목록 조회</summary>
    Task<IEnumerable<DetailCode>> GetDetailCodesAsync(string groupCode);
}
