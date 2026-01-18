using BluntServe.Models;
using Microsoft.EntityFrameworkCore;

namespace BluntServe
{
    public class PgDbContext: DbContext
    {
        public PgDbContext(DbContextOptions<PgDbContext> options) : base(options)
        {
        }
        public PgDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 配置迁移策略 - 保护现有表结构
            ConfigureMigrationStrategy(modelBuilder);

            // 实体表配置
            ConfigureEntities(modelBuilder);
        }

        private void ConfigureMigrationStrategy(ModelBuilder modelBuilder)
        {
            // 设置全局迁移策略为仅迁移不删除
            modelBuilder.HasAnnotation("Relational:MigrationStrategy", "Migrate");

            // 为每个实体设置保护
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.AddAnnotation("Relational:Protected", true);
            }
        }
        public DbSet<User> User { get; set; }
        private void ConfigureEntities(ModelBuilder modelBuilder) {

            modelBuilder.Entity<User>().ToTable("t_users", "blunt");
            modelBuilder.Entity<User>().HasKey(e => e.UserId);
        }
    }
}
