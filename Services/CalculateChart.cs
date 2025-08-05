using SharpAstrology.Ephemerides;
using SharpAstrology.DataModels;
using SharpAstrology.ExtensionMethods;
using SharpAstrology.Enums;
using System;
using System.Linq;

namespace HumanDesignClean
{
    public static class CalculateChart
    {
        public static ChartDTO Calculate(DateTime localTime, string tzId)
        {
            try
            {
                // ① 本地时间 → UTC
                var tz = TimeZoneInfo.FindSystemTimeZoneById(tzId);
                var utc = TimeZoneInfo.ConvertTimeToUtc(localTime, tz);

                // ② 计算人类图
                using var eph = new SwissEphemeridesService("ephe").CreateContext();
                var chart = new HumanDesignChart(utc, eph);

                // ③ 将 Gates enum 转 int (枚举值本身就是门号)
                int[] gateNumbers = chart.ActiveGates.Select(g => (int)g).OrderBy(n => n).ToArray();

                // ④ 将 Channels 转为字符串数组
                string[] activeChannels = chart.ActiveChannels.Select(c => c.ToString()).ToArray();

                // ⑤ 获取已定义的能量中心 (如果可用)
                string[] definedCenters = Array.Empty<string>(); // 暂时留空，可后续扩展

                return new ChartDTO(
                    chart.Type.ToString(),
                    chart.Profile.ToString(),
                    gateNumbers,
                    activeChannels,
                    definedCenters
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"计算错误: {ex.Message}");
                throw;
            }
        }
    }
}
