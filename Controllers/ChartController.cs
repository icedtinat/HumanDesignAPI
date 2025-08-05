using Microsoft.AspNetCore.Mvc;
using HumanDesignAPI.Models;
using HumanDesignAPI.Services;

namespace HumanDesignAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChartController : ControllerBase
    {
        private readonly IChartService _chartService;
        private readonly ILogger<ChartController> _logger;

        public ChartController(IChartService chartService, ILogger<ChartController> logger)
        {
            _chartService = chartService;
            _logger = logger;
        }

        /// <summary>
        /// 计算人类图
        /// </summary>
        /// <param name="request">生日和时区信息</param>
        /// <returns>人类图数据</returns>
        [HttpPost("calculate")]
        public async Task<ActionResult<ChartResponse>> CalculateChart([FromBody] ChartRequest request)
        {
            try
            {
                _logger.LogInformation($"Received chart calculation request for {request.BirthDateTime}");
                
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _chartService.CalculateChartAsync(request);
                
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CalculateChart endpoint");
                return StatusCode(500, new ChartResponse 
                { 
                    Success = false, 
                    ErrorMessage = "Internal server error" 
                });
            }
        }

        /// <summary>
        /// 计算每日运势
        /// </summary>
        /// <param name="request">日期和时区信息</param>
        /// <returns>当日运势数据</returns>
        [HttpPost("transit")]
        public async Task<ActionResult<TransitResponse>> CalculateTransit([FromBody] TransitRequest request)
        {
            try
            {
                _logger.LogInformation($"Received transit calculation request for {request.Date}");
                
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _chartService.CalculateTransitAsync(request);
                
                if (!result.Success)
                {
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CalculateTransit endpoint");
                return StatusCode(500, new TransitResponse 
                { 
                    Success = false, 
                    ErrorMessage = "Internal server error" 
                });
            }
        }

        /// <summary>
        /// 健康检查端点
        /// </summary>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }
    }
}
