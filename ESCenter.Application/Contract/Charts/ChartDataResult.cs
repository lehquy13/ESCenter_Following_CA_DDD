namespace ESCenter.Application.Contract.Charts;

public class ChartDataResult
{
    public readonly LineChartData[] LineChartData = Array.Empty<LineChartData>();
    public readonly DonutChartData[] DonutChartData = Array.Empty<DonutChartData>();
}