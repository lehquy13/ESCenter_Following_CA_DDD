namespace ESCenter.Application.Contracts.Charts;

public record AreaData( string name,List<float> data);
public record AreaChartData(AreaData TotalRevuenues,AreaData Incoming,AreaData Cenceleds,List<string> Dates);
