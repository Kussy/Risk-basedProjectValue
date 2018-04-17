using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kussy.Analysis.Project.Core
{
    [TestClass]
    public class TestEPCProject
    {
        [TestMethod]
        public void EPCプロジェクトのCPMの結果が手動で計算した値と一致するべき()
        {
            #region Arrange
            var project = Create();
            var a_12BD01 = project.Activities.First(a => a.Id == "12BD01");
            var a_01CS01 = project.Activities.First(a => a.Id == "01CS01");
            var a_15DE01 = project.Activities.First(a => a.Id == "15DE01");
            var a_17DE01 = project.Activities.First(a => a.Id == "17DE01");
            var a_12BD02 = project.Activities.First(a => a.Id == "12BD02");
            var a_16BD01 = project.Activities.First(a => a.Id == "16BD01");
            var a_14DE01 = project.Activities.First(a => a.Id == "14DE01");
            var a_15DE02 = project.Activities.First(a => a.Id == "15DE02");
            var a_17DE02 = project.Activities.First(a => a.Id == "17DE02");
            var a_16PR01 = project.Activities.First(a => a.Id == "16PR01");
            var a_16DE01 = project.Activities.First(a => a.Id == "16DE01");
            var a_15PR01 = project.Activities.First(a => a.Id == "15PR01");
            var a_14PR01 = project.Activities.First(a => a.Id == "14PR01");
            var a_16DE02 = project.Activities.First(a => a.Id == "16DE02");
            var a_13DE01 = project.Activities.First(a => a.Id == "13DE01");
            var a_16DE03 = project.Activities.First(a => a.Id == "16DE03");
            var a_13CS01 = project.Activities.First(a => a.Id == "13CS01");
            var a_17PR01 = project.Activities.First(a => a.Id == "17PR01");
            var a_13CS02 = project.Activities.First(a => a.Id == "13CS02");
            var a_14CS01 = project.Activities.First(a => a.Id == "14CS01");
            var a_16CS01 = project.Activities.First(a => a.Id == "16CS01");
            var a_16CS02 = project.Activities.First(a => a.Id == "16CS02");
            var a_15CS01 = project.Activities.First(a => a.Id == "15CS01");
            var a_16CS03 = project.Activities.First(a => a.Id == "16CS03");
            var a_17CS01 = project.Activities.First(a => a.Id == "17CS01");
            var a_12CM01 = project.Activities.First(a => a.Id == "12CM01");
            #endregion

            #region Act Assert
            a_12BD01.EarliestStart().Value.Is(0m);
            a_12BD01.EarliestFinish().Value.Is(3m);
            a_12BD01.LatestStart().Value.Is(0m);
            a_12BD01.LatestFinish().Value.Is(3m);
            a_12BD01.TotalFloat().Value.Is(0m);
            a_12BD01.FreeFloat().Value.Is(0m);
            a_12BD01.Drag().Value.Is(0.5m);
            a_12BD01.IsInCriticalPath().Is(true);

            a_12BD02.EarliestStart().Value.Is(3m);
            a_12BD02.EarliestFinish().Value.Is(10m);
            a_12BD02.LatestStart().Value.Is(3.5m);
            a_12BD02.LatestFinish().Value.Is(10.5m);
            a_12BD02.TotalFloat().Value.Is(0.5m);
            a_12BD02.FreeFloat().Value.Is(0.5m);
            a_12BD02.Drag().Value.Is(0m);
            a_12BD02.IsInCriticalPath().Is(false);

            a_15DE01.EarliestStart().Value.Is(3m);
            a_15DE01.EarliestFinish().Value.Is(7m);
            a_15DE01.LatestStart().Value.Is(3m);
            a_15DE01.LatestFinish().Value.Is(7m);
            a_15DE01.TotalFloat().Value.Is(0m);
            a_15DE01.FreeFloat().Value.Is(0m);
            a_15DE01.Drag().Value.Is(0m);
            a_15DE01.IsInCriticalPath().Is(true);

            a_16BD01.EarliestStart().Value.Is(3m);
            a_16BD01.EarliestFinish().Value.Is(8.5m);
            a_16BD01.LatestStart().Value.Is(4m);
            a_16BD01.LatestFinish().Value.Is(9.5m);
            a_16BD01.TotalFloat().Value.Is(1m);
            a_16BD01.FreeFloat().Value.Is(0m);
            a_16BD01.Drag().Value.Is(0m);
            a_16BD01.IsInCriticalPath().Is(false);

            a_17DE01.EarliestStart().Value.Is(3m);
            a_17DE01.EarliestFinish().Value.Is(8.5m);
            a_17DE01.LatestStart().Value.Is(3m);
            a_17DE01.LatestFinish().Value.Is(8.5m);
            a_17DE01.TotalFloat().Value.Is(0m);
            a_17DE01.FreeFloat().Value.Is(0m);
            a_17DE01.Drag().Value.Is(0m);
            a_17DE01.IsInCriticalPath().Is(true);

            a_14DE01.EarliestStart().Value.Is(7m);
            a_14DE01.EarliestFinish().Value.Is(12m);
            a_14DE01.LatestStart().Value.Is(7m);
            a_14DE01.LatestFinish().Value.Is(12m);
            a_14DE01.TotalFloat().Value.Is(0m);
            a_14DE01.FreeFloat().Value.Is(0m);
            a_14DE01.Drag().Value.Is(0m);
            a_14DE01.IsInCriticalPath().Is(true);

            a_15DE02.EarliestStart().Value.Is(7m);
            a_15DE02.EarliestFinish().Value.Is(9m);
            a_15DE02.LatestStart().Value.Is(8.5m);
            a_15DE02.LatestFinish().Value.Is(10.5m);
            a_15DE02.TotalFloat().Value.Is(1.5m);
            a_15DE02.FreeFloat().Value.Is(0m);
            a_15DE02.Drag().Value.Is(0m);
            a_15DE02.IsInCriticalPath().Is(false);

            a_16PR01.EarliestStart().Value.Is(8.5m);
            a_16PR01.EarliestFinish().Value.Is(18.5m);
            a_16PR01.LatestStart().Value.Is(9.5m);
            a_16PR01.LatestFinish().Value.Is(19.5m);
            a_16PR01.TotalFloat().Value.Is(1m);
            a_16PR01.FreeFloat().Value.Is(1m);
            a_16PR01.Drag().Value.Is(0m);
            a_16PR01.IsInCriticalPath().Is(false);

            a_17DE02.EarliestStart().Value.Is(8.5m);
            a_17DE02.EarliestFinish().Value.Is(10.5m);
            a_17DE02.LatestStart().Value.Is(8.5m);
            a_17DE02.LatestFinish().Value.Is(10.5m);
            a_17DE02.TotalFloat().Value.Is(0m);
            a_17DE02.FreeFloat().Value.Is(0m);
            a_17DE02.Drag().Value.Is(0m);
            a_17DE02.IsInCriticalPath().Is(true);

            a_01CS01.EarliestStart().Value.Is(0m);
            a_01CS01.EarliestFinish().Value.Is(13.5m);
            a_01CS01.LatestStart().Value.Is(0.5m);
            a_01CS01.LatestFinish().Value.Is(14m);
            a_01CS01.TotalFloat().Value.Is(0.5m);
            a_01CS01.FreeFloat().Value.Is(0m);
            a_01CS01.Drag().Value.Is(0m);
            a_01CS01.IsInCriticalPath().Is(false);

            a_13DE01.EarliestStart().Value.Is(12m);
            a_13DE01.EarliestFinish().Value.Is(13m);
            a_13DE01.LatestStart().Value.Is(13m);
            a_13DE01.LatestFinish().Value.Is(14m);
            a_13DE01.TotalFloat().Value.Is(1m);
            a_13DE01.FreeFloat().Value.Is(0.5m);
            a_13DE01.Drag().Value.Is(0m);
            a_13DE01.IsInCriticalPath().Is(false);

            a_14PR01.EarliestStart().Value.Is(12m);
            a_14PR01.EarliestFinish().Value.Is(19m);
            a_14PR01.LatestStart().Value.Is(12m);
            a_14PR01.LatestFinish().Value.Is(19m);
            a_14PR01.TotalFloat().Value.Is(0m);
            a_14PR01.FreeFloat().Value.Is(0m);
            a_14PR01.Drag().Value.Is(0m);
            a_14PR01.IsInCriticalPath().Is(true);

            a_15PR01.EarliestStart().Value.Is(9m);
            a_15PR01.EarliestFinish().Value.Is(23m);
            a_15PR01.LatestStart().Value.Is(12m);
            a_15PR01.LatestFinish().Value.Is(26m);
            a_15PR01.TotalFloat().Value.Is(3m);
            a_15PR01.FreeFloat().Value.Is(2.5m);
            a_15PR01.Drag().Value.Is(0m);
            a_15PR01.IsInCriticalPath().Is(false);

            a_16DE01.EarliestStart().Value.Is(10.5m);
            a_16DE01.EarliestFinish().Value.Is(12.5m);
            a_16DE01.LatestStart().Value.Is(10.5m);
            a_16DE01.LatestFinish().Value.Is(12.5m);
            a_16DE01.TotalFloat().Value.Is(0m);
            a_16DE01.FreeFloat().Value.Is(0m);
            a_16DE01.Drag().Value.Is(0m);
            a_16DE01.IsInCriticalPath().Is(true);

            a_17PR01.EarliestStart().Value.Is(10.5m);
            a_17PR01.EarliestFinish().Value.Is(25.5m);
            a_17PR01.LatestStart().Value.Is(15m);
            a_17PR01.LatestFinish().Value.Is(30m);
            a_17PR01.TotalFloat().Value.Is(4.5m);
            a_17PR01.FreeFloat().Value.Is(4.5m);
            a_17PR01.Drag().Value.Is(0m);
            a_17PR01.IsInCriticalPath().Is(false);

            a_13CS01.EarliestStart().Value.Is(13.5m);
            a_13CS01.EarliestFinish().Value.Is(18.5m);
            a_13CS01.LatestStart().Value.Is(14m);
            a_13CS01.LatestFinish().Value.Is(19m);
            a_13CS01.TotalFloat().Value.Is(0.5m);
            a_13CS01.FreeFloat().Value.Is(0m);
            a_13CS01.Drag().Value.Is(0m);
            a_13CS01.IsInCriticalPath().Is(false);

            a_16DE02.EarliestStart().Value.Is(12.5m);
            a_16DE02.EarliestFinish().Value.Is(13.5m);
            a_16DE02.LatestStart().Value.Is(12.5m);
            a_16DE02.LatestFinish().Value.Is(13.5m);
            a_16DE02.TotalFloat().Value.Is(0m);
            a_16DE02.FreeFloat().Value.Is(0m);
            a_16DE02.Drag().Value.Is(0m);
            a_16DE02.IsInCriticalPath().Is(true);

            a_13CS02.EarliestStart().Value.Is(18.5m);
            a_13CS02.EarliestFinish().Value.Is(25.5m);
            a_13CS02.LatestStart().Value.Is(19m);
            a_13CS02.LatestFinish().Value.Is(26m);
            a_13CS02.TotalFloat().Value.Is(0.5m);
            a_13CS02.FreeFloat().Value.Is(0m);
            a_13CS02.Drag().Value.Is(0m);
            a_13CS02.IsInCriticalPath().Is(false);

            a_14CS01.EarliestStart().Value.Is(19m);
            a_14CS01.EarliestFinish().Value.Is(24m);
            a_14CS01.LatestStart().Value.Is(19m);
            a_14CS01.LatestFinish().Value.Is(24m);
            a_14CS01.TotalFloat().Value.Is(0m);
            a_14CS01.FreeFloat().Value.Is(0m);
            a_14CS01.Drag().Value.Is(0m);
            a_14CS01.IsInCriticalPath().Is(true);

            a_16DE03.EarliestStart().Value.Is(13.5m);
            a_16DE03.EarliestFinish().Value.Is(19.5m);
            a_16DE03.LatestStart().Value.Is(13.5m);
            a_16DE03.LatestFinish().Value.Is(19.5m);
            a_16DE03.TotalFloat().Value.Is(0m);
            a_16DE03.FreeFloat().Value.Is(0m);
            a_16DE03.Drag().Value.Is(0m);
            a_16DE03.IsInCriticalPath().Is(true);

            a_15CS01.EarliestStart().Value.Is(25.5m);
            a_15CS01.EarliestFinish().Value.Is(29.5m);
            a_15CS01.LatestStart().Value.Is(26m);
            a_15CS01.LatestFinish().Value.Is(30m);
            a_15CS01.TotalFloat().Value.Is(0.5m);
            a_15CS01.FreeFloat().Value.Is(0.5m);
            a_15CS01.Drag().Value.Is(0m);
            a_15CS01.IsInCriticalPath().Is(false);

            a_16CS01.EarliestStart().Value.Is(19.5m);
            a_16CS01.EarliestFinish().Value.Is(24m);
            a_16CS01.LatestStart().Value.Is(19.5m);
            a_16CS01.LatestFinish().Value.Is(24m);
            a_16CS01.TotalFloat().Value.Is(0m);
            a_16CS01.FreeFloat().Value.Is(0m);
            a_16CS01.Drag().Value.Is(0m);
            a_16CS01.IsInCriticalPath().Is(true);

            a_16CS02.EarliestStart().Value.Is(24m);
            a_16CS02.EarliestFinish().Value.Is(30m);
            a_16CS02.LatestStart().Value.Is(24m);
            a_16CS02.LatestFinish().Value.Is(30m);
            a_16CS02.TotalFloat().Value.Is(0m);
            a_16CS02.FreeFloat().Value.Is(0m);
            a_16CS02.Drag().Value.Is(0.5m);
            a_16CS02.IsInCriticalPath().Is(true);

            a_16CS03.EarliestStart().Value.Is(30m);
            a_16CS03.EarliestFinish().Value.Is(36m);
            a_16CS03.LatestStart().Value.Is(30m);
            a_16CS03.LatestFinish().Value.Is(36m);
            a_16CS03.TotalFloat().Value.Is(0m);
            a_16CS03.FreeFloat().Value.Is(0m);
            a_16CS03.Drag().Value.Is(0m);
            a_16CS03.IsInCriticalPath().Is(true);

            a_17CS01.EarliestStart().Value.Is(30m);
            a_17CS01.EarliestFinish().Value.Is(36m);
            a_17CS01.LatestStart().Value.Is(30m);
            a_17CS01.LatestFinish().Value.Is(36m);
            a_17CS01.TotalFloat().Value.Is(0m);
            a_17CS01.FreeFloat().Value.Is(0m);
            a_17CS01.Drag().Value.Is(0m);
            a_17CS01.IsInCriticalPath().Is(true);

            a_12CM01.EarliestStart().Value.Is(36m);
            a_12CM01.EarliestFinish().Value.Is(42m);
            a_12CM01.LatestStart().Value.Is(36m);
            a_12CM01.LatestFinish().Value.Is(42m);
            a_12CM01.TotalFloat().Value.Is(0m);
            a_12CM01.FreeFloat().Value.Is(0m);
            a_12CM01.Drag().Value.Is(6m);
            a_12CM01.IsInCriticalPath().Is(true);
            #endregion
        }

        [TestMethod]
        public void EPCプロジェクトの全てのルートを求める()
        {
            #region Arrange
            var project = Create();
            var a_12BD01 = project.Activities.First(a => a.Id == "12BD01");
            var a_01CS01 = project.Activities.First(a => a.Id == "01CS01");
            var a_15DE01 = project.Activities.First(a => a.Id == "15DE01");
            var a_17DE01 = project.Activities.First(a => a.Id == "17DE01");
            var a_12BD02 = project.Activities.First(a => a.Id == "12BD02");
            var a_16BD01 = project.Activities.First(a => a.Id == "16BD01");
            var a_14DE01 = project.Activities.First(a => a.Id == "14DE01");
            var a_15DE02 = project.Activities.First(a => a.Id == "15DE02");
            var a_17DE02 = project.Activities.First(a => a.Id == "17DE02");
            var a_16PR01 = project.Activities.First(a => a.Id == "16PR01");
            var a_16DE01 = project.Activities.First(a => a.Id == "16DE01");
            var a_15PR01 = project.Activities.First(a => a.Id == "15PR01");
            var a_14PR01 = project.Activities.First(a => a.Id == "14PR01");
            var a_16DE02 = project.Activities.First(a => a.Id == "16DE02");
            var a_13DE01 = project.Activities.First(a => a.Id == "13DE01");
            var a_16DE03 = project.Activities.First(a => a.Id == "16DE03");
            var a_13CS01 = project.Activities.First(a => a.Id == "13CS01");
            var a_17PR01 = project.Activities.First(a => a.Id == "17PR01");
            var a_13CS02 = project.Activities.First(a => a.Id == "13CS02");
            var a_14CS01 = project.Activities.First(a => a.Id == "14CS01");
            var a_16CS01 = project.Activities.First(a => a.Id == "16CS01");
            var a_16CS02 = project.Activities.First(a => a.Id == "16CS02");
            var a_15CS01 = project.Activities.First(a => a.Id == "15CS01");
            var a_16CS03 = project.Activities.First(a => a.Id == "16CS03");
            var a_17CS01 = project.Activities.First(a => a.Id == "17CS01");
            var a_12CM01 = project.Activities.First(a => a.Id == "12CM01");

            var routes = project.Routes();
            #endregion
            project.RoutesVia(a_12BD01).Count().Is(21);
            project.RoutesVia(a_01CS01).Count().Is(3);
            project.RoutesVia(a_15DE01).Count().Is(9);
            project.RoutesVia(a_17DE01).Count().Is(4);
            project.RoutesVia(a_12BD02).Count().Is(3);
            project.RoutesVia(a_16BD01).Count().Is(5);
            project.RoutesVia(a_14DE01).Count().Is(5);
            project.RoutesVia(a_15DE02).Count().Is(4);
            project.RoutesVia(a_17DE02).Count().Is(4);
            project.RoutesVia(a_16PR01).Count().Is(2);
            project.RoutesVia(a_16DE01).Count().Is(12);
            project.RoutesVia(a_15PR01).Count().Is(1);
            project.RoutesVia(a_14PR01).Count().Is(2);
            project.RoutesVia(a_16DE02).Count().Is(12);
            project.RoutesVia(a_13DE01).Count().Is(3);
            project.RoutesVia(a_16DE03).Count().Is(8);
            project.RoutesVia(a_13CS01).Count().Is(6);
            project.RoutesVia(a_17PR01).Count().Is(1);
            project.RoutesVia(a_13CS02).Count().Is(2);
            project.RoutesVia(a_14CS01).Count().Is(6);
            project.RoutesVia(a_16CS01).Count().Is(10);
            project.RoutesVia(a_16CS02).Count().Is(16);
            project.RoutesVia(a_15CS01).Count().Is(7);
            project.RoutesVia(a_16CS03).Count().Is(15);
            project.RoutesVia(a_17CS01).Count().Is(9);
            project.RoutesVia(a_12CM01).Count().Is(24);
        }

        private static Project Create()
        {
            var project = Project.Define(unitOfTime: TimeType.Month, term: 42m, badjet: 1000m);
            var a_12BD01 = Activity.Define(id: "12BD01", name: "Process design - basic", fixTime: 3m);
            var a_01CS01 = Activity.Define(id: "01CS01", name: "Site Preparation", fixTime: 13.5m);
            var a_15DE01 = Activity.Define(id: "15DE01", name: "Equipment eng & vendor selection", fixTime: 4m);
            var a_17DE01 = Activity.Define(id: "17DE01", name: "E&I engineering - development", fixTime: 5.5m);
            var a_12BD02 = Activity.Define(id: "12BD02", name: "Process enginnering - development", fixTime: 7m);
            var a_16BD01 = Activity.Define(id: "16BD01", name: "Plant layout & piping engineering", fixTime: 5.5m);
            var a_14DE01 = Activity.Define(id: "14DE01", name: "Structure engineering", fixTime: 5m);
            var a_15DE02 = Activity.Define(id: "15DE02", name: "Equipment vendor eng & review", fixTime: 2m);
            var a_17DE02 = Activity.Define(id: "17DE02", name: "E&I vendor eng & review", fixTime: 2m);
            var a_16PR01 = Activity.Define(id: "16PR01", name: "Piping mat'l procurement & shipping 1st", fixTime: 10m);
            var a_16DE01 = Activity.Define(id: "16DE01", name: "3D modeling", fixTime: 2m);
            var a_15PR01 = Activity.Define(id: "15PR01", name: "Equipment manufacturing & shipping", fixTime: 14m);
            var a_14PR01 = Activity.Define(id: "14PR01", name: "Steel structure fabrication & shipping 1st", fixTime: 7m);
            var a_16DE02 = Activity.Define(id: "16DE02", name: "3D model review", fixTime: 1m);
            var a_13DE01 = Activity.Define(id: "13DE01", name: "Civil & pipe rack foundation engineering", fixTime: 1m);
            var a_16DE03 = Activity.Define(id: "16DE03", name: "Piping ISO drawing production 1st", fixTime: 6m);
            var a_13CS01 = Activity.Define(id: "13CS01", name: "Civil & pipe rack foundation construction", fixTime: 5m);
            var a_17PR01 = Activity.Define(id: "17PR01", name: "E&I manufacturing & shipping", fixTime: 15m);
            var a_13CS02 = Activity.Define(id: "13CS02", name: "Other civil & foundation constr.", fixTime: 7m);
            var a_14CS01 = Activity.Define(id: "14CS01", name: "Steel structure construction 1st", fixTime: 5m);
            var a_16CS01 = Activity.Define(id: "16CS01", name: "Piping prefabrication 1st", fixTime: 4.5m);
            var a_16CS02 = Activity.Define(id: "16CS02", name: "Piping on rack installation", fixTime: 6m);
            var a_15CS01 = Activity.Define(id: "15CS01", name: "Equipment erection", fixTime: 4m);
            var a_16CS03 = Activity.Define(id: "16CS03", name: "Piping a/Eq. installation", fixTime: 6m);
            var a_17CS01 = Activity.Define(id: "17CS01", name: "E&I a/Eq. construction", fixTime: 6m);
            var a_12CM01 = Activity.Define(id: "12CM01", name: "Pre-comm. & commissioning", fixTime: 6m);

            a_12BD01.Precede(a_12BD02, a_15DE01, a_16BD01, a_17DE01);
            a_12BD02.Precede(a_16DE01);
            a_15DE01.Precede(a_14DE01, a_15DE02);
            a_16BD01.Precede(a_16DE01, a_16PR01);
            a_17DE01.Precede(a_17DE02);
            a_14DE01.Precede(a_13DE01, a_14PR01);
            a_15DE02.Precede(a_15PR01, a_16DE01);
            a_16PR01.Precede(a_16CS01);
            a_17DE02.Precede(a_16DE01, a_17PR01);
            a_01CS01.Precede(a_13CS01);
            a_13DE01.Precede(a_13CS01);
            a_14PR01.Precede(a_14CS01);
            a_15PR01.Precede(a_15CS01);
            a_16DE01.Precede(a_16DE02);
            a_17PR01.Precede(a_17CS01);
            a_13CS01.Precede(a_13CS02, a_14CS01);
            a_16DE02.Precede(a_15CS01, a_16DE03);
            a_13CS02.Precede(a_15CS01);
            a_14CS01.Precede(a_16CS02);
            a_16DE03.Precede(a_16CS01);
            a_15CS01.Precede(a_16CS03);
            a_16CS01.Precede(a_16CS02);
            a_16CS02.Precede(a_16CS03, a_17CS01);
            a_16CS03.Precede(a_12CM01);
            a_17CS01.Precede(a_12CM01);

            project.Add(
                a_12BD01
                , a_12BD02
                , a_15DE01
                , a_16BD01
                , a_17DE01
                , a_14DE01
                , a_15DE02
                , a_16PR01
                , a_17DE02
                , a_01CS01
                , a_13DE01
                , a_14PR01
                , a_15PR01
                , a_16DE01
                , a_17PR01
                , a_13CS01
                , a_16DE02
                , a_13CS02
                , a_14CS01
                , a_16DE03
                , a_15CS01
                , a_16CS01
                , a_16CS02
                , a_16CS03
                , a_17CS01
                , a_12CM01
                );
            return project;
        }
    }
}
