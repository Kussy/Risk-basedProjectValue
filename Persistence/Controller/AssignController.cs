using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>資源割当操作</summary>
    public class AssignController
    {
        /// <summary>DBコンテキスト</summary>
        public RpvDbContext Context { get; private set; }

        /// <summary>コンストラクタ隠蔽</summary>
        private AssignController() { }

        /// <summary>DBコンテキストの依存性注入</summary>
        /// <param name="context">DBコンテキスト</param>
        public AssignController(RpvDbContext context)
        {
            Context = context;
        }

        /// <summary>資源割当の作成</summary>
        /// <param name="activity">作業</param>
        /// <param name="resource">資源</param>
        /// <param name="quantity">割当量</param>
        public void Create(Activity activity, Resource resource, decimal quantity)
        {
            var assign = new Assign
            {
                Activity = activity,
                Resource = resource,
                Quantity = quantity,
            };
            Context.Assigns.Add(assign);
            Context.SaveChanges();
        }

        /// <summary>資源割当の取得</summary>
        /// <param name="activity">作業</param>
        /// <param name="resource">資源</param>
        /// <returns>資源割当</returns>
        public Assign Read(Activity activity, Resource resource)
        {
            var assign = Context.Assigns.Find(activity.Id, resource.Id);
            if (assign is null) return assign;
            Context.Entry(assign).Reference(a => a.Resource).Load();
            Context.Entry(assign).Reference(a => a.Activity).Load();
            Context.Entry(assign.Activity).Reference(a => a.Project).Load();
            return assign;
        }

        /// <summary>資源割当の更新</summary>
        /// <param name="assign">資源割当</param>
        public void Update(Assign assign)
        {
            var findAssign = Context.Assigns.Find(assign.ActivityId, assign.ResourceId);
            findAssign.Quantity = assign.Quantity;
            Context.SaveChanges();
        }

        /// <summary>資源割当の削除</summary>
        /// <param name="assign">資源割当</param>
        public void Delete(Assign assign)
        {
            var findAssign = Context.Assigns.Find(assign.ActivityId, assign.ResourceId);
            Context.Assigns.Remove(findAssign);
            Context.SaveChanges();
        }
    }
}