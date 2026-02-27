using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICOM.Application.Interfaces;
using ICOM.Domain.Entities;
using ICOM.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ICOM.Infrastructure.Repositories;

/// <summary>코드 테이블 조회 저장소 구현체</summary>
public class CodeRepository : ICodeRepository
{
    private readonly AppDbContext _context;

    public CodeRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>특정 그룹의 사용 중인 상세 코드 목록 조회</summary>
    public async Task<IEnumerable<DetailCode>> GetDetailCodesAsync(string groupCode)
    {
        return await _context.DetailCodes
            .AsNoTracking()
            .Where(d => d.GroupCode == groupCode && d.IsUse && d.DeleteDate == null)
            .OrderBy(d => d.Code)
            .ToListAsync();
    }
}
