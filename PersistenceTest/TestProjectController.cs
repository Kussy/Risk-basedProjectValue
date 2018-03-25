using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    [TestClass]
    public class TestProjectController
    {
        RpvDbContext DbContext { get; set; }

        ProjectController ProjectController { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            DbContext = new RpvDbContext();
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
            var expectedCode = "code";
            var expectedName = "name";
            ProjectController.Create(expectedCode, expectedName);

            var project = ProjectController.Read(expectedCode);
            project.Code.Is(expectedCode);
            project.Name.Is(expectedName);
            project.Budjet.Is(0m);
            project.LiquidatedDamages.Is(0m);
            project.Term.Is(0m);
            project.UnitOfCurrency.Is(CurrencyType.JPY);
            project.UnitOfTime.Is(TimeType.Day);
            project.Activities.Count().Is(0);

            var changedCode = "changedCode";
            var changedName = "changedName";
            var changedNumber = 10m;
            var changedCurrency = CurrencyType.USD;
            var changedTime = TimeType.Month;
            project.Code = changedCode;
            project.Name = changedName;
            project.Budjet = changedNumber;
            project.LiquidatedDamages = changedNumber;
            project.Term = changedNumber;
            project.UnitOfCurrency = changedCurrency;
            project.UnitOfTime = changedTime;
            ProjectController.Update(project);

            var changedProject = ProjectController.Read(changedCode);
            changedProject.Id.Is(project.Id);
            changedProject.Code.Is(changedCode);
            changedProject.Name.Is(changedName);
            project.Budjet.Is(changedNumber);
            project.LiquidatedDamages.Is(changedNumber);
            project.Term.Is(changedNumber);
            project.UnitOfCurrency.Is(changedCurrency);
            project.UnitOfTime.Is(changedTime);
            project.Activities.Count().Is(0);

            ProjectController.Delete(changedProject);
            ProjectController.Read(changedCode).IsNull();
        }
    }
}