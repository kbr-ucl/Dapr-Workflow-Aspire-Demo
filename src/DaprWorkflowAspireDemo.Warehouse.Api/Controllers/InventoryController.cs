using CrossCut.DaprConsts;
using CrossCut.Messages.OrderProcessingWorkflowModels.Warehouse;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DaprWorkflowAspireDemo.Warehouse.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class InventoryItemReservationController : ControllerBase
{
    private readonly DaprClient _client;

    public InventoryItemReservationController(DaprClient client)
    {
        _client = client;
    }

    //// GET: api/<InventoryController>
    //[HttpGet("/InventoryItem")]
    //public async Task<ActionResult<IEnumerable<InventoryItem>>> Get() //IEnumerable<InventoryItem> Get()
    //{
    //    //var res = new List<InventoryItem>();
    //    //var result = await _client.GetStateAsync<string>(DaprWarehouseConsts.DAPR_STORE_NAME, orderId.ToString());
    //    var result2 =
    //        await _client.GetBulkStateAsync<IEnumerable<InventoryItem>>(DaprWarehouseConsts.DAPR_STORE_NAME,
    //            new[] { DaprWarehouseConsts.DAPR_ID }, 0);

    //    Console.WriteLine("All inventory: ");
    //    return Ok(result2);
    //}

    // GET InventoryItemReservationRequest/InventoryItem/5
    [HttpGet("/InventoryItem/{itemId}")]
    public async Task<ActionResult<InventoryItem>> Get(int itemId)
    {
        var result = await _client.GetStateAsync<InventoryItem>(DaprWarehouseConsts.DAPR_STORE_NAME, itemId.ToString());
        Console.WriteLine("Getting Order: " + result);
        return Ok(result);
    }

    [HttpGet("/InventoryItem/{itemId}/InventoryItemReservation/{requestId}")]
    public async Task<ActionResult<InventoryItem>> Get(int itemId, int requestId)
    {
        var result = await _client.GetStateAsync<InventoryItem>(DaprWarehouseConsts.DAPR_STORE_NAME, itemId.ToString());
        Console.WriteLine("Getting Order: " + result);
        return Ok(result);
    }


    // POST InventoryItemReservationRequest
    [HttpPost("/InventoryItemReservation")]
    public async Task<ActionResult> Post(InventoryItemCreateReservationRequest req)
    {
        var inventoryItemReservation = new InventoryItemReservation
        {
            ItemId = req.ItemId,
            ItemName = req.ItemName,
            Quantity = req.Quantity,
            RequestId = req.RequestId,
            State = ReservationState.Reserved
        };

        var item = await _client.GetStateAsync<InventoryItem>(DaprWarehouseConsts.DAPR_STORE_NAME,
            req.ItemId.ToString());
        if (item.Stock >= req.Quantity)
        {
            item.Stock -= req.Quantity;
            item.InventoryItemReservations.Add(inventoryItemReservation);
            await _client.SaveStateAsync(DaprWarehouseConsts.DAPR_STORE_NAME, req.ItemId.ToString(), item);
            return Ok(new InventoryItemReservationResult(true, inventoryItemReservation));
        }

        inventoryItemReservation.State = ReservationState.Rejected;
        Console.WriteLine(
            $"Creating InventoryItemReservationRequest: (item, req, quantity): {item.ItemId}, {req.RequestId}, {req.Quantity}");
        return Ok();
    }

    // PUT InventoryItemReservationRequest
    [HttpPut("/InventoryItemReservation")]
    public async Task<ActionResult> Put(InventoryItemUpdateReservationRequest req)
    {
        var item = await _client.GetStateAsync<InventoryItem>(DaprWarehouseConsts.DAPR_STORE_NAME,
            req.ItemId.ToString());
        if (item is null) return BadRequest(new InventoryItemReservationResult(false, null, $"InventoryItem not found {req.ItemId}"));

        var inventoryItemReservation = item.InventoryItemReservations.Find(x => x.RequestId == req.RequestId);

        if (inventoryItemReservation == null) return BadRequest(new InventoryItemReservationResult(false, null, $"InventoryItemReservation not found {req.RequestId}"));
        if (item.Stock < req.Quantity) return BadRequest(new InventoryItemReservationResult(false, inventoryItemReservation, $"Stock level to low {item.Stock}"));

        item.Stock -= req.Quantity;
        inventoryItemReservation.Quantity = req.Quantity;
        await _client.SaveStateAsync(DaprWarehouseConsts.DAPR_STORE_NAME, req.ItemId.ToString(), item);
        return Ok(new InventoryItemReservationResult(true, inventoryItemReservation));
    }

    // DELETE api/InventoryItem/5/6
    [HttpDelete("/InventoryItemReservation/{itemId}/{requestId}")]
    public async Task Delete(int itemId, int requestId)
    {
    }

    [HttpPost("/InventoryItem/Seed")]
    public async Task<ActionResult> Post(CancellationToken ct)
{
        var inventoryItems = new List<InventoryItem>
        {
            new InventoryItem(1, "Item 1", 100),
            new InventoryItem(2, "Item 2", 100),
            new InventoryItem(3, "Item 3", 100),
            new InventoryItem(4, "Item 4", 100),
            new InventoryItem(5, "Item 5", 100),
            new InventoryItem(6, "Item 6", 100),
            new InventoryItem(7, "Item 7", 100),
            new InventoryItem(8, "Item 8", 100),
            new InventoryItem(9, "Item 9", 100),
            new InventoryItem(10, "Item 10", 100)
        };

        foreach (var inventoryItem in inventoryItems)
        {
            await _client.SaveStateAsync(DaprWarehouseConsts.DAPR_STORE_NAME, inventoryItem.ItemId.ToString(), inventoryItem,null, null, ct);
        }

        return Ok();
    }
}