using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>作業操作</summary>
    public class ActivityController
    {
        /// <summary>DBコンテキスト</summary>
        public RpvDbContext Context { get; private set; }

        /// <summary>コンストラクタ隠蔽</summary>
        private ActivityController() { }

        /// <summary>DBコンテキストの依存性注入</summary>
        /// <param name="context">DBコンテキスト</param>
        public ActivityController(RpvDbContext context)
        {
            Context = context;
        }

        /// <summary>作業の作成</summary>
        /// <param name="project">プロジェクト</param>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="state">進捗状況</param>
        /// <param name="income">収入</param>
        /// <param name="externalCost">外部支出</param>
        /// <param name="fixedLeadTime">固定所要時間</param>
        /// <param name="workload">作業量</param>
        /// <param name="rateOfFailure">失敗確率</param>
        public void Create(Project project, string id, string name, StateType state = StateType.Unknown, decimal income = 0m, decimal externalCost = 0m, decimal fixedLeadTime = 0m, decimal workload = 0m, decimal rateOfFailure = 0m)
        {
            var activity = new Activity
            {
                Id = id,
                Name = name,
                State = state,
                Income = income,
                ExternalCost = externalCost,
                FixedLeadTime = fixedLeadTime,
                Workload = workload,
                RateOfFailure = rateOfFailure,
            };
            var scope = new Scope
            {
                Project = project,
                Activity = activity,
            };
            var network = new Network
            {
                Ancestor = activity,
                Descendant = activity,
                Depth = 0,
            };
            Context.Activities.Add(activity);
            Context.Scopes.Add(scope);
            Context.Networks.Add(network);
            Context.SaveChanges();
        }

        /// <summary>作業の取得</summary>
        /// <param name="id">ID</param>
        /// <returns>作業</returns>
        public Activity Read(string id)
        {
            var activity = Context.Activities.Find(id);
            if (activity is null) return activity;
            Context.Entry(activity).Reference(a => a.Scope).Load();
            Context.Entry(activity.Scope).Reference(s => s.Project).Load();
            Context.Entry(activity).Collection(a => a.Assigns).Load();
            foreach (var assign in activity.Assigns)
            {
                Context.Entry(assign).Reference(a => a.Resource).Load();
            }
            return activity;
        }

        /// <summary>作業の更新</summary>
        /// <param name="activity">作業</param>
        /// <remarks>勝手にエンティティを作られてたらAssignsは信用できないので無視</remarks>
        public void Update(Activity activity)
        {
            var findActivity = Context.Activities.Find(activity.Id);
            findActivity.Name = activity.Name;
            findActivity.State = activity.State;
            findActivity.Income = activity.Income;
            findActivity.ExternalCost = activity.ExternalCost;
            findActivity.FixedLeadTime = activity.FixedLeadTime;
            findActivity.Workload = activity.Workload;
            findActivity.RateOfFailure = activity.RateOfFailure;
            Context.SaveChanges();
        }

        /// <summary>作業の削除</summary>
        /// <param name="activity">作業</param>
        public void Delete(Activity activity)
        {
            var findAcitivy = Context.Activities.Find(activity.Id);
            Context.Activities.Remove(findAcitivy);
            Context.SaveChanges();
        }
    }
}