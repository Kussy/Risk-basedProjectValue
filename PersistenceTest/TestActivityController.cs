using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    [TestClass]
    public class TestActivityController
    {
        RpvDbContext DbContext { get; set; }

        ProjectController ProjectController { get; set; }
        ActivityController ActivityController { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            DbContext = new RpvDbContext();
            DbContext.Database.EnsureDeleted();
            DbContext.Database.EnsureCreated();
            ActivityController = new ActivityController(DbContext);
            ProjectController = new ProjectController(DbContext);
        }

        [TestCleanup]
        public void Creanup()
        {
            DbContext.Dispose();
        }

        [TestMethod]
        public void 作業のCRUD操作を一通り()
        {
            var expectedCode = "code";
            var expectedName = "name";
            ProjectController.Create(expectedCode, expectedName);
            var project = ProjectController.Read(expectedCode);
            ActivityController.Create(project, expectedCode, expectedName);

            var activity = ActivityController.Read(expectedCode);
            activity.ProjectId.Is(project.Id);
            activity.Code.Is(expectedCode);
            activity.Name.Is(expectedName);
            activity.State.Is(StateType.Unknown);
            activity.Income.Is(0m);
            activity.ExternalCost.Is(0m);
            activity.FixedLeadTime.Is(0m);
            activity.Workload.Is(0m);
            activity.RateOfFailure.Is(0m);
            activity.Assigns.Count().Is(0);

            var changedCode = "changedCode";
            var changedName = "changedName";
            var changedNumber = 1m;
            var changedType = StateType.ToDo;
            ProjectController.Create(changedCode, changedName);
            var changedProject = ProjectController.Read(changedCode);
            activity.ProjectId = changedProject.Id;
            activity.Code = changedCode;
            activity.Name = changedName;
            activity.State = changedType;
            activity.Income= changedNumber;
            activity.ExternalCost = changedNumber;
            activity.FixedLeadTime = changedNumber;
            activity.Workload = changedNumber;
            activity.RateOfFailure = changedNumber;
            ActivityController.Update(activity);

            var changedActivity = ActivityController.Read(changedCode);
            changedActivity.ProjectId.IsNot(project.Id);
            changedActivity.ProjectId.Is(changedProject.Id);
            changedActivity.Code.Is(changedCode);
            changedActivity.Name.Is(changedName);
            changedActivity.State.Is(changedType);
            changedActivity.Income.Is(changedNumber);
            changedActivity.ExternalCost.Is(changedNumber);
            changedActivity.FixedLeadTime.Is(changedNumber);
            changedActivity.Workload.Is(changedNumber);
            changedActivity.RateOfFailure.Is(changedNumber);
            changedActivity.Assigns.Count().Is(0);

            ActivityController.Delete(changedActivity);
            ActivityController.Read(changedCode).IsNull();
        }
    }
}