using BluntServe.Models;
using Microsoft.EntityFrameworkCore;

namespace BluntServe.Data
{
    public class PgDbContext : DbContext
    {
        public PgDbContext(DbContextOptions<PgDbContext> options) : base(options)
        {
        }
        public PgDbContext()
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<UserSocial> UserSocials { get; set; }
        public DbSet<UserVerifyCode> UserVerifyCodes { get; set; }
        public DbSet<SysLog> SysLogs { get; set; }
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
        private void ConfigureEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("t_users", "blunt");
                entity.HasKey(e => e.UserId);
            });


            modelBuilder.Entity<UserRefreshToken>(entity =>
            {
                entity.ToTable("t_user_refresh_tokens", "blunt");
                entity.HasKey(e => e.Id);
                // 级联删除
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserSocial>(entity =>
            {
                entity.ToTable("t_user_social", "blunt");
                entity.HasKey(e => e.Id);
                // 级联删除
                entity.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserVerifyCode>(entity =>
            {
                entity.ToTable("t_user_verify_codes", "blunt");
                entity.HasKey(e => e.Id);
            });


            modelBuilder.Entity<SysLog>(entity =>
            {
                entity.ToTable("t_sys_log", "blunt");
                entity.HasKey(e => e.Id);
            });
        }
    }
}
