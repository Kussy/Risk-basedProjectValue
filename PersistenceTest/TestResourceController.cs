using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    [TestClass]
    public class TestResourceController
    {
        RpvDbContext DbContext { get; set; }

        ResourceController ResourceController { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            DbContext = new RpvDbContext();
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
            var expectedCode = "code";
            var expectedName = "name";
            ResourceController.Create(expectedCode, expectedName);

            var resource = ResourceController.Read(expectedCode);
            resource.Code.Is(expectedCode);
            resource.Name.Is(expectedName);
            resource.Type.Is(ResourceType.Unknown);
            resource.Productivity.Is(1m);
            resource.Assigns.Count().Is(0);

            var changedCode = "changedCode";
            var changedName = "changedName";
            var changedNumber = 2m;
            var changedType = ResourceType.Human;
            resource.Code = changedCode;
            resource.Name = changedName;
            resource.Type = changedType;
            resource.Productivity= changedNumber;
            ResourceController.Update(resource);

            var changedResource = ResourceController.Read(changedCode);
            changedResource.Id.Is(resource.Id);
            changedResource.Code.Is(changedCode);
            changedResource.Name.Is(changedName);
            resource.Type.Is(changedType);
            resource.Productivity.Is(changedNumber);
            resource.Assigns.Count().Is(0);

            ResourceController.Delete(changedResource);
            ResourceController.Read(changedCode).IsNull();
        }
    }
}