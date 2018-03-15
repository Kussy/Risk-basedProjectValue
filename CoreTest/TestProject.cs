﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Kussy.Analysis.Project.Core
{
    [TestClass]
    public class TestProject
    {
        [TestMethod]
        public void 単純プロジェクトのRPVは論文と同じものであるべき()
        {
            var activity = TestHelper.Activity(income: 100m, directCost: 20m, failRate: 0.5m);
            var project = new Project();
            project.AddActivities(activity);
            project.RPVstart().Value.Is(30m);
            project.RPVfinish().Value.Is(80m);
        }

        [TestMethod]
        public void 段階的プロジェクト１のRPVは論文と同じものであるべき()
        {
            var activityProduct = TestHelper.Activity(income: 0m, directCost: 20m, failRate: 0.1m);
            var activitySales = TestHelper.Activity(income: 100m, directCost: 0m, failRate: 0.5m);
            activityProduct.Precede(activitySales);
            var project = new Project();
            project.AddActivities(new[] { activityProduct, activitySales });

            project.RPVstart().Value.Is(25m);
            project.RPVfinish().Value.Is(80m);
        }

        [TestMethod]
        public void 段階的プロジェクト２のRPVは論文と同じものであるべき()
        {
            var activityProduct = TestHelper.Activity(income: 0m, directCost: 20m, failRate: 0.5m);
            var activitySales = TestHelper.Activity(income: 100m, directCost: 0m, failRate: 0.5m);
            activityProduct.Precede(activitySales);
            var project = new Project();
            project.AddActivities(activityProduct, activitySales);

            project.RPVstart().Value.Is(5m);
            project.RPVfinish().Value.Is(80m);
        }

        [TestMethod]
        public void 段階的プロジェクト３のRPVは論文と同じものであるべき()
        {
            var activityProduct = TestHelper.Activity(income: 100m, directCost: 20m, failRate: 0.1m);
            var activitySales = TestHelper.Activity(income: 0m, directCost: 0m, failRate: 0.5m);
            activityProduct.Succeed(activitySales);
            var project = new Project();
            project.AddActivities(activityProduct, activitySales);

            project.RPVstart().Value.Is(35m);
            project.RPVfinish().Value.Is(80m);
        }

        [TestMethod]
        public void 三段階プロジェクトのRPVは期待されたものであるべき()
        {
            var activityDesign = TestHelper.Activity(income: 100m, directCost: 20m, failRate: 0.5m);
            var activityProduct = TestHelper.Activity(income: 200m, directCost: 80m, failRate: 0.5m);
            var activitySales = TestHelper.Activity(income: 1000m, directCost: 200m, failRate: 0.1m);
            activityProduct.Succeed(activityDesign);
            activitySales.Succeed(activityProduct);
            var project = new Project();
            project.AddActivities(activityDesign, activityProduct, activitySales);

            project.RPVstart().Value.Is(215m);
            project.RPVfinish().Value.Is(1000m);
        }


        [TestMethod]
        public void 三段階プロジェクトの途中RPVは貢献価値が反映されたものであるべき()
        {
            var activityDesign = TestHelper.Activity(income: 100m, directCost: 20m, failRate: 0.5m);
            var activityProduct = TestHelper.Activity(income: 200m, directCost: 80m, failRate: 0.5m);
            var activitySales = TestHelper.Activity(income: 1000m, directCost: 200m, failRate: 0.1m);
            activityProduct.Succeed(activityDesign);
            activitySales.Succeed(activityProduct);
            var project = new Project();
            project.AddActivities(activityDesign, activityProduct, activitySales);

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
            var basicDesign = TestHelper.Activity(fixTime: 20);
            var hardProcurement = TestHelper.Activity(fixTime: 35);
            var detailDesign = TestHelper.Activity(fixTime: 10);
            var hardConfiguration = TestHelper.Activity(fixTime: 5);
            var develop = TestHelper.Activity(fixTime: 20);
            var testing = TestHelper.Activity(fixTime: 15);

            basicDesign.Branch(new[] { hardProcurement, detailDesign });
            hardProcurement.Precede(hardConfiguration);
            detailDesign.Precede(develop);
            testing.Merge(new[] { hardConfiguration, develop });

            var project = new Project();
            project.AddActivities(basicDesign, hardProcurement, hardConfiguration, detailDesign, develop, testing);

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
            var basicDesign = TestHelper.Activity(fixTime: 20, directCost: 10);
            var hardProcurement = TestHelper.Activity(fixTime: 35, directCost: 100);
            var detailDesign = TestHelper.Activity(fixTime: 10, directCost: 10);
            var hardConfiguration = TestHelper.Activity(fixTime: 5, directCost: 50);
            var develop = TestHelper.Activity(fixTime: 20, directCost: 100);
            var testing = TestHelper.Activity(fixTime: 15, directCost: 50);

            basicDesign.Branch(new[] { hardProcurement, detailDesign });
            hardProcurement.Precede(hardConfiguration);
            detailDesign.Precede(develop);
            testing.Merge(new[] { hardConfiguration, develop });

            var project = new Project();
            project.AddActivities(basicDesign, hardProcurement, hardConfiguration, detailDesign, develop, testing);

            basicDesign.IntrinsicCost(liquidatedDamages).Value.Is(110m);
            hardProcurement.IntrinsicCost(liquidatedDamages).Value.Is(150m);
            hardConfiguration.IntrinsicCost(liquidatedDamages).Value.Is(75m);
            detailDesign.IntrinsicCost(liquidatedDamages).Value.Is(10m);
            develop.IntrinsicCost(liquidatedDamages).Value.Is(100m);
            testing.IntrinsicCost(liquidatedDamages).Value.Is(125m);
        }
    }
}