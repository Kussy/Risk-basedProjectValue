using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kussy.Analysis.Project.Core
{
    [TestClass]
    public class TestActivity
    {
        [TestMethod]
        public void アクティビティの初期状態は未着手であるべき()
        {
            var activity = new Activity();
            activity.State.Is(State.ToDo);
        }

        [TestMethod]
        public void アクティビティの進捗は報告された状態であるべき()
        {
            var activity = new Activity();
            activity.Progress(State.Doing);
            activity.State.Is(State.Doing);
            activity.Progress(State.Done);
            activity.State.Is(State.Done);
        }

        [TestMethod]
        public void 見積前のアクティビティの収入はゼロであるべき()
        {
            var activity = new Activity();
            activity.Income.Value.Is(0m);
        }

        [TestMethod]
        public void 見積後のアクティビティの収入は与えられたものであるべき()
        {
            var expectedValue = 100m;
            var expectedCurrency = Currency.USD;
            var activity = new Activity();
            activity.Estimate(Income.Of(expectedValue, expectedCurrency));
            activity.Income.Value.Is(expectedValue);
            activity.Income.Currency.Is(expectedCurrency);
        }

        [TestMethod]
        public void 見積前のアクティビティの支出はゼロであるべき()
        {
            var activity = new Activity();
            activity.DirectCost.Value.Is(0m);
        }

        [TestMethod]
        public void 見積後のアクティビティの支出は与えられたものであるべき()
        {
            var expectedValue = 100m;
            var expectedCurrency = Currency.USD;
            var activity = new Activity();
            activity.Estimate(Cost.Of(expectedValue, expectedCurrency));
            activity.DirectCost.Value.Is(expectedValue);
            activity.DirectCost.Currency.Is(expectedCurrency);
        }

        [TestMethod]
        public void 見積前のアクティビティの作業量はゼロであるべき()
        {
            var activity = new Activity();
            activity.WorkLoad.Value.Is(0m);
        }

        [TestMethod]
        public void 見積後のアクティビティの作業量は与えられたものであるべき()
        {
            var expectedValue = 100m;
            var expectedTimeType = TimeType.Day;
            var expectedWorkerType = ResourceType.Human;

            var activity = new Activity();
            activity.Estimate(WorkLoad.Of(expectedValue, expectedTimeType, expectedWorkerType));
            activity.WorkLoad.Value.Is(expectedValue);
            activity.WorkLoad.TimeUnit.Is(expectedTimeType);
            activity.WorkLoad.ResourceUnit.Is(expectedWorkerType);
        }

        [TestMethod]
        public void 見積前のアクティビティの固定時間はゼロであるべき()
        {
            var activity = new Activity();
            activity.FixTime.Value.Is(0m);
            activity.FixTime.TimeUnit.Is(TimeType.Day);
        }

        [TestMethod]
        public void 見積後のアクティビティの固定時間は与えられたものであるべき()
        {
            var expectedValue = 100m;
            var expectedTimeType = TimeType.Day;

            var activity = new Activity();
            activity.Estimate(LeadTime.Of(expectedValue, expectedTimeType));
            activity.FixTime.Value.Is(expectedValue);
            activity.FixTime.TimeUnit.Is(expectedTimeType);
        }

        [TestMethod]
        public void 見積前のアクティビティのリスクはゼロであるべき()
        {
            var activity = new Activity();
            activity.Risk.FailRate.Is(0m);
            activity.Risk.CostOverRate.Is(0m);
            activity.Risk.ReworkRate.Is(0m);
        }

        [TestMethod]
        public void 見積後のアクティビティのリスクは与えられたものであるべき()
        {
            var expectedFailRate= 0.3m;
            var expectedReworkRate = 0.4m;
            var expectedCostOverRate = 0.5m;

            var activity = new Activity();
            activity.Estimate(Risk.Of(expectedFailRate, expectedReworkRate, expectedCostOverRate));
            activity.Risk.FailRate.Is(expectedFailRate);
            activity.Risk.ReworkRate.Is(expectedReworkRate);
            activity.Risk.CostOverRate.Is(expectedCostOverRate);
        }

        [TestMethod]
        public void アクティビティの初期状態は資源未割当であるべき()
        {
            var activity = new Activity();
            activity.Resources.IsNotNull();
            activity.Resources.Count().Is(0);
        }

        [TestMethod]
        public void アクティビティの資源割当は追加的であるべき()
        {
            var activity = new Activity();
            var resources = new[] { Resource.Of(1m, 1m) };
            activity.Assign(resources);
            activity.Resources.Count().Is(1);
            resources = new[] { Resource.Of(2m, 2m) };
            activity.Assign(resources);
            activity.Resources.Count().Is(2);
        }

        [TestMethod]
        public void アクティビティの資源割当解除を行うと初期状態に戻るべき()
        {
            var activity = new Activity();
            var resources = new[] { Resource.Of(1m, 1m) };
            activity.Assign(resources);
            activity.UnAssign();
            activity.Resources.IsNotNull();
            activity.Resources.Count().Is(0);
        }

        [TestMethod]
        public void アクティビティの初期状態は先行も後続も存在しないべき()
        {
            var activity = new Activity();
            activity.Parents.Count().Is(0);
            activity.Children.Count().Is(0);
        }

        [TestMethod]
        public void 後続アクティビティを指定すると親子の両方に関係が作られるべき()
        {
            var parentActivity = new Activity();
            var childActivity = new Activity();
            parentActivity.Precede(childActivity);

            parentActivity.Children.Count().Is(1);
            parentActivity.Children.First().Is(childActivity);
            childActivity.Parents.Count().Is(1);
            childActivity.Parents.First().Is(parentActivity);
        }

        [TestMethod]
        public void 先行アクティビティを指定すると親子の両方に関係が作られるべき()
        {
            var parentActivity = new Activity();
            var childActivity = new Activity();
            childActivity.Succeed(parentActivity);

            parentActivity.Children.Count().Is(1);
            parentActivity.Children.First().Is(childActivity);
            childActivity.Parents.Count().Is(1);
            childActivity.Parents.First().Is(parentActivity);
        }

        [TestMethod]
        public void 分岐アクティビティを指定すると親子の両方に関係が作られるべき()
        {
            var parentActivity = new Activity();
            var childActivity1 = new Activity();
            var childActivity2 = new Activity();
            parentActivity.Branch(new[] { childActivity1, childActivity2 });

            parentActivity.Children.Count().Is(2);
            parentActivity.Children.Contains(childActivity1).Is(true);
            parentActivity.Children.Contains(childActivity2).Is(true);
            childActivity1.Parents.Count().Is(1);
            childActivity1.Parents.First().Is(parentActivity);
            childActivity2.Parents.Count().Is(1);
            childActivity2.Parents.First().Is(parentActivity);
        }

        [TestMethod]
        public void 合流アクティビティを指定すると親子の両方に関係が作られるべき()
        {
            var parentActivity1 = new Activity();
            var parentActivity2 = new Activity();
            var childActivity = new Activity();
            childActivity.Merge(new[] { parentActivity1, parentActivity2 });

            childActivity.Parents.Count().Is(2);
            childActivity.Parents.Contains(parentActivity1).Is(true);
            childActivity.Parents.Contains(parentActivity2).Is(true);
            parentActivity1.Children.Count().Is(1);
            parentActivity1.Children.First().Is(childActivity);
            parentActivity2.Children.Count().Is(1);
            parentActivity2.Children.First().Is(childActivity);
        }
    }
}
