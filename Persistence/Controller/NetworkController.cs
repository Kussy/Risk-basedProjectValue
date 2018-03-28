using Microsoft.EntityFrameworkCore;
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

        /// <summary>ネットワークの作成</summary>
        /// <param name="ancestor">先祖</param>
        /// <param name="descendant">子孫</param>
        /// <param name="depth">深さ</param>
        public void Create(Activity ancestor, Activity descendant, int depth)
        {
            var network = new Network
            {
                Ancestor = ancestor,
                Descendant = descendant,
                Depth = depth,
            };
            Context.Networks.Add(network);
            Context.SaveChanges();
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
            Context.Entry(network.Ancestor).Reference(a => a.Project).Load();
            Context.Entry(network.Ancestor).Collection(a => a.Assigns).Load();
            foreach (var assign in network.Ancestor.Assigns)
            {
                Context.Entry(assign).Reference(a => a.Resource).Load();
            }
            Context.Entry(network).Reference(a => a.Descendant).Load();
            Context.Entry(network.Descendant).Reference(a => a.Project).Load();
            Context.Entry(network.Descendant).Collection(a => a.Assigns).Load();
            foreach (var assign in network.Descendant.Assigns)
            {
                Context.Entry(assign).Reference(a => a.Resource).Load();
            }

            return network;
        }

        /// <summary>ネットワークの更新</summary>
        /// <param name="network">ネットワーク</param>
        public void Update(Network network)
        {
            var findNetwork = Context.Networks.Find(network.AncestorId, network.DescendantId);
            findNetwork.Depth = network.Depth;
            Context.SaveChanges();
        }

        /// <summary>ネットワークの削除の削除</summary>
        /// <param name="network">ネットワーク</param>
        public void Delete(Network network)
        {
            var findNetwork = Context.Networks.Find(network.AncestorId, network.DescendantId);
            Context.Networks.Remove(findNetwork);
            Context.SaveChanges();
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
            Network ancestorsDescendants(Network a, Network d)
                => new Network { Ancestor = a.Ancestor, Descendant = d.Descendant, Depth = a.Depth + d.Depth + 1, };
            var networks = Descendants(child).SelectMany(d => Ancestors(parent).Select(a => ancestorsDescendants(a, d)));
            Context.Networks.AddRange(networks);
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