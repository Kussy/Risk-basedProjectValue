using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>スコープ操作</summary>
    public class ScopeController
    {
        /// <summary>DBコンテキスト</summary>
        public RpvDbContext Context { get; private set; }

        /// <summary>コンストラクタ隠蔽</summary>
        private ScopeController() { }

        /// <summary>DBコンテキストの依存性注入</summary>
        /// <param name="context">DBコンテキスト</param>
        public ScopeController(RpvDbContext context)
        {
            Context = context;
        }

        /// <summary>スコープの取得</summary>
        /// <param name="project">プロジェクト</param>
        /// <param name="activity">アクティビティ</param>
        /// <returns>ネットワーク</returns>
        public Scope Read(Project project, Activity activity)
        {
            var scope = Context.Scopes.Find(project.Id, activity.Id);
            if (scope is null) return scope;
            Context.Entry(scope).Reference(s => s.Project).Load();
            Context.Entry(scope).Reference(s => s.Activity).Load();
            Context.Entry(scope.Activity).Collection(a => a.Assigns).Load();
            foreach (var assign in scope.Activity.Assigns)
            {
                Context.Entry(assign).Reference(a => a.Resource).Load();
            }
            return scope;
        }

        /// <summary>スコープのプロジェクトを別のプロジェクトに変更する</summary>
        /// <param name="scope">スコープ</param>
        /// <param name="project">プロジェクト</param>
        public Scope Change(Scope scope, Project project)
        {
            Contract.Requires(!(scope is null));
            Contract.Requires(!(project is null));
            if (scope.ProjectId == project.Id) return scope;
            var changedScope = new Scope()
            {
                Project = project,
                Activity = scope.Activity,
            };
            Context.Remove(scope);
            Context.Add(changedScope);
            Context.SaveChanges();
            return changedScope;
        }

        /// <summary>スコープのアクティビティを別のアクティビティに変更する</summary>
        /// <param name="scope">スコープ</param>
        /// <param name="activity">アクティビティ</param>
        public Scope Change(Scope scope, Activity activity)
        {
            Contract.Requires(!(scope is null));
            Contract.Requires(!(activity is null));
            if (scope.ActivityId == activity.Id) return scope;
            var changedScope = new Scope()
            {
                Project = scope.Project,
                Activity = activity,
            };
            Context.Remove(scope);
            Context.Add(changedScope);
            Context.SaveChanges();
            return changedScope;
        }
    }
}