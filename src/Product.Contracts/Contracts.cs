namespace Product.Contracts
{
    public record ProductItemCreated(Guid ItemId, string Name, string Description, decimal Price, int StockQuantity);
    public record ProductItemUpdated(Guid ItemId, string Name, string Description, decimal Price ,int StockQuantity);
    public record ProductItemDeleted(Guid ItemId);
}
