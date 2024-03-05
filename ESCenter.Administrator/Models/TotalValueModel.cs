using ESCenter.Domain.Shared.Courses;

namespace ESCenter.Administrator.Models;

public class TotalValueModel<T> where  T : class
{
    public List<T> Models = new();
    public bool IsIncrease = false;
    public double IncreasePercentage = 0;
    public string Time = ByTime.Today;
}