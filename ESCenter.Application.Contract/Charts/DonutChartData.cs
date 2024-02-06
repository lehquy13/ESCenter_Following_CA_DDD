namespace ESCenter.Application.Contract.Charts;

public record DonutData(int value, string name);
public record DonutChartData(List<int> values, List<string> names);
