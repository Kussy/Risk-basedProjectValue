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
        public void アクティビティの作成時に自分自身のネットワークが作成されているべき()
        {
            var code = "ancestor";
            var name = "ancestor";
            var expectedDepth = 0;
            ProjectController.Create(code, name);
            var project = ProjectController.Read(code);
            ActivityController.Create(project, code, name);
            var activity = ActivityController.Read(code);
 
            var selfNetwork = NetworkController.Read(activity, activity);
            selfNetwork.AncestorId.Is(activity.Id);
            selfNetwork.DescendantId.Is(activity.Id);
            selfNetwork.Depth.Is(expectedDepth);
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

            NetworkController.Connect(activityA, activityB);
            NetworkController.Connect(activityB, activityC);
            NetworkController.Connect(activityC, activityD);

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
        public void 途中のアクティビティを切断した場合その子孫までで独立したネットワークが残るべき()
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

            NetworkController.Connect(activityA, activityB);
            NetworkController.Connect(activityB, activityC);
            NetworkController.Connect(activityC, activityD);

            NetworkController.Disconnect(activityC);

            NetworkController.Read(activityA, activityA).Depth.Is(0);
            NetworkController.Read(activityA, activityB).Depth.Is(1);
            NetworkController.Read(activityB, activityB).Depth.Is(0);
            NetworkController.Read(activityC, activityC).Depth.Is(0);
            NetworkController.Read(activityC, activityD).Depth.Is(1);
            NetworkController.Read(activityD, activityD).Depth.Is(0);
        }

        [TestMethod]
        public void アクティビティの親子を取得できるべき()
        {
            var codeProject = "p";
            ProjectController.Create(codeProject, codeProject);
            var project = ProjectController.Read(codeProject);

            var codeA = "a";
            var codeB = "b";
            var codeC = "c";
            var codeD = "d";
            var codeE = "e";
            ActivityController.Create(project, codeA, codeA);
            ActivityController.Create(project, codeB, codeB);
            ActivityController.Create(project, codeC, codeC);
            ActivityController.Create(project, codeD, codeD);
            ActivityController.Create(project, codeE, codeE);
            var activityA = ActivityController.Read(codeA);
            var activityB = ActivityController.Read(codeB);
            var activityC = ActivityController.Read(codeC);
            var activityD = ActivityController.Read(codeD);
            var activityE = ActivityController.Read(codeE);

            NetworkController.Connect(activityA, activityB);
            NetworkController.Connect(activityB, activityC);
            NetworkController.Connect(activityB, activityD);
            NetworkController.Connect(activityC, activityE);
            NetworkController.Connect(activityD, activityE);

            NetworkController.Parents(activityE).Count().Is(2);
            NetworkController.Parents(activityE).Select(a => a.Code).Contains(codeC).Is(true);
            NetworkController.Parents(activityE).Select(a => a.Code).Contains(codeD).Is(true);
            NetworkController.Children(activityB).Count().Is(2);
            NetworkController.Children(activityB).Select(a => a.Code).Contains(codeC).Is(true);
            NetworkController.Children(activityB).Select(a => a.Code).Contains(codeD).Is(true);
        }
    }
}