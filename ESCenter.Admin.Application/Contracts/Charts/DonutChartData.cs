namespace ESCenter.Admin.Application.Contracts.Charts;

public record DonutData(int Value, string Name);
public record DonutChartData(List<int> Values, List<string> Names);
