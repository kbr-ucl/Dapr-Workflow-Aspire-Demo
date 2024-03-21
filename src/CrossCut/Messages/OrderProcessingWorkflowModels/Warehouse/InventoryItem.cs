namespace CrossCut.Messages.OrderProcessingWorkflowModels.Warehouse;

public class InventoryItem
{
    public InventoryItem()
    {
        
    }
    public InventoryItem(int itemId, string itemName, int stock)
    {
        ItemId = itemId;
        ItemName = itemName;
        Stock = stock;
        InventoryItemReservations = new List<InventoryItemReservation>();
    }

    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public int Stock { get; set; }


    public List<InventoryItemReservation> InventoryItemReservations { get; set; } = new();
}

public class InventoryItemReservation
{
    public string RequestId { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; }
    public int Quantity { get; set; }
    public ReservationState State { get; set; } 
}

public enum ReservationState
{
    Unspecified = 0,
    Reserved = 1,
    Rejected = 2,
    Delivered = 3
}

public record InventoryItemReservationResult(bool Success, InventoryItemReservation OrderPayload, string ErrorMessage="");

public record InventoryItemCreateReservationRequest(string RequestId, int ItemId, string ItemName, int Quantity);

public record InventoryItemUpdateReservationRequest(string RequestId, int ItemId, int Quantity);

public record InventoryItemCancleReservationRequest(string RequestId, int ItemId);