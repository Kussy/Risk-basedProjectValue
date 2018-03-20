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
            var activity = Activity.Define();
            activity.State.Is(State.ToDo);
        }

        [TestMethod]
        public void アクティビティの進捗は報告された状態であるべき()
        {
            var activity = Activity.Define();
            activity.Progress(State.Doing);
            activity.State.Is(State.Doing);
            activity.Progress(State.Done);
            activity.State.Is(State.Done);
        }

        [TestMethod]
        public void 見積前のアクティビティの収入はゼロであるべき()
        {
            var activity = Activity.Define();
            activity.Income.Value.Is(0m);
        }

        [TestMethod]
        public void 見積後のアクティビティの収入は与えられたものであるべき()
        {
            var expectedValue = 100m;
            var activity = Activity.Define();
            activity.Estimate(Income.Of(expectedValue));
            activity.Income.Value.Is(expectedValue);
        }

        [TestMethod]
        public void 見積前のアクティビティの支出はゼロであるべき()
        {
            var activity = Activity.Define();
            activity.ExternalCost.Value.Is(0m);
        }

        [TestMethod]
        public void 見積後のアクティビティの支出は与えられたものであるべき()
        {
            var expectedValue = 100m;
            var activity = Activity.Define();
            activity.Estimate(Cost.Of(expectedValue));
            activity.ExternalCost.Value.Is(expectedValue);
        }

        [TestMethod]
        public void 見積前のアクティビティの作業量はゼロであるべき()
        {
            var activity = Activity.Define();
            activity.WorkLoad.Value.Is(0m);
        }

        [TestMethod]
        public void 見積後のアクティビティの作業量は与えられたものであるべき()
        {
            var expectedValue = 100m;

            var activity = Activity.Define();
            activity.Estimate(WorkLoad.Of(expectedValue));
            activity.WorkLoad.Value.Is(expectedValue);
        }

        [TestMethod]
        public void 見積前のアクティビティの固定時間はゼロであるべき()
        {
            var activity = Activity.Define();
            activity.FixTime.Value.Is(0m);
        }

        [TestMethod]
        public void 見積後のアクティビティの固定時間は与えられたものであるべき()
        {
            var expectedValue = 100m;

            var activity = Activity.Define();
            activity.Estimate(LeadTime.Of(expectedValue));
            activity.FixTime.Value.Is(expectedValue);
        }

        [TestMethod]
        public void 見積前のアクティビティのリスクはゼロであるべき()
        {
            var activity = Activity.Define();
            activity.Risk.FailRate.Is(0m);
            activity.Risk.CostOverRate.Is(0m);
            activity.Risk.ReworkRate.Is(0m);
        }

        [TestMethod]
        public void 見積後のアクティビティのリスクは与えられたものであるべき()
        {
            var expectedFailRate = 0.3m;
            var expectedReworkRate = 0.4m;
            var expectedCostOverRate = 0.5m;

            var activity = Activity.Define();
            activity.Estimate(Risk.Of(expectedFailRate, expectedReworkRate, expectedCostOverRate));
            activity.Risk.FailRate.Is(expectedFailRate);
            activity.Risk.ReworkRate.Is(expectedReworkRate);
            activity.Risk.CostOverRate.Is(expectedCostOverRate);
        }

        [TestMethod]
        public void アクティビティの初期状態は資源未割当であるべき()
        {
            var activity = Activity.Define();
            activity.Resources.IsNotNull();
            activity.Resources.Count().Is(0);
        }

        [TestMethod]
        public void アクティビティの資源割当は追加的であるべき()
        {
            var activity = Activity.Define();
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
            var activity = Activity.Define();
            var resources = new[] { Resource.Of(1m, 1m) };
            activity.Assign(resources);
            activity.UnAssign();
            activity.Resources.IsNotNull();
            activity.Resources.Count().Is(0);
        }

        [TestMethod]
        public void アクティビティの初期状態は先行も後続も存在しないべき()
        {
            var activity = Activity.Define();
            activity.Parents.Count().Is(0);
            activity.Children.Count().Is(0);
        }

        [TestMethod]
        public void 後続アクティビティを指定すると親子の両方に関係が作られるべき()
        {
            var parentActivity = Activity.Define();
            var childActivity = Activity.Define();
            parentActivity.Precede(childActivity);

            parentActivity.Children.Count().Is(1);
            parentActivity.Children.First().Is(childActivity);
            childActivity.Parents.Count().Is(1);
            childActivity.Parents.First().Is(parentActivity);
        }

        [TestMethod]
        public void 先行アクティビティを指定すると親子の両方に関係が作られるべき()
        {
            var parentActivity = Activity.Define();
            var childActivity = Activity.Define();
            childActivity.Succeed(parentActivity);

            parentActivity.Children.Count().Is(1);
            parentActivity.Children.First().Is(childActivity);
            childActivity.Parents.Count().Is(1);
            childActivity.Parents.First().Is(parentActivity);
        }

        [TestMethod]
        public void 分岐アクティビティを指定すると親子の両方に関係が作られるべき()
        {
            var parentActivity = Activity.Define();
            var childActivity1 = Activity.Define();
            var childActivity2 = Activity.Define();
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
            var parentActivity1 = Activity.Define();
            var parentActivity2 = Activity.Define();
            var childActivity = Activity.Define();
            childActivity.Merge(new[] { parentActivity1, parentActivity2 });

            childActivity.Parents.Count().Is(2);
            childActivity.Parents.Contains(parentActivity1).Is(true);
            childActivity.Parents.Contains(parentActivity2).Is(true);
            parentActivity1.Children.Count().Is(1);
            parentActivity1.Children.First().Is(childActivity);
            parentActivity2.Children.Count().Is(1);
            parentActivity2.Children.First().Is(childActivity);
        }

        [TestMethod]
        public void 三段階プロジェクトの最終アクティビティの先祖は二つであるべき()
        {
            var activityDesign = Activity.Define();
            var activityProduct = Activity.Define();
            var activitySales = Activity.Define();
            activityProduct.Succeed(activityDesign);
            activitySales.Succeed(activityProduct);

            activityDesign.Ancestors.Count().Is(0);
            activityProduct.Ancestors.Count().Is(1);
            activitySales.Ancestors.Count().Is(2);
        }

        [TestMethod]
        public void 三段階プロジェクトの最初アクティビティの子孫は二つであるべき()
        {
            var activityDesign = Activity.Define();
            var activityProduct = Activity.Define();
            var activitySales = Activity.Define();
            activityProduct.Succeed(activityDesign);
            activitySales.Succeed(activityProduct);

            activityDesign.Descendants.Count().Is(2);
            activityProduct.Descendants.Count().Is(1);
            activitySales.Descendants.Count().Is(0);
        }

        [TestMethod]
        public void 単一アクティビティの貢献価値はリスク確率と収入によって決まるべきべき()
        {
            var activity = Activity.Define(income: 100m, failRate: 0.5m);
            activity.ContributedValue().Value.Is(50m);
        }

        [TestMethod]
        public void 単一アクティビティの将来キャッシュフロー期待値はゼロであるべきべき()
        {
            var activity = Activity.Define(income: 100m, externalCost: 20m, failRate: 0.5m);
            activity.ExpectedFutureCachFlow().Value.Is(0m);
        }

        [TestMethod]
        public void 段階的プロジェクト１の貢献価値はアクティビティ期待値は論文と同じものであるべき()
        {
            var activityProduct = Activity.Define(income: 0m, externalCost: 20m, failRate: 0.1m);
            var activitySales = Activity.Define(income: 100m, externalCost: 0m, failRate: 0.5m);
            activityProduct.Precede(activitySales);

            activityProduct.ContributedValue().Value.Is(5m);
            activitySales.ContributedValue().Value.Is(50m);
        }

        [TestMethod]
        public void 段階的プロジェクト２の貢献価値はアクティビティ期待値は論文と同じものであるべき()
        {
            var activityProduct = Activity.Define(income: 0m, externalCost: 20m, failRate: 0.5m);
            var activitySales = Activity.Define(income: 100m, externalCost: 0m, failRate: 0.5m);
            activityProduct.Precede(activitySales);

            activityProduct.ContributedValue().Value.Is(25m);
            activitySales.ContributedValue().Value.Is(50m);
        }

        [TestMethod]
        public void 段階的プロジェクト３の貢献価値はアクティビティ期待値は論文と同じものであるべき()
        {
            var activityProduct = Activity.Define(income: 100m, externalCost: 20m, failRate: 0.1m);
            var activitySales = Activity.Define(income: 0m, externalCost: 0m, failRate: 0.5m);
            activityProduct.Succeed(activitySales);

            activityProduct.ContributedValue().Value.Is(10m);
            activitySales.ContributedValue().Value.Is(35m);
        }

        [TestMethod]
        public void 単純プロジェクトのCPMはアクティビティの値そのものであるべき()
        {
            var activity = Activity.Define(fixTime: 5m);
            activity.EarliestStart().Value.Is(0m);
            activity.EarliestFinish().Value.Is(5m);
            activity.LatestStart().Value.Is(0m);
            activity.LatestFinish().Value.Is(5m);
            activity.Float().Value.Is(0m);
        }

        [TestMethod]
        public void 段階的プロジェクトのCPMはアクティビティの値の和であるべき()
        {
            var activity1 = Activity.Define(fixTime: 5m);
            var activity2 = Activity.Define(workLoad: 10m);
            activity2.Assign(new[] { Resource.Of(quantity: 5m, productivity: 1m) });
            activity1.Precede(activity2);

            activity1.EarliestStart().Value.Is(0m);
            activity1.EarliestFinish().Value.Is(5m);
            activity1.LatestStart().Value.Is(0m);
            activity1.LatestFinish().Value.Is(5m);
            activity1.Float().Value.Is(0m);

            activity2.EarliestStart().Value.Is(5m);
            activity2.EarliestFinish().Value.Is(7m);
            activity2.LatestStart().Value.Is(5m);
            activity2.LatestFinish().Value.Is(7m);
            activity2.Float().Value.Is(0m);
        }

        [TestMethod]
        public void 分岐ありプロジェクトのCPMはアクティビティの値の最大値であるべき()
        {
            var activity1 = Activity.Define(fixTime: 5m);
            var activity2 = Activity.Define(workLoad: 10m);
            activity2.Assign(new[] { Resource.Of(quantity: 5m, productivity: 1m) });
            var activity3 = Activity.Define(fixTime: 3m);
            activity3.Merge(new[] { activity1, activity2 });

            activity1.EarliestStart().Value.Is(0m);
            activity1.EarliestFinish().Value.Is(5m);
            activity1.LatestStart().Value.Is(0m);
            activity1.LatestFinish().Value.Is(5m);
            activity1.Float().Value.Is(0m);

            activity2.EarliestStart().Value.Is(0m);
            activity2.EarliestFinish().Value.Is(2m);
            activity2.LatestStart().Value.Is(3m);
            activity2.LatestFinish().Value.Is(5m);
            activity2.Float().Value.Is(3m);

            activity3.EarliestStart().Value.Is(5m);
            activity3.EarliestFinish().Value.Is(8m);
            activity3.LatestStart().Value.Is(5m);
            activity3.LatestFinish().Value.Is(8m);
            activity3.Float().Value.Is(0m);
        }
    }
}