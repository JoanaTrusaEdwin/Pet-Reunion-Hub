////using Microsoft.AspNetCore.Identity;
////using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
////using Microsoft.EntityFrameworkCore;

////namespace PRHDATALIB.Models;

////public class DatabaseContext : IdentityDbContext<IdentityUser>
////{
////    public DatabaseContext(DbContextOptions<DatabaseContext> options)
////        : base(options)
////    {
////    }

////    protected override void OnModelCreating(ModelBuilder builder)
////    {
////        base.OnModelCreating(builder);
////        // Customize the ASP.NET Identity model and override the defaults if needed.
////        // For example, you can rename the ASP.NET Identity table names and more.
////        // Add your customizations after calling base.OnModelCreating(builder);
////    }
////}


#nullable disable
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



namespace PRHDATALIB.Models
{
    //public partial class DatabaseContext : DbContext
    public partial class DatabaseContext : IdentityDbContext<IdentityUser> // Modify the inheritance
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        //public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public virtual DbSet<CreateReport> CreateReport { get; set; }

        public virtual DbSet<ReportPhoto> ReportPhoto { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=LAPTOP-4OJRF6T6\\SQLEXPRESS;Database=PetReunionHub;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CreateReport>(entity =>
            {
                entity.ToTable("CreateReport");

                //entity.Property(e => e.Id)
                //    .ValueGeneratedNever()
                //    .HasColumnName("Id");

                entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");


                entity.Property(e => e.PetName)
                    .HasMaxLength(255)
                    .IsRequired()
                    .HasColumnName("PetName");

                entity.Property(e => e.PetBreed)
                    .HasMaxLength(255)
                    .HasColumnName("PetBreed");

                entity.Property(e => e.PetGender)
                    .HasMaxLength(10)
                    .IsRequired()
                    .HasColumnName("PetGender");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("DateOfBirth");

                entity.Property(e => e.PetMicrochipID)
                    .HasMaxLength(50)
                    .HasColumnName("PetMicrochipID");

                entity.Property(e => e.LastSeenLocation)
                    .HasMaxLength(255)
                    .IsRequired()
                    .HasColumnName("LastSeenLocation");

                entity.Property(e => e.LastSeenTime)
                    .IsRequired()
                    .HasColumnName("LastSeenTime");

                entity.Property(e => e.MainPhoto)
                    .HasMaxLength(255)
                    .IsRequired()
                    .HasColumnName("MainPhoto");

                entity.Property(e => e.ContactInformation)
                    .HasMaxLength(255)
                    .IsRequired()
                    .HasColumnName("ContactInformation");
            });

            modelBuilder.Entity<ReportPhoto>(entity =>
            {
                entity.ToTable("ReportPhoto");

                entity.HasKey(e => e.PhotoId);

                entity.Property(e => e.ReportId)
                      .IsRequired();

                entity.Property(e => e.PhotoUrl)
                      .HasMaxLength(255)
                      .IsRequired();

                // Define the relationship with CreateReport
                entity.HasOne(rp => rp.CreateReport)
                      .WithMany(cr => cr.ReportPhotos)
                      .HasForeignKey(rp => rp.ReportId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ReportPhoto_CreateReport");
            });

            //modelBuilder.Entity<IdentityUser>(entity =>
            //{
            //    entity.ToTable("AspNetUsers");

            //    entity.Property(e => e.Id)
            //        .HasMaxLength(450)
            //        .IsRequired()
            //        .HasColumnName("Id");

            //    entity.Property(e => e.UserName)
            //        .HasMaxLength(256)
            //        .IsRequired()
            //        .HasColumnName("UserName");

            //    entity.Property(e => e.NormalizedUserName)
            //        .HasMaxLength(256)
            //        .IsRequired()
            //        .HasColumnName("NormalizedUserName");

            //    entity.Property(e => e.Email)
            //        .HasMaxLength(256)
            //        .HasColumnName("Email");

            //    entity.Property(e => e.NormalizedEmail)
            //        .HasMaxLength(256)
            //        .HasColumnName("NormalizedEmail");

            //    entity.Property(e => e.EmailConfirmed)
            //        .HasColumnName("EmailConfirmed");

            //    entity.Property(e => e.PasswordHash)
            //        .HasColumnName("PasswordHash");

            //    entity.Property(e => e.SecurityStamp)
            //        .HasColumnName("SecurityStamp");

            //    entity.Property(e => e.ConcurrencyStamp)
            //        .HasColumnName("ConcurrencyStamp");

            //    entity.Property(e => e.PhoneNumber)
            //        .HasColumnName("PhoneNumber");

            //    entity.Property(e => e.PhoneNumberConfirmed)
            //        .HasColumnName("PhoneNumberConfirmed");

            //    entity.Property(e => e.TwoFactorEnabled)
            //        .HasColumnName("TwoFactorEnabled");

            //    entity.Property(e => e.LockoutEnd)
            //        .HasColumnName("LockoutEnd");

            //    entity.Property(e => e.LockoutEnabled)
            //        .HasColumnName("LockoutEnabled");

            //    entity.Property(e => e.AccessFailedCount)
            //        .HasColumnName("AccessFailedCount");

            //    entity.HasIndex(e => e.NormalizedEmail)
            //        .HasName("EmailIndex");

            //    entity.HasIndex(e => e.NormalizedUserName)
            //        .HasName("UserNameIndex")
            //        .IsUnique();
            //});


            base.OnModelCreating(modelBuilder);
            //OnModelCreatingPartial(modelBuilder);

            //modelBuilder.Entity<ApplicationUser>().ToTable("AspNetUsers");
            //modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");
            //modelBuilder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
            //modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
            //modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
            //modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
            //modelBuilder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");
        }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
