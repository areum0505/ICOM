using System.Collections.Generic;
using System.Threading.Tasks;
using ICOM.Application.DTOs;
using ICOM.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICOM.Api.Controllers;

/// <summary>
/// 코드 관리 API — 상세 코드 조회 전용
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CodesController : ControllerBase
{
    private readonly ICodeService _codeService;

    public CodesController(ICodeService codeService)
    {
        _codeService = codeService;
    }

    /// <summary>특정 그룹의 상세 코드 목록 조회</summary>
    /// <param name="groupCode">그룹 코드 (예: ProductCategory)</param>
    [HttpGet("{groupCode}/details")]
    [ProducesResponseType(typeof(IEnumerable<DetailCodeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDetails(string groupCode)
    {
        var details = await _codeService.GetDetailCodesAsync(groupCode);
        return Ok(details);
    }
}
