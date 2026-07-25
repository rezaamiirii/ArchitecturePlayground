using DistributedMonolith.Products.Domain; using DistributedMonolith.Products.Infrastructure; using DistributedMonolith.SharedContracts; using Microsoft.EntityFrameworkCore;
namespace DistributedMonolith.Products.Application;
public sealed class ProductService(ProductsDbContext db){
 public async Task<ProductResponse>Create(CreateProductRequest r){if(r.Price<0||r.Stock<0)throw new ArgumentOutOfRangeException(nameof(r));var p=new Product{Id=Guid.NewGuid(),Name=r.Name,Price=r.Price,Stock=r.Stock};db.Add(p);await db.SaveChangesAsync();return Map(p);}
 public async Task<ProductResponse?>Get(Guid id){var p=await db.Products.AsNoTracking().SingleOrDefaultAsync(x=>x.Id==id);return p is null?null:Map(p);}
 public async Task<ProductResponse?>UpdateStock(Guid id,int quantity){if(quantity<0)throw new ArgumentOutOfRangeException(nameof(quantity));var p=await db.Products.FindAsync(id);if(p is null)return null;p.Stock=quantity;await db.SaveChangesAsync();return Map(p);}
 public async Task<StockReservationResponse>Reserve(Guid id,int quantity){if(quantity<=0)return new(false,"Quantity must be positive.");var changed=await db.Products.Where(p=>p.Id==id&&p.Stock>=quantity).ExecuteUpdateAsync(s=>s.SetProperty(p=>p.Stock,p=>p.Stock-quantity));return changed==1?new(true):new(false,"Product does not exist or has insufficient stock.");}
 static ProductResponse Map(Product p)=>new(p.Id,p.Name,p.Price,p.Stock);
}
