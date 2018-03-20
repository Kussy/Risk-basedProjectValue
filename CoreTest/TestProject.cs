using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kussy.Analysis.Project.Core
{
    [TestClass]
    public class TestProject
    {
        [TestMethod]
        public void プロジェクト作成時は開始と完了のアクティビティのみ存在するべき()
        {
            var project = Project.Define();
            project.Activities.Count().Is(2);
            project.Start.Parents.Count().Is(0);
            project.End.Children.Count().Is(0);
        }

        [TestMethod]
        public void 先頭アクティビティが複数の場合でもプロジェクト完了時間は遅い方を考慮すべき()
        {
            var hoge = Activity.Define(fixTime: 3);
            var fuga = Activity.Define(fixTime: 5);
            var project = Project.Define();
            project.Add(hoge, fuga);
            hoge.EarliestStart().Value.Is(0);
            fuga.EarliestStart().Value.Is(0);
            hoge.EarliestFinish().Value.Is(3);
            fuga.EarliestFinish().Value.Is(5);
            hoge.LatestStart().Value.Is(2);
            fuga.LatestStart().Value.Is(0);
            hoge.LatestFinish().Value.Is(5);
            fuga.LatestFinish().Value.Is(5);
        }


        [TestMethod]
        public void 単純プロジェクトのRPVは論文と同じものであるべき()
        {
            var activity = Activity.Define(income: 100m, externalCost: 20m, failRate: 0.5m);
            var project = Project.Define();
            project.Add(activity);
            project.RPVstart().Value.Is(30m);
            project.RPVfinish().Value.Is(80m);
        }

        [TestMethod]
        public void 段階的プロジェクト１のRPVは論文と同じものであるべき()
        {
            var activityProduct = Activity.Define(income: 0m, externalCost: 20m, failRate: 0.1m);
            var activitySales = Activity.Define(income: 100m, externalCost: 0m, failRate: 0.5m);
            activityProduct.Precede(activitySales);
            var project = Project.Define();
            project.Add(new[] { activityProduct, activitySales });

            project.RPVstart().Value.Is(25m);
            project.RPVfinish().Value.Is(80m);
        }

        [TestMethod]
        public void 段階的プロジェクト２のRPVは論文と同じものであるべき()
        {
            var activityProduct = Activity.Define(income: 0m, externalCost: 20m, failRate: 0.5m);
            var activitySales = Activity.Define(income: 100m, externalCost: 0m, failRate: 0.5m);
            activityProduct.Precede(activitySales);
            var project = Project.Define();
            project.Add(activityProduct, activitySales);

            project.RPVstart().Value.Is(5m);
            project.RPVfinish().Value.Is(80m);
        }

        [TestMethod]
        public void 段階的プロジェクト３のRPVは論文と同じものであるべき()
        {
            var activityProduct = Activity.Define(income: 100m, externalCost: 20m, failRate: 0.1m);
            var activitySales = Activity.Define(income: 0m, externalCost: 0m, failRate: 0.5m);
            activityProduct.Succeed(activitySales);
            var project = Project.Define();
            project.Add(activityProduct, activitySales);

            project.RPVstart().Value.Is(35m);
            project.RPVfinish().Value.Is(80m);
        }

        [TestMethod]
        public void 三段階プロジェクトのRPVは期待されたものであるべき()
        {
            var activityDesign = Activity.Define(income: 100m, externalCost: 20m, failRate: 0.5m);
            var activityProduct = Activity.Define(income: 200m, externalCost: 80m, failRate: 0.5m);
            var activitySales = Activity.Define(income: 1000m, externalCost: 200m, failRate: 0.1m);
            activityProduct.Succeed(activityDesign);
            activitySales.Succeed(activityProduct);
            var project = Project.Define();
            project.Add(activityDesign, activityProduct, activitySales);

            project.RPVstart().Value.Is(215m);
            project.RPVfinish().Value.Is(1000m);
        }


        [TestMethod]
        public void 三段階プロジェクトの途中RPVは貢献価値が反映されたものであるべき()
        {
            var activityDesign = Activity.Define(income: 100m, externalCost: 20m, failRate: 0.5m);
            var activityProduct = Activity.Define(income: 200m, externalCost: 80m, failRate: 0.5m);
            var activitySales = Activity.Define(income: 1000m, externalCost: 200m, failRate: 0.1m);
            activityProduct.Succeed(activityDesign);
            activitySales.Succeed(activityProduct);
            var project = Project.Define();
            project.Add(activityDesign, activityProduct, activitySales);

            activityDesign.ContributedValue().Value.Is(235m);
            activityProduct.ContributedValue().Value.Is(450m);
            activitySales.ContributedValue().Value.Is(100m);

            activityDesign.Progress(State.Done);
            project.RPV().Value.Is(450m);
            activityProduct.Progress(State.Done);
            project.RPV().Value.Is(900m);
            activitySales.Progress(State.Done);
            project.RPV().Value.Is(1000m);
        }

        [TestMethod]
        public void プロジェクトのDRAGは定義を反映したものであるべき()
        {
            var liquidatedDamages = Money.Of(5m);
            var basicDesign = Activity.Define(fixTime: 20);
            var hardProcurement = Activity.Define(fixTime: 35);
            var detailDesign = Activity.Define(fixTime: 10);
            var hardConfiguration = Activity.Define(fixTime: 5);
            var develop = Activity.Define(fixTime: 20);
            var testing = Activity.Define(fixTime: 15);

            basicDesign.Precede(hardProcurement, detailDesign);
            hardProcurement.Precede(hardConfiguration);
            detailDesign.Precede(develop);
            testing.Succeed(hardConfiguration, develop);

            var project = Project.Define();
            project.Add(basicDesign, hardProcurement, hardConfiguration, detailDesign, develop, testing);

            basicDesign.Drag().Value.Is(20m);
            basicDesign.DragCost(liquidatedDamages).Value.Is(100m);
            hardProcurement.Drag().Value.Is(10m);
            hardProcurement.DragCost(liquidatedDamages).Value.Is(50m);
            hardConfiguration.Drag().Value.Is(5m);
            hardConfiguration.DragCost(liquidatedDamages).Value.Is(25m);
            detailDesign.Drag().Value.Is(0m);
            detailDesign.DragCost(liquidatedDamages).Value.Is(0m);
            develop.Drag().Value.Is(0m);
            develop.DragCost(liquidatedDamages).Value.Is(0m);
            testing.Drag().Value.Is(15m);
            testing.DragCost(liquidatedDamages).Value.Is(75m);
        }

        [TestMethod]
        public void プロジェクトの本質的コストはDRAGと作業量を反映したものであるべき()
        {
            var liquidatedDamages = Money.Of(5m);
            var basicDesign = Activity.Define(fixTime: 20, externalCost: 10);
            var hardProcurement = Activity.Define(fixTime: 35, externalCost: 100);
            var detailDesign = Activity.Define(fixTime: 10, externalCost: 10);
            var hardConfiguration = Activity.Define(fixTime: 5, externalCost: 50);
            var develop = Activity.Define(fixTime: 20, externalCost: 100);
            var testing = Activity.Define(fixTime: 15, externalCost: 50);

            basicDesign.Precede(hardProcurement, detailDesign);
            hardProcurement.Precede(hardConfiguration);
            detailDesign.Precede(develop);
            testing.Succeed(hardConfiguration, develop);

            var project = Project.Define(liquidatedDamages: 5m);
            project.Add(basicDesign, hardProcurement, hardConfiguration, detailDesign, develop, testing);

            basicDesign.IntrinsicCost(project.LiquidatedDamages).Value.Is(110m);
            hardProcurement.IntrinsicCost(project.LiquidatedDamages).Value.Is(150m);
            hardConfiguration.IntrinsicCost(project.LiquidatedDamages).Value.Is(75m);
            detailDesign.IntrinsicCost(project.LiquidatedDamages).Value.Is(10m);
            develop.IntrinsicCost(project.LiquidatedDamages).Value.Is(100m);
            testing.IntrinsicCost(project.LiquidatedDamages).Value.Is(125m);
        }
    }
}