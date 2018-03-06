using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kussy.Analysis.Project.Core
{
    [TestClass]
    public class TestProject
    {
        [TestMethod]
        public void 段階的プロジェクト１のRPVは論文と同じものであるべき()
        {
            var activityProduct = new Activity();
            activityProduct.Estimate(Income.Of(0m, Currency.JPY));
            activityProduct.Estimate(Cost.Of(20m, Currency.JPY));
            activityProduct.Estimate(Risk.Of(0.1m, 0m, 0m));
            var activitySales = new Activity();
            activitySales.Estimate(Income.Of(100m, Currency.JPY));
            activitySales.Estimate(Cost.Of(0m, Currency.JPY));
            activitySales.Estimate(Risk.Of(0.5m, 0m, 0m));
            activityProduct.Precede(activitySales);
            var project = new Project();
            project.AddActivities(new[] { activityProduct, activitySales });

            project.RPVstart().Value.Is(25m);
            project.RPVfinish().Value.Is(80m);
        }

        [TestMethod]
        public void 段階的プロジェクト２のRPVは論文と同じものであるべき()
        {
            var activityProduct = new Activity();
            activityProduct.Estimate(Income.Of(0m, Currency.JPY));
            activityProduct.Estimate(Cost.Of(20m, Currency.JPY));
            activityProduct.Estimate(Risk.Of(0.5m, 0m, 0m));
            var activitySales = new Activity();
            activitySales.Estimate(Income.Of(100m, Currency.JPY));
            activitySales.Estimate(Cost.Of(0m, Currency.JPY));
            activitySales.Estimate(Risk.Of(0.5m, 0m, 0m));
            activityProduct.Precede(activitySales);
            var project = new Project();
            project.AddActivities(new[] { activityProduct, activitySales });

            project.RPVstart().Value.Is(5m);
            project.RPVfinish().Value.Is(80m);
        }

        [TestMethod]
        public void 段階的プロジェクト３のRPVは論文と同じものであるべき()
        {
            var activityProduct = new Activity();
            activityProduct.Estimate(Income.Of(100m, Currency.JPY));
            activityProduct.Estimate(Cost.Of(20m, Currency.JPY));
            activityProduct.Estimate(Risk.Of(0.1m, 0m, 0m));
            var activitySales = new Activity();
            activitySales.Estimate(Income.Of(0m, Currency.JPY));
            activitySales.Estimate(Cost.Of(0m, Currency.JPY));
            activitySales.Estimate(Risk.Of(0.5m, 0m, 0m));
            activityProduct.Succeed(activitySales);
            var project = new Project();
            project.AddActivities(new[] { activityProduct, activitySales });

            project.RPVstart().Value.Is(35m);
            project.RPVfinish().Value.Is(80m);
        }
    }
}
