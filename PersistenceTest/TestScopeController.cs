using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    [TestClass]
    public class TestScopeController
    {
        SqliteConnection Connection { get; set; }
        DbContextOptions<RpvDbContext> Options { get; set; }
        RpvDbContext DbContext { get; set; }
        ProjectController ProjectController { get; set; }
        ActivityController ActivityController { get; set; }
        ScopeController ScopeController { get; set; }

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
            ScopeController = new ScopeController(DbContext);
        }

        [TestCleanup]
        public void Creanup()
        {
            DbContext.Dispose();
        }

        [TestMethod]
        public void スコープのCRUD操作を一通り()
        {
            var expectedId = "id";
            var expectedName = "name";
            ProjectController.Create(expectedId, expectedName);
            var project = ProjectController.Read(expectedId);
            ActivityController.Create(project, expectedId, expectedName);
            var activity = ActivityController.Read(expectedId);

            var scope = ScopeController.Read(project, activity);
            scope.ActivityId.Is(expectedId);
            scope.ProjectId.Is(expectedId);

            var anotherId = "anotherId";
            ProjectController.Create(anotherId, expectedName);
            ActivityController.Create(project, anotherId, expectedName);
            var anotherProject = ProjectController.Read(anotherId);
            var anotherActivity = ActivityController.Read(anotherId);

            var changeProjectScope = ScopeController.Change(scope, anotherProject);
            changeProjectScope.ProjectId.Is(anotherId);
            changeProjectScope.ActivityId.Is(expectedId);
            var changeActivityScope = ScopeController.Change(changeProjectScope, anotherActivity);
            changeActivityScope.ProjectId.Is(anotherId);
            changeActivityScope.ActivityId.Is(anotherId);
        }
    }
}