using System;
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
            var expectedWorkerType = WorkerType.Human;

            var activity = new Activity();
            activity.Estimate(WorkLoad.Of(expectedValue, expectedTimeType, expectedWorkerType));
            activity.WorkLoad.Value.Is(expectedValue);
            activity.WorkLoad.TimeUnit.Is(expectedTimeType);
            activity.WorkLoad.WorkerUnit.Is(expectedWorkerType);
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
    }
}
