namespace CrossCut.Messages;

public record OrderPayload(int ItemId, string ItemName, double TotalCost, int Quantity = 1);





public record PaymentRequest(string RequestId, string ItemName, int Amount, double Currency);

public record OrderResult(bool Processed);

//public record InventoryItem(int Id, string Name, double PerItemCost, int Quantity, int Stock);

public enum ApprovalResult
{
    Unspecified = 0,
    Approved = 1,
    Rejected = 2
}

