using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>プロジェクト操作</summary>
    public class ProjectController
    {
        /// <summary>DBコンテキスト</summary>
        public RpvDbContext Context { get; private set; }

        /// <summary>コンストラクタ隠蔽</summary>
        private ProjectController() { }

        /// <summary>DBコンテキストの依存性注入</summary>
        /// <param name="context">DBコンテキスト</param>
        public ProjectController(RpvDbContext context)
        {
            Context = context;
        }

        /// <summary>プロジェクトの作成</summary>
        /// <param name="code">コード</param>
        /// <param name="name">名称</param>
        /// <param name="unitOfCurrency">通貨単位</param>
        /// <param name="unitOfTime">時間単位</param>
        /// <param name="term">期間</param>
        /// <param name="budget">予算</param>
        /// <param name="liquidatedDamages">遅延損害金</param>
        public void Create(string code, string name, CurrencyType unitOfCurrency = CurrencyType.JPY, TimeType unitOfTime = TimeType.Day, decimal term = 0m, decimal budget = 0m, decimal liquidatedDamages = 0m)
        {
            var project = new Project
            {
                Code = code,
                Name = name,
                UnitOfTime = unitOfTime,
                UnitOfCurrency = unitOfCurrency,
                Term = term,
                Budjet = budget,
                LiquidatedDamages = liquidatedDamages,
            };
            Context.Projects.Add(project);
            Context.SaveChanges();
        }

        /// <summary>プロジェクトの取得</summary>
        /// <param name="id">ID</param>
        /// <returns>プロジェクト</returns>
        public Project Read(int id)
        {
            var project = Context.Projects.Find(id);
            if (project is null) return project;
            Context.Entry(project).Collection(p => p.Activities).Load();
            foreach (var activity in project.Activities)
            {
                Context.Entry(activity).Collection(a => a.Assigns).Load();
                foreach (var assign in activity.Assigns)
                {
                    Context.Entry(assign).Reference(a => a.Resource).Load();
                }
            }
            return project;
        }

        /// <summary>プロジェクトの取得</summary>
        /// <param name="code">コード</param>
        /// <remarks>ユニークインデックスを張っているので一本引き可能</remarks>
        /// <returns>プロジェクト</returns>
        public Project Read(string code)
        {
            return Context.Projects
                .Include(p => p.Activities)
                .ThenInclude(a => a.Assigns)
                .ThenInclude(a => a.Resource)
                .Where(p => p.Code == code)
                .FirstOrDefault();
        }

        /// <summary>プロジェクトの更新</summary>
        /// <param name="project">プロジェクト</param>
        /// <remarks>勝手にエンティティを作られてたらActivitiesは信用できないので無視</remarks>
        public void Update(Project project)
        {
            var findProject = Context.Projects.Find(project.Id);
            findProject.Code = project.Code;
            findProject.Name = project.Name;
            findProject.UnitOfTime = project.UnitOfTime;
            findProject.UnitOfCurrency = project.UnitOfCurrency;
            findProject.Term = project.Term;
            findProject.Budjet = project.Budjet;
            findProject.LiquidatedDamages = project.LiquidatedDamages;
            Context.SaveChanges();
        }

        /// <summary>プロジェクトの削除</summary>
        /// <param name="project">プロジェクト</param>
        public void Delete(Project project)
        {
            var findProject = Context.Projects.Find(project.Id);
            Context.Projects.Remove(findProject);
            Context.SaveChanges();
        }
    }
}