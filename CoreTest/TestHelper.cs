using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kussy.Analysis.Project.Core
{
    public static class TestHelper
    {
        public static Activity Activity(
            decimal income = 0m,
            decimal directCost = 0m,
            decimal failRate = 0m,
            decimal reworkRate = 0m,
            decimal costoverRate = 0m,
            State state = State.ToDo,
            decimal fixTime = 0m,
            decimal workLoad = 0m,
            decimal resourceQuantity = 0m,
            decimal resourceProductivity = 0m
            )
        {
            var activity = new Activity();
            activity.Estimate(Income.Of(income));
            activity.Estimate(Cost.Of(directCost));
            activity.Estimate(Risk.Of(failRate, reworkRate, costoverRate));
            activity.Estimate(LeadTime.Of(fixTime));
            activity.Estimate(WorkLoad.Of(workLoad));
            if (resourceQuantity != 0m && resourceProductivity != 0m)
            {
                activity.Assign(new[] { Resource.Of(resourceQuantity, resourceProductivity) });
            }
            activity.Progress(state);
            return activity;
        }
    }
}