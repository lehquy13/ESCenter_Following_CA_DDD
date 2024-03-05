namespace ESCenter.Admin.Application.Contracts.Charts;

public record LineData(string Name, List<int> Data);
public record LineChartData(List<LineData> LineData, List<int> Dates);
