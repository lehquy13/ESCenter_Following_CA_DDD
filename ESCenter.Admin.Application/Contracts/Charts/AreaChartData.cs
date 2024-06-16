namespace ESCenter.Admin.Application.Contracts.Charts;

public record AreaData(string Name, List<decimal> Data);

public record AreaChartData(AreaData TotalRevenue, AreaData Incoming, AreaData Cancels, List<string> Dates);