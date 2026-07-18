using System.Reflection;
using Xunit;

namespace ModularMonolith.ArchitectureTests;

public sealed class ArchitectureRulesTests
{
    [Fact] public void Orders_Domain_Does_Not_Reference_User_Or_Product_Domain() => AssertSource("Modules/Orders", ["Users.Domain", "Products.Domain"]);
    [Fact] public void Users_And_Products_Do_Not_Depend_On_Orders() { AssertSource("Modules/Users", ["Modules.Orders"]); AssertSource("Modules/Products", ["Modules.Orders"]); }
    [Fact] public void Domain_Entities_Are_Internal() => AssertSource("Modules", ["public sealed class User", "public sealed class Product", "public sealed class Order ", "public sealed class OrderItem"]);
    [Fact] public void Controllers_Are_In_Module_Api_Namespaces() { foreach (var file in Directory.GetFiles(PathToRepo(), "*Controller.cs", SearchOption.AllDirectories).Where(x=>x.Contains("Modules"))) Assert.Contains(".Api.Controllers", File.ReadAllText(file)); }
    [Fact] public void Module_DbContexts_Are_Not_Referenced_By_Other_Modules() { AssertSource("Modules/Orders", ["UsersDbContext", "ProductsDbContext"]); AssertSource("Modules/Users", ["OrdersDbContext"]); AssertSource("Modules/Products", ["OrdersDbContext"]); }
    [Fact] public void Host_Contains_Composition_Only() { var program=File.ReadAllText(Path.Combine(PathToRepo(),"ModularMonolith.Reference.Api/Program.cs")); Assert.DoesNotContain("DbContext", program); Assert.DoesNotContain("Repository", program); }
    private static void AssertSource(string relativePath, string[] forbidden){ foreach(var file in Directory.GetFiles(Path.Combine(PathToRepo(),relativePath),"*.cs",SearchOption.AllDirectories)) { var text=File.ReadAllText(file); foreach(var f in forbidden) Assert.DoesNotContain(f,text); } }
    private static string PathToRepo(){ var d=new DirectoryInfo(AppContext.BaseDirectory); while(d is not null && !Directory.Exists(Path.Combine(d.FullName,"Modules"))) d=d.Parent; return d!.FullName; }
}
