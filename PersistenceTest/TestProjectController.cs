using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    [TestClass]
    public class TestProjectController
    {
        SqliteConnection Connection { get; set; }
        DbContextOptions<RpvDbContext> Options { get; set; }
        RpvDbContext DbContext { get; set; }
        ProjectController ProjectController { get; set; }

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
        }

        [TestCleanup]
        public void Creanup()
        {
            DbContext.Dispose();
        }

        [TestMethod]
        public void プロジェクトのCRUD操作を一通り()
        {
            var expectedId = "id";
            var expectedName = "name";
            ProjectController.Create(expectedId, expectedName);

            var project = ProjectController.Read(expectedId);
            project.Id.Is(expectedId);
            project.Name.Is(expectedName);
            project.Budjet.Is(0m);
            project.LiquidatedDamages.Is(0m);
            project.Term.Is(0m);
            project.UnitOfCurrency.Is(CurrencyType.JPY);
            project.UnitOfTime.Is(TimeType.Day);
            project.Scopes.Count().Is(0);

            var changedName = "changedName";
            var changedNumber = 10m;
            var changedCurrency = CurrencyType.USD;
            var changedTime = TimeType.Month;
            project.Name = changedName;
            project.Budjet = changedNumber;
            project.LiquidatedDamages = changedNumber;
            project.Term = changedNumber;
            project.UnitOfCurrency = changedCurrency;
            project.UnitOfTime = changedTime;
            ProjectController.Update(project);

            var changedProject = ProjectController.Read(expectedId);
            changedProject.Id.Is(project.Id);
            changedProject.Name.Is(changedName);
            project.Budjet.Is(changedNumber);
            project.LiquidatedDamages.Is(changedNumber);
            project.Term.Is(changedNumber);
            project.UnitOfCurrency.Is(changedCurrency);
            project.UnitOfTime.Is(changedTime);
            project.Scopes.Count().Is(0);

            ProjectController.Delete(changedProject);
            ProjectController.Read(expectedId).IsNull();
        }
    }
}