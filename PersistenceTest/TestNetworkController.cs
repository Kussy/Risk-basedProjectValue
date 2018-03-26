using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    [TestClass]
    public class TestNetworkController
    {
        SqliteConnection Connection { get; set; }
        DbContextOptions<RpvDbContext> Options { get; set; }
        RpvDbContext DbContext { get; set; }
        ProjectController ProjectController { get; set; }
        ActivityController ActivityController { get; set; }
        NetworkController NetworkController { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Connection = new SqliteConnection("DataSource=:memory:");
            Connection.Open();
            Options = new DbContextOptionsBuilder<RpvDbContext>().UseSqlite(Connection).Options;
            DbContext = new RpvDbContext(Options);
            DbContext.Database.EnsureCreated();
            ProjectController = new ProjectController(DbContext);
            ActivityController = new ActivityController(DbContext);
            NetworkController = new NetworkController(DbContext);
        }

        [TestCleanup]
        public void Creanup()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }

        [TestMethod]
        public void ネットワークのCRUD操作を一通り()
        {
            var code = "ancestor";
            var name = "ancestor";
            var expectedDepth = 0;
            ProjectController.Create(code, name);
            var project = ProjectController.Read(code);
            ActivityController.Create(project, code, name);
            var activity = ActivityController.Read(code);
 
            NetworkController.Create(activity, activity, expectedDepth);
            var selfNetwork = NetworkController.Read(activity, activity);
            selfNetwork.AncestorId.Is(activity.Id);
            selfNetwork.DescendantId.Is(activity.Id);
            selfNetwork.Depth.Is(expectedDepth);

            var changedDepth = 1;
            selfNetwork.Depth = changedDepth;
            NetworkController.Update(selfNetwork);

            var changedNetwork = NetworkController.Read(activity, activity);
            changedNetwork.Depth.Is(changedDepth);

            NetworkController.Delete(changedNetwork);
            NetworkController.Read(activity, activity).IsNull();
        }
    }
}