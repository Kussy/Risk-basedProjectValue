using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    [TestClass]
    public class TestAssignController
    {
        SqliteConnection Connection { get; set; }
        DbContextOptions<RpvDbContext> Options { get; set; }
        RpvDbContext DbContext { get; set; }
        ProjectController ProjectController { get; set; }
        ActivityController ActivityController { get; set; }
        ResourceController ResourceController { get; set; }
        AssignController AssignController { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Connection = new SqliteConnection("DataSource=:memory:");
            Connection.Open();
            Options = new DbContextOptionsBuilder<RpvDbContext>().UseSqlite(Connection).Options;
            DbContext = new RpvDbContext(Options);
            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
            ProjectController = new ProjectController(DbContext);
            ActivityController = new ActivityController(DbContext);
            ResourceController = new ResourceController(DbContext);
            AssignController = new AssignController(DbContext);
        }

        [TestCleanup]
        public void Creanup()
        {
            DbContext.Dispose();
        }

        [TestMethod]
        public void 資源割当のCRUD操作を一通り()
        {
            var expectedCode = "code";
            var expectedName = "name";
            var expectedNumber = 1m;
            ProjectController.Create(expectedCode, expectedName);
            var project = ProjectController.Read(expectedCode);
            ActivityController.Create(project, expectedCode, expectedName);
            var activity = ActivityController.Read(expectedCode);
            ResourceController.Create(expectedCode, expectedName);
            var resource = ResourceController.Read(expectedCode);

            AssignController.Create(activity, resource, expectedNumber);

            var assign = AssignController.Read(activity, resource);
            assign.ActivityId.Is(activity.Id);
            assign.ResourceId.Is(resource.Id);
            assign.Quantity.Is(expectedNumber);

            var changedNumber = 2m;
            assign.Quantity = changedNumber;
            AssignController.Update(assign);

            var changedAssign = AssignController.Read(activity, resource);
            changedAssign.Quantity.Is(changedNumber);

            AssignController.Delete(changedAssign);
            AssignController.Read(activity, resource).IsNull();
        }
    }
}