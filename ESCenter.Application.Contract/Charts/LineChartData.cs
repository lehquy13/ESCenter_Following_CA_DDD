namespace ESCenter.Application.Contracts.Charts;

public record LineData(string name, List<int> data);
public record LineChartData(List<LineData> LineDatas, List<int> dates);
