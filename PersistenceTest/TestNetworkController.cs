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

        [TestMethod]
        public void 末端にアクティビティを追加した場合先祖から子孫までの閉包が作成されるべき()
        {
            var codeProject = "p";
            ProjectController.Create(codeProject, codeProject);
            var project = ProjectController.Read(codeProject);

            var codeA = "a";
            var codeB = "b";
            var codeC = "c";
            var codeD = "d";
            ActivityController.Create(project, codeA, codeA);
            ActivityController.Create(project, codeB, codeB);
            ActivityController.Create(project, codeC, codeC);
            ActivityController.Create(project, codeD, codeD);
            var activityA = ActivityController.Read(codeA);
            var activityB = ActivityController.Read(codeB);
            var activityC = ActivityController.Read(codeC);
            var activityD = ActivityController.Read(codeD);

            NetworkController.Create(activityA, activityA, 0);
            NetworkController.Create(activityA, activityB, 1);
            NetworkController.Create(activityB, activityB, 0);

            NetworkController.Create(activityC, activityC, 0);
            NetworkController.Create(activityC, activityD, 1);
            NetworkController.Create(activityD, activityD, 0);

            NetworkController.Connect(activityB, activityC);

            NetworkController.Read(activityA, activityA).Depth.Is(0);
            NetworkController.Read(activityA, activityB).Depth.Is(1);
            NetworkController.Read(activityA, activityC).Depth.Is(2);
            NetworkController.Read(activityA, activityD).Depth.Is(3);
            NetworkController.Read(activityB, activityB).Depth.Is(0);
            NetworkController.Read(activityB, activityC).Depth.Is(1);
            NetworkController.Read(activityB, activityD).Depth.Is(2);
            NetworkController.Read(activityC, activityC).Depth.Is(0);
            NetworkController.Read(activityC, activityD).Depth.Is(1);
            NetworkController.Read(activityD, activityD).Depth.Is(0);
        }

        [TestMethod]
        public void MyTestMethod()
        {
            var codeProject = "p";
            ProjectController.Create(codeProject, codeProject);
            var project = ProjectController.Read(codeProject);

            var codeA = "a";
            var codeB = "b";
            var codeC = "c";
            var codeD = "d";
            ActivityController.Create(project, codeA, codeA);
            ActivityController.Create(project, codeB, codeB);
            ActivityController.Create(project, codeC, codeC);
            ActivityController.Create(project, codeD, codeD);
            var activityA = ActivityController.Read(codeA);
            var activityB = ActivityController.Read(codeB);
            var activityC = ActivityController.Read(codeC);
            var activityD = ActivityController.Read(codeD);

            NetworkController.Create(activityA, activityA, 0);
            NetworkController.Create(activityA, activityB, 1);
            NetworkController.Create(activityA, activityC, 2);
            NetworkController.Create(activityA, activityD, 3);
            NetworkController.Create(activityB, activityB, 0);
            NetworkController.Create(activityB, activityC, 1);
            NetworkController.Create(activityB, activityD, 2);
            NetworkController.Create(activityC, activityC, 0);
            NetworkController.Create(activityC, activityD, 1);
            NetworkController.Create(activityD, activityD, 0);

            NetworkController.Disconnect(activityC);

            var networks = NetworkController.Context.Networks.ToList();
        }
    }
}