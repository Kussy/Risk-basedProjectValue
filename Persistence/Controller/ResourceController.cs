using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>資源操作</summary>
    public class ResourceController
    {
        /// <summary>DBコンテキスト</summary>
        public RpvDbContext Context { get; private set; }

        /// <summary>コンストラクタ隠蔽</summary>
        private ResourceController() { }

        /// <summary>DBコンテキストの依存性注入</summary>
        /// <param name="context">DBコンテキスト</param>
        public ResourceController(RpvDbContext context)
        {
            Context = context;
        }

        /// <summary>資源の作成</summary>
        /// <param name="code">コード</param>
        /// <param name="name">名称</param>
        /// <param name="type">資源種類</param>
        /// <param name="productivity">生産性</param>
        public void Create(string code, string name, ResourceType type = ResourceType.Unknown, decimal productivity = 1m)
        {
            var resource = new Resource
            {
                Code = code,
                Name = name,
                Type = type,
                Productivity = productivity,
            };
            Context.Resources.Add(resource);
            Context.SaveChanges();
        }

        /// <summary>資源の取得</summary>
        /// <param name="id">ID</param>
        /// <returns>資源</returns>
        public Resource Read(int id)
        {
            var resource = Context.Resources.Find(id);
            if (resource is null) return resource;
            Context.Entry(resource).Collection(r => r.Assigns).Load();
            foreach (var assign in resource.Assigns)
            {
                Context.Entry(assign).Reference(a => a.Activity).Load();
                Context.Entry(assign.Activity).Reference(a => a.Project).Load();
            }
            return resource;
        }

        /// <summary>資源の取得</summary>
        /// <param name="code">コード</param>
        /// <remarks>ユニークインデックスを張っているので一本引き可能</remarks>
        /// <returns>資源</returns>
        public Resource Read(string code)
        {
            return Context.Resources
                .Include(r => r.Assigns)
                .ThenInclude(a => a.Activity)
                .ThenInclude(a => a.Project)
                .Where(r => r.Code == code)
                .FirstOrDefault();
        }

        /// <summary>資源の更新</summary>
        /// <param name="resource">資源</param>
        /// <remarks>勝手にエンティティを作られてたらAssignsは信用できないので無視</remarks>
        public void Update(Resource resource)
        {
            var findResource = Context.Resources.Find(resource.Id);
            findResource.Code = resource.Code;
            findResource.Name = resource.Name;
            findResource.Type = resource.Type;
            findResource.Productivity = resource.Productivity;
            Context.SaveChanges();
        }

        /// <summary>資源の削除</summary>
        /// <param name="resouce">資源</param>
        public void Delete(Resource resouce)
        {
            var findResource = Context.Resources.Find(resouce.Id);
            Context.Resources.Remove(findResource);
            Context.SaveChanges();
        }
    }
}