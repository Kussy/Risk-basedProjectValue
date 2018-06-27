using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    [TestClass]
    public class TestResourceController
    {
        SqliteConnection Connection { get; set; }
        DbContextOptions<RpvDbContext> Options { get; set; }
        RpvDbContext DbContext { get; set; }
        ResourceController ResourceController { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Connection = new SqliteConnection("DataSource=:memory:");
            Connection.Open();
            Options = new DbContextOptionsBuilder<RpvDbContext>().UseSqlite(Connection).Options;
            DbContext = new RpvDbContext(Options);
            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
            ResourceController = new ResourceController(DbContext);
        }

        [TestCleanup]
        public void Creanup()
        {
            DbContext.Dispose();
        }

        [TestMethod]
        public void 資源のCRUD操作を一通り()
        {
            var expectedId = "id";
            var expectedName = "name";
            ResourceController.Create(expectedId, expectedName);

            var resource = ResourceController.Read(expectedId);
            resource.Id.Is(expectedId);
            resource.Name.Is(expectedName);
            resource.Type.Is(ResourceType.Unknown);
            resource.Productivity.Is(1m);
            resource.Assigns.Count().Is(0);

            var changedName = "changedName";
            var changedNumber = 2m;
            var changedType = ResourceType.Human;
            resource.Name = changedName;
            resource.Type = changedType;
            resource.Productivity= changedNumber;
            ResourceController.Update(resource);

            var changedResource = ResourceController.Read(expectedId);
            changedResource.Id.Is(resource.Id);
            changedResource.Name.Is(changedName);
            resource.Type.Is(changedType);
            resource.Productivity.Is(changedNumber);
            resource.Assigns.Count().Is(0);

            ResourceController.Delete(changedResource);
            ResourceController.Read(expectedId).IsNull();
        }
    }
}