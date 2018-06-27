using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>ネットワーク操作</summary>
    public class NetworkController
    {
        /// <summary>DBコンテキスト</summary>
        public RpvDbContext Context { get; private set; }

        /// <summary>コンストラクタ隠蔽</summary>
        private NetworkController() { }

        /// <summary>DBコンテキストの依存性注入</summary>
        /// <param name="context">DBコンテキスト</param>
        public NetworkController(RpvDbContext context)
        {
            Context = context;
        }

        /// <summary>ネットワークの取得</summary>
        /// <param name="ancestor">先祖</param>
        /// <param name="descendant">子孫</param>
        /// <returns>ネットワーク</returns>
        public Network Read(Activity ancestor, Activity descendant)
        {
            var network = Context.Networks.Find(ancestor.Id, descendant.Id);
            if (network is null) return network;
            Context.Entry(network).Reference(a => a.Ancestor).Load();
            Context.Entry(network.Ancestor).Reference(a => a.Scope).Load();
            Context.Entry(network.Ancestor.Scope).Reference(s => s.Project).Load();
            Context.Entry(network.Ancestor).Collection(a => a.Assigns).Load();
            foreach (var assign in network.Ancestor.Assigns)
            {
                Context.Entry(assign).Reference(a => a.Resource).Load();
            }
            Context.Entry(network).Reference(a => a.Descendant).Load();
            Context.Entry(network.Descendant).Reference(a => a.Scope).Load();
            Context.Entry(network.Descendant.Scope).Reference(s => s.Project).Load();
            Context.Entry(network.Descendant).Collection(a => a.Assigns).Load();
            foreach (var assign in network.Descendant.Assigns)
            {
                Context.Entry(assign).Reference(a => a.Resource).Load();
            }

            return network;
        }

        /// <summary>先行アクティビティ群を取得する</summary>
        /// <param name="activity">アクティビティ</param>
        /// <returns>先行アクティビティ群</returns>
        public IEnumerable<Activity> Parents(Activity activity)
        {
            return Context.Networks
                .Where(n => n.DescendantId == activity.Id)
                .Where(n => n.Depth == 1)
                .Distinct()
                .Select(n => n.Ancestor)
                .ToList();
        }

        /// <summary>後続アクティビティ群を取得する</summary>
        /// <param name="activity">アクティビティ</param>
        /// <returns>後続アクティビティ群</returns>
        public IEnumerable<Activity> Children(Activity activity)
        {
            return Context.Networks
                .Where(n => n.AncestorId == activity.Id)
                .Where(n => n.Depth == 1)
                .Distinct()
                .Select(n => n.Descendant)
                .ToList();
        }

        /// <summary>アクティビティの先祖を取得する</summary>
        /// <param name="activity">アクティビティ</param>
        /// <returns>自身を含む先祖全て</returns>
        public IEnumerable<Network> Ancestors(Activity activity)
        {
            return Context.Networks.Where(n => n.Descendant == activity).ToList();
        }

        /// <summary>アクティビティの子孫を取得する</summary>
        /// <param name="activity">アクティビティ</param>
        /// <returns>自身を含む子孫全て</returns>
        public IEnumerable<Network> Descendants(Activity activity)
        {
            return Context.Networks.Where(n => n.Ancestor == activity).ToList();
        }

        /// <summary>親アクティビティと子アクティビティを接続する</summary>
        /// <param name="parent">親</param>
        /// <param name="child">子</param>
        public void Connect(Activity parent, Activity child)
        {
            if (parent.Scope.Project.Id != child.Scope.Project.Id) throw new Exception();
            Network ancestorsDescendants(Network a, Network d)
                => new Network { AncestorId = a.AncestorId, DescendantId = d.DescendantId, Depth = a.Depth + d.Depth + 1, };
            var networks = Descendants(child).SelectMany(d => Ancestors(parent).Select(a => ancestorsDescendants(a, d)));
            var unique = new List<Network>();

            foreach (var n in networks)
            {
                var target = Context.Networks.Find(n.AncestorId, n.DescendantId);
                if (target is null) unique.Add(n);
            }
            Context.Networks.AddRange(unique);
            Context.SaveChanges();
        }

        /// <summary>ネットワークから該当アクティビティとその子孫を切り離す</summary>
        /// <param name="activity">切断対象のアクティビティ</param>
        public void Disconnect(Activity activity)
        {
            var removeTargets = Context.Networks
                .Where(n => Ancestors(activity).Select(a => a.AncestorId).Contains(n.AncestorId))
                .Where(n => Descendants(activity).Select(d => d.DescendantId).Contains(n.DescendantId))
                .Where(n => n.AncestorId != activity.Id);
            Context.Networks.RemoveRange(removeTargets);
            Context.SaveChanges();
        }
    }
}