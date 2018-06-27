using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    [TestClass]
    public class TestActivityController
    {
        SqliteConnection Connection { get; set; }
        DbContextOptions<RpvDbContext> Options { get; set; }
        RpvDbContext DbContext { get; set; }
        ProjectController ProjectController { get; set; }
        ActivityController ActivityController { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            Connection = new SqliteConnection("DataSource=:memory:");
            Connection.Open();
            Options = new DbContextOptionsBuilder<RpvDbContext>().UseSqlite(Connection).Options;
            DbContext = new RpvDbContext(Options);
            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
            ActivityController = new ActivityController(DbContext);
            ProjectController = new ProjectController(DbContext);
        }

        [TestCleanup]
        public void Creanup()
        {
            DbContext.Dispose();
            Connection.Close();
        }

        [TestMethod]
        public void 作業のCRUD操作を一通り()
        {
            var expectedId = "id";
            var expectedName = "name";
            ProjectController.Create(expectedId, expectedName);
            var project = ProjectController.Read(expectedId);
            ActivityController.Create(project, expectedId, expectedName);

            var activity = ActivityController.Read(expectedId);
            activity.Scope.Project.Id.Is(project.Id);
            activity.Id.Is(expectedId);
            activity.Name.Is(expectedName);
            activity.State.Is(StateType.Unknown);
            activity.Income.Is(0m);
            activity.ExternalCost.Is(0m);
            activity.FixedLeadTime.Is(0m);
            activity.Workload.Is(0m);
            activity.RateOfFailure.Is(0m);
            activity.Assigns.Count().Is(0);

            var changedId = "changedId";
            var changedName = "changedName";
            var changedNumber = 1m;
            var changedType = StateType.ToDo;
            ProjectController.Create(changedId, changedName);
            var changedProject = ProjectController.Read(changedId);
            activity.Name = changedName;
            activity.State = changedType;
            activity.Income= changedNumber;
            activity.ExternalCost = changedNumber;
            activity.FixedLeadTime = changedNumber;
            activity.Workload = changedNumber;
            activity.RateOfFailure = changedNumber;
            ActivityController.Update(activity);

            var changedActivity = ActivityController.Read(expectedId);
            changedActivity.Id.Is(expectedId);
            changedActivity.Name.Is(changedName);
            changedActivity.State.Is(changedType);
            changedActivity.Income.Is(changedNumber);
            changedActivity.ExternalCost.Is(changedNumber);
            changedActivity.FixedLeadTime.Is(changedNumber);
            changedActivity.Workload.Is(changedNumber);
            changedActivity.RateOfFailure.Is(changedNumber);
            changedActivity.Assigns.Count().Is(0);

            ActivityController.Delete(changedActivity);
            ActivityController.Read(changedId).IsNull();
        }
    }
}