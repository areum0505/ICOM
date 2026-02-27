using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICOM.Application.DTOs;
using ICOM.Application.Interfaces;
using ICOM.Domain.Entities;

namespace ICOM.Application.Services;

/// <summary>코드 조회 서비스 구현체</summary>
public class CodeService : ICodeService
{
    private readonly ICodeRepository _repository;

    public CodeService(ICodeRepository repository)
    {
        _repository = repository;
    }

    /// <summary>특정 그룹의 사용 중인 상세 코드 목록 반환</summary>
    public async Task<IEnumerable<DetailCodeDto>> GetDetailCodesAsync(string groupCode)
    {
        var details = await _repository.GetDetailCodesAsync(groupCode);
        return details.Select(ToDetailDto);
    }

    private static DetailCodeDto ToDetailDto(DetailCode detail) => new()
    {
        Code = detail.Code,
        GroupCode = detail.GroupCode,
        Name = detail.Name,
        Description = detail.Description,
        IsUse = detail.IsUse
    };
}
