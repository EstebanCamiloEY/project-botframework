using Azure.Data.Tables;
namespace Sample_BF.AzureStorage { 
public class AzureStorageHelper
{
    public TableEntity insert(Claim rk)
    {
        var tableClient = new TableClient(new Uri("https://storageformacionia.table.core.windows.net/RKsAbiertas"), "RKsAbiertas", new TableSharedKeyCredential("storageformacionia", "aWV/K6aBs3vjW0EoWY5wK/33S4zuIYhSwpg+tYFM+H2fN50KPwP03Pgmhk6vUHBp4lz4f/4zdmUa+AStylcriw=="));
        var entity = new TableEntity(rk.Id, Guid.NewGuid().ToString())
        {
            { "id", rk.Id },
            { "idinvoice", rk.IdInvoice },
            { "amount", rk.Amount },
            { "clientComment", rk.ClientComment },
            { "openaicomment", rk.OpenAIComment },
            { "concept", rk.Concept },

        };

        tableClient.AddEntity(entity);
        return entity;
    }
}
}

