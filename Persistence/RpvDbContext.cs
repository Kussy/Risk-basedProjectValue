﻿using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Kussy.Analysis.Project.Persistence
{
    /// <summary>リスク基準プロジェクト価値コンテキスト</summary>
    public class RpvDbContext : DbContext
    {
        #region プロパティ
        /// <summary>プロジェクト</summary>
        public DbSet<Project> Projects { get; set; }
        /// <summary>アクティビティ</summary>
        public DbSet<Activity> Activities { get; set; }
        /// <summary>スコープ</summary>
        public DbSet<Scope> Scopes { get; set; }
        /// <summary>ネットワーク</summary>
        public DbSet<Network> Networks { get; set; }
        /// <summary>リソース</summary>
        public DbSet<Resource> Resources { get; set; }
        /// <summary>アサイン</summary>
        public DbSet<Assign> Assigns { get; set; }
        #endregion

        /// <summary>コンストラクタ隠蔽</summary>
        private RpvDbContext() { }

        /// <summary>引数ありコンストラクタ</summary>
        /// <param name="options">DBコンテキストオプション</param>
        public RpvDbContext(DbContextOptions<RpvDbContext> options) : base(options) { }

        /// <summary>モデル作成</summary>
        /// <param name="modelBuilder">モデルビルダー</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Table Name
            modelBuilder.Entity<Project>().ToTable("projects");
            modelBuilder.Entity<Activity>().ToTable("activities");
            modelBuilder.Entity<Scope>().ToTable("scopes");
            modelBuilder.Entity<Network>().ToTable("networks");
            modelBuilder.Entity<Resource>().ToTable("resources");
            modelBuilder.Entity<Assign>().ToTable("assigns");
            #endregion
            #region Project Table
            modelBuilder.Entity<Project>().Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsRequired();
            modelBuilder.Entity<Project>().Property(c => c.Name)
                .HasColumnName("name")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();
            modelBuilder.Entity<Project>().Property(c => c.UnitOfCurrency)
                .HasColumnName("unit_of_currency")
                .HasColumnType("integer")
                .IsRequired()
                .HasDefaultValue(CurrencyType.JPY);
            modelBuilder.Entity<Project>().Property(c => c.UnitOfTime)
                .HasColumnName("unit_of_time")
                .HasColumnType("integer")
                .IsRequired()
                .HasDefaultValue(TimeType.Day);
            modelBuilder.Entity<Project>().Property(c => c.Term)
                .HasColumnName("term")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);
            modelBuilder.Entity<Project>().Property(c => c.Budjet)
                .HasColumnName("budjet")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);
            modelBuilder.Entity<Project>().Property(c => c.LiquidatedDamages)
                .HasColumnName("liquidated_damages")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);
            modelBuilder.Entity<Project>().HasKey(c => new { c.Id });
            modelBuilder.Entity<Project>().HasMany(c => c.Scopes);
            #endregion
            #region Activity Table
            modelBuilder.Entity<Activity>().Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsRequired();
            modelBuilder.Entity<Activity>().Property(c => c.Name)
                .HasColumnName("name")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();
            modelBuilder.Entity<Activity>().Property(c => c.State)
                .HasColumnName("state")
                .HasColumnType("integer")
                .IsRequired()
                .HasDefaultValue(StateType.Unknown);
            modelBuilder.Entity<Activity>().Property(c => c.Workload)
                .HasColumnName("workload")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);
            modelBuilder.Entity<Activity>().Property(c => c.FixedLeadTime)
                .HasColumnName("fixed_lead_time")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);
            modelBuilder.Entity<Activity>().Property(c => c.Income)
                .HasColumnName("income")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);
            modelBuilder.Entity<Activity>().Property(c => c.ExternalCost)
                .HasColumnName("external_cost")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(0m);
            modelBuilder.Entity<Activity>().Property(c => c.RateOfFailure)
                .HasColumnName("rate_of_failure")
                .HasColumnType("decimal(1,2)")
                .IsRequired()
                .HasDefaultValue(0m);
            modelBuilder.Entity<Activity>().HasKey(c => new { c.Id });
            modelBuilder.Entity<Activity>().HasOne(c => c.Scope).WithOne(c => c.Activity);
            modelBuilder.Entity<Activity>().HasMany(c => c.Assigns).WithOne(c => c.Activity);
            #endregion
            #region Scope Table
            modelBuilder.Entity<Scope>().Property(c => c.ProjectId)
                .HasColumnName("project_id")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsRequired();
            modelBuilder.Entity<Scope>().Property(c => c.ActivityId)
                .HasColumnName("activity_id")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsRequired();
            modelBuilder.Entity<Scope>().HasKey(c => new { c.ProjectId, c.ActivityId });
            modelBuilder.Entity<Scope>().HasOne(c => c.Project).WithMany(c => c.Scopes).HasForeignKey(c => new { c.ProjectId });
            modelBuilder.Entity<Scope>().HasOne(c => c.Activity).WithOne(c => c.Scope);
            #endregion
            #region Network Table
            modelBuilder.Entity<Network>().Property(c => c.AncestorId)
                .HasColumnName("ancestor_id")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsRequired();
            modelBuilder.Entity<Network>().Property(c => c.DescendantId)
                .HasColumnName("descendant_id")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsRequired();
            modelBuilder.Entity<Network>().Property(c => c.Depth)
                .HasColumnName("depth")
                .HasColumnType("integer")
                .IsRequired();
            modelBuilder.Entity<Network>().HasKey(c => new { c.AncestorId, c.DescendantId });
            modelBuilder.Entity<Network>().HasOne(c => c.Ancestor).WithMany().HasForeignKey(c => new { c.AncestorId });
            modelBuilder.Entity<Network>().HasOne(c => c.Descendant).WithMany().HasForeignKey(c => new { c.DescendantId });
            #endregion
            #region Resource Table
            modelBuilder.Entity<Resource>().Property(c => c.Id)
                .HasColumnName("id")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsRequired();
            modelBuilder.Entity<Resource>().Property(c => c.Name)
                .HasColumnName("name")
                .HasColumnType("nvarchar")
                .HasMaxLength(256)
                .IsRequired();
            modelBuilder.Entity<Resource>().Property(c => c.Type)
                .HasColumnName("type")
                .HasColumnType("integer")
                .IsRequired()
                .HasDefaultValue(ResourceType.Unknown);
            modelBuilder.Entity<Resource>().Property(c => c.Productivity)
                .HasColumnName("productivity")
                .HasColumnType("decimal(12,2)")
                .IsRequired()
                .HasDefaultValue(1m);
            modelBuilder.Entity<Resource>().HasKey(c => new { c.Id });
            modelBuilder.Entity<Resource>().HasIndex(c => new { c.Type });
            modelBuilder.Entity<Resource>().HasMany(c => c.Assigns).WithOne(c => c.Resource);
            #endregion
            #region Assign Table
            modelBuilder.Entity<Assign>().Property(c => c.ActivityId)
                .HasColumnName("activity_id")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsRequired();
            modelBuilder.Entity<Assign>().Property(c => c.ResourceId)
                .HasColumnName("resource_id")
                .HasColumnType("nvarchar")
                .HasMaxLength(32)
                .IsRequired();
            modelBuilder.Entity<Assign>().Property(c => c.Quantity)
                .HasColumnName("quantity")
                .HasColumnType("decimal(12,2)")
                .IsRequired();
            modelBuilder.Entity<Assign>().HasKey(c => new { c.ActivityId, c.ResourceId });
            modelBuilder.Entity<Assign>().HasOne(c => c.Activity).WithMany(c => c.Assigns).HasForeignKey(c => new { c.ActivityId });
            modelBuilder.Entity<Assign>().HasOne(c => c.Resource).WithMany(c => c.Assigns).HasForeignKey(c => c.ResourceId);
            #endregion
        }
    }
}