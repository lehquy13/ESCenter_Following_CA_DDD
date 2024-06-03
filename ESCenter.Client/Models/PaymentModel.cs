namespace ESCenter.Client.Models;

public class PaymentModel(Guid id, string code, string tutorName)
{
    public Guid PaymentId { get; } = id;

    public string PaymentUrl =
        $"https://img.vietqr.io/image/vietinbank-107867236970-compact2.jpg?amount=5000&addInfo=Payment {code} by {tutorName}&accountName=Hu%E1%BB%B3nh%20Trung%20Hi%E1%BA%BFu";
}