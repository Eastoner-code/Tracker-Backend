using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NPoco;
using System.Linq;

namespace TrackerApi.Models
{
    public class TrackerContext : IdentityDbContext<IdentityAuthUser, IdentityAuthRole, int>
    {

        public TrackerContext()
        {
        }

        public TrackerContext(DbContextOptions<TrackerContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }


        public virtual DbSet<Activity> Activity { get; set; }
        public override DbSet<IdentityAuthUser> Users { get; set; }
        public virtual DbSet<UserPosition> UsersPosition { get; set; }
        public virtual DbSet<UserProject> UserProject { get; set; }
        public virtual DbSet<UserSkill> UserSkill { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Skill> Skill { get; set; }
        public virtual DbSet<UserRate> UserRates { get; set; }
        public virtual DbSet<Holiday> Holidays { get; set; }
        public virtual DbSet<Absence> Absences { get; set; }
        public virtual DbSet<UserYearRange> UserYearRanges { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Vacancy> Vacancies { get; set; }
        public virtual DbSet<Candidate> Candidates { get; set; }
        public virtual DbSet<InvoicePipeline> Invoices { get; set; }
        public virtual DbSet<InvoiceUserProject> InvoiceUserProject { get; set; }
        public virtual DbSet<PaymentDetails> PaymentDetails { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseNpgsql("Host=localhost;port=5432;Database=time_tracker_db;Username=postgres;Password=pass");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name);
                entity.Property(e => e.CreatedAtUtc)
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Description)
                    .HasMaxLength(255);

                entity.Property(e => e.Duration)
                    .HasColumnType("numeric(10,0)")
                    .HasDefaultValueSql("NULL::numeric");

                entity.Property(e => e.UpdatedAtUtc)
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.WorkedFromUtc)
                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.WorkedToUtc)
                    .HasColumnType("timestamp(6) without time zone");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_as_ac_user_id");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Activity)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("fk_as_ac_project_id");
            });


            modelBuilder.Entity<UserPosition>(entity =>
            {
                entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPosition)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_as_upo_user_id");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.UserPosition)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("fk_as_upo_position_id");
            });

            modelBuilder.Entity<UserProject>(entity =>
            {
                entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserProject)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_as_upr_user_id");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.UserProject)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("fk_as_upr_project_id");
            });

            modelBuilder.Entity<UserSkill>(entity =>
            {
                entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserSkill)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_as_sk_user_id");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.UserSkill)
                    .HasForeignKey(d => d.SkillId)
                    .HasConstraintName("fk_as_sk_skill_id");
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name);

                entity.Property(e => e.Name)
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name);

                entity.Property(e => e.CreatedAtUtc)

                    .HasColumnType("timestamp(6) without time zone");

                entity.Property(e => e.Meta)
                    .IsRequired()
                    .HasColumnType("jsonb");

                entity.Property(e => e.Name)
                    .HasMaxLength(255);

                entity.Property(e => e.MainCof)
                .HasColumnType("double precision");

                entity.Property(e => e.OverCof)
                .HasColumnType("double precision");

                entity.Property(e => e.WeekCof)
                .HasColumnType("double precision");

                entity.Property(e => e.IsArchive)
                .HasColumnType("boolean");

                entity.Property(e => e.CustomerUrl)
                .HasColumnType("varchar")
                .HasMaxLength(50);

                entity.Property(e => e.UpdatedAtUtc)
                    .HasColumnType("timestamp(6) without time zone");
            });

            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name);

                entity.Property(e => e.Name)
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<InvoiceUserProject>(entity =>
            {
                entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                
                entity.HasOne(invoiceUserProject => invoiceUserProject.User)
                    .WithMany(u => u.InvoiceUserProject)
                    .HasForeignKey(invoiceUserProject => invoiceUserProject.UserId);

                entity.HasOne(invoiceUserProject => invoiceUserProject.Project)
                .WithMany(inv => inv.InvoiceUserProject)
                .HasForeignKey(invoiceUserProject => invoiceUserProject.ProjectId);

                entity.HasOne(invoiceUserProject => invoiceUserProject.Invoice)
                .WithMany(inv => inv.InvoiceUserProject)
                .HasForeignKey(invoiceUserProject => invoiceUserProject.InvoiceId);
                    
            });

            modelBuilder.Entity<InvoicePipeline>(entity =>
            {
                entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name);

                entity.HasOne(inv => inv.User)
                .WithMany(u => u.InvoicePipeline)
                .HasForeignKey(inv => inv.UserId);
            });

            modelBuilder.Entity<PaymentDetails>(entity =>
            {
                entity.HasOne(p => p.Invoice)
                .WithMany(inv => inv.PaymentDetails)
                .HasForeignKey(p => p.InvoiceId);
            });

            modelBuilder.Entity<UserRate>(entity => { entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name); });
            modelBuilder.Entity<Holiday>(entity => { entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name); });
            modelBuilder.Entity<Absence>(entity => { entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name); });
            modelBuilder.Entity<UserYearRange>(entity => { entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name); });
            modelBuilder.Entity<Notification>(entity => { entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name); });
            modelBuilder.Entity<Vacancy>(entity => { entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name); });
            modelBuilder.Entity<Candidate>(entity => { entity.ToTable(entity.GetTheType().GenericTypeArguments.FirstOrDefault().Name); });

            base.OnModelCreating(modelBuilder);
        }

    }
}
