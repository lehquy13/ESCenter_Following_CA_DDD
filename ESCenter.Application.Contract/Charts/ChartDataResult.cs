namespace ESCenter.Application.Contracts.Charts;

public class ChartDataResult
{
    public readonly LineChartData[] LineChartData = Array.Empty<LineChartData>();
    public readonly DonutChartData[] DonutChartData = Array.Empty<DonutChartData>();
}