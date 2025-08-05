using SharpAstrology.Ephemerides;
using SharpAstrology.DataModels;
using SharpAstrology.ExtensionMethods;
using SharpAstrology.Enums;
using HumanDesignAPI.Models;

namespace HumanDesignAPI.Services
{
    public interface IChartService
    {
        Task<ChartResponse> CalculateChartAsync(ChartRequest request);
        Task<TransitResponse> CalculateTransitAsync(TransitRequest request);
    }

    public class ChartService : IChartService
    {
        private readonly ILogger<ChartService> _logger;

        public ChartService(ILogger<ChartService> logger)
        {
            _logger = logger;
        }

        public async Task<ChartResponse> CalculateChartAsync(ChartRequest request)
        {
            try
            {
                _logger.LogInformation($"Calculating chart for {request.BirthDateTime} in timezone {request.TimeZone}");

                // 转换本地时间到UTC
                var tz = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZone);
                var utc = TimeZoneInfo.ConvertTimeToUtc(request.BirthDateTime, tz);

                // 计算人类图
                using var eph = new SwissEphemeridesService("ephe").CreateContext();
                var chart = new HumanDesignChart(utc, eph);

                // 将Gates enum转为int数组
                int[] gateNumbers = chart.ActiveGates.Select(g => (int)g).OrderBy(n => n).ToArray();

                var response = new ChartResponse
                {
                    Type = chart.Type.ToString(),
                    Profile = chart.Profile.ToString(),
                    Gates = gateNumbers,
                    Channels = chart.ActiveChannels.Select(c => c.ToString()).ToArray(),
                    DefinedCenters = Array.Empty<string>(), // 暂时留空，稍后可以添加
                    Success = true
                };

                _logger.LogInformation($"Chart calculation successful. Type: {response.Type}, Profile: {response.Profile}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating chart");
                return new ChartResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<TransitResponse> CalculateTransitAsync(TransitRequest request)
        {
            try
            {
                _logger.LogInformation($"Calculating transit for {request.Date}");

                // 转换到UTC
                var tz = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZone);
                var utc = TimeZoneInfo.ConvertTimeToUtc(request.Date, tz);

                // 计算当日transit
                using var eph = new SwissEphemeridesService("ephe").CreateContext();
                var chart = new HumanDesignChart(utc, eph);

                // 获取活跃的gates和channels
                int[] activeGates = chart.ActiveGates.Select(g => (int)g).OrderBy(n => n).ToArray();
                string[] activeChannels = chart.ActiveChannels.Select(c => c.ToString()).ToArray();

                var response = new TransitResponse
                {
                    Date = request.Date,
                    ActiveGates = activeGates,
                    ActiveChannels = activeChannels,
                    Summary = GenerateTransitSummary(activeGates, activeChannels),
                    Success = true
                };

                _logger.LogInformation($"Transit calculation successful for {request.Date}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating transit");
                return new TransitResponse
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        private string GenerateTransitSummary(int[] gates, string[] channels)
        {
            // 简单的summary生成逻辑，可以后续扩展
            return $"今日共有 {gates.Length} 个门激活，{channels.Length} 个通道激活。";
        }
    }
}
