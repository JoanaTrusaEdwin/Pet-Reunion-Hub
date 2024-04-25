

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

        public virtual DbSet<IdentityUser> Users { get; set; }

        public virtual DbSet<GENERALLOCATION> GENERALLOCATION { get; set; }

        public DbSet<Tribute> Tribute { get; set; }

        public DbSet<Post> Post { get; set; }

        public DbSet<Media> Media { get; set; }

        public DbSet<RESOURCE> RESOURCE { get; set; }

        public DbSet<CONTAINER> CONTAINER { get; set; }

        //public DbSet<TESTBIT> TESTBIT { get; set; }
        public DbSet<Comment> Comment { get; set; }
        //public DbSet<Heart> Heart { get; set; }
        //public DbSet<Post> Post { get; set; }
        //public DbSet<Media> Media { get; set; }

        public DbSet<TributeNotification> TributeNotification { get; set; }

        public DbSet<NEWNOTIFICATION> NEWNOTIFICATION { get; set; }

        public DbSet<Newtest> NEWTEST { get; set; }

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
                    .IsRequired()
                    .HasColumnName("PetName");

                entity.Property(e => e.PetType)
                  .HasColumnName("PetType");

                entity.Property(e => e.PetBreed)
                    .HasColumnName("PetBreed");

                entity.Property(e => e.PetGender)
                    .IsRequired()
                    .HasColumnName("PetGender");

                entity.Property(e => e.DateOfBirth)
                    //.HasColumnType("date")
                    .HasColumnName("DateOfBirth");

                entity.Property(e => e.PetMicrochipID)
                    .HasColumnName("PetMicrochipID");

                entity.Property(e => e.LastSeenLocation)
                    .IsRequired()
                    .HasColumnName("LastSeenLocation");

                entity.Property(e => e.LastSeenTime)
                    .IsRequired()
                    .HasColumnName("LastSeenTime");

                //entity.Property(e => e.MainPhoto)
                //    .IsRequired()
                //    .HasColumnName("MainPhoto");

                entity.Property(e => e.ContactInformation)
                    .IsRequired()
                    .HasColumnName("ContactInformation");

                entity.Property(e => e.AdditionalDescription)              
                   .HasColumnName("AdditionalDescription");

                entity.Property(e => e.Age)               
                   .HasColumnName("Age");

                entity.Property(e => e.GenLoc)   
                  .HasColumnName("GenLoc");

                entity.Property(e => e.CreatedAt);

                //entity.Property(e => e.UserId)
                //    .IsRequired();

                // Define the relationship with AspNetUsers
                entity.HasOne(cr => cr.User)
                    .WithMany()
                    .HasForeignKey(cr => cr.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_CreateReport_AspNetUsers");

            });

            modelBuilder.Entity<ReportPhoto>(entity =>
            {
                entity.ToTable("ReportPhoto");

                entity.HasKey(e => e.PhotoId);

                entity.Property(e => e.ReportId)
                      .IsRequired();

                entity.Property(e => e.PhotoUrl)
                      .IsRequired();

                // Define the relationship with CreateReport
                entity.HasOne(rp => rp.CreateReport)
                      .WithMany(cr => cr.ReportPhotos)
                      .HasForeignKey(rp => rp.ReportId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ReportPhoto_CreateReport");
            });

            modelBuilder.Entity<GENERALLOCATION>(entity =>
            {
                entity.ToTable("GENERALLOCATION");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.LOCATIONVALUE)
                     .IsRequired();

                entity.HasOne(cr => cr.User)
                 .WithMany()
                 .HasForeignKey(cr => cr.UserId)
                 .OnDelete(DeleteBehavior.Restrict)
                 .HasConstraintName("FK_GENERALLOCATION_AspNetUsers");


            });

            modelBuilder.Entity<Tribute>(entity =>
            {
                entity.ToTable("Tribute");
                entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id"); 
                /*entity.HasKey(e => e.Id);*/ // Primary key
                entity.Property(e => e.PetName);
                entity.Property(e => e.PetType);
                entity.Property(e => e.PetBreed);
                //entity.Property(e => e.PetSex).HasMaxLength(10);
                entity.Property(e => e.PetSex);
                entity.Property(e => e.DateOfBirth);
                entity.Property(e => e.DateOfAdoption);
                entity.Property(e => e.DateOfDeparture);
                entity.Property(e => e.TributeText).HasMaxLength(int.MaxValue); // Max length
                entity.Property(e => e.TributePhoto);
                //entity.Property(e => e.IsPublic).IsRequired().HasColumnType("BIT");
                entity.Property(e => e.Visibility);
                
                entity.Property(e => e.CreatedAt);

                entity.Property(e => e.UserId).IsRequired().HasMaxLength(450);

                entity.HasOne(cr => cr.User)
                .WithMany()
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Tribute_AspNetUsers");


            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");
                entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id"); entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");
                //entity.Property(e => e.Tribute);
                entity.Property(e => e.Content);
                entity.Property(e => e.IsPublic);
                entity.Property(e => e.CreatedAt);

                entity.Property(e => e.ContainerId).HasColumnName("ContainerId"); 

                entity.HasOne(p => p.CONTAINER)
                    .WithMany()
                    .HasForeignKey(p => p.ContainerId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Post_CONTAINER");

                //entity.Property(e => e.TributeId).HasColumnName("TributeId");

                //entity.HasOne(p => p.Tribute)
                //    .WithMany()
                //    .HasForeignKey(p => p.TributeId)
                //    .OnDelete(DeleteBehavior.Restrict)
                //    .HasConstraintName("FK_Post_Tribute");

                entity.Property(e => e.UserId).IsRequired().HasMaxLength(450);

                entity.HasOne(cr => cr.User)
                .WithMany()
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Tribute_AspNetUsers");


            });

            modelBuilder.Entity<RESOURCE>(entity =>
            {
                entity.ToTable("RESOURCE");
                entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id"); entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                entity.Property(e => e.TITLE);
                entity.Property(e => e.DESCRIPTION);
                entity.Property(e => e.LINK);
                entity.Property(e => e.FILEURL);
                entity.Property(e => e.TYPE);
                entity.Property(e => e.FORMAT);
                entity.Property(e => e.CreatedAt);


                entity.Property(e => e.UserId).IsRequired().HasMaxLength(450);

                entity.HasOne(cr => cr.User)
                .WithMany()
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Tribute_AspNetUsers");


            });

            modelBuilder.Entity<Media>(entity =>
            {
                entity.ToTable("Media");

                entity.Property(e => e.Id)
                       .ValueGeneratedOnAdd()
                       .HasColumnName("Id"); entity.Property(e => e.Id)
                       .ValueGeneratedOnAdd()
                       .HasColumnName("Id");

                entity.Property(e => e.PostId);

                entity.Property(e => e.MediaUrl);
                entity.Property(e => e.MediaType);

                // Define the relationship with CreateReport
                entity.HasOne(rp => rp.Post)
                      .WithMany(cr => cr.Media)
                      .HasForeignKey(rp => rp.PostId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_Media_Post");
            });

            modelBuilder.Entity<CONTAINER>(entity =>
            {
                entity.ToTable("CONTAINER");
                entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id"); entity.Property(e => e.Id)
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id");

                entity.Property(e => e.Name);
                entity.Property(e => e.Description);

                entity.Property(e => e.UserId).IsRequired().HasMaxLength(450);

                entity.HasOne(cr => cr.User)
                .WithMany()
                .HasForeignKey(cr => cr.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CONTAINER_AspNetUsers");


            });

            //modelBuilder.Entity<TESTBIT>(entity =>
            //{
            //    entity.ToTable("TESTBIT");
            //    entity.Property(e => e.Id)
            //            .ValueGeneratedOnAdd()
            //            .HasColumnName("Id"); entity.Property(e => e.Id)
            //            .ValueGeneratedOnAdd()
            //            .HasColumnName("Id");

            //    entity.Property(e => e.Cute);
            //    entity.Property(e => e.UserId).IsRequired().HasMaxLength(450);

            //    entity.HasOne(cr => cr.User)
            //    .WithMany()
            //    .HasForeignKey(cr => cr.UserId)
            //    .OnDelete(DeleteBehavior.Restrict)
            //    .HasConstraintName("FK_TESTBIT_AspNetUsers");


            //});
            //modelBuilder.Entity<Comment>(entity =>
            //{
            //    entity.ToTable("Comment"); // Table name
            //    entity.HasKey(e => e.Id); // Primary key

            //    // Configure properties
            //    entity.Property(e => e.Id).HasColumnName("Id");
            //    entity.Property(e => e.Content).HasColumnName("Content").HasColumnType("NVARCHAR(MAX)");
            //    entity.Property(e => e.UserId).HasColumnName("UserId").IsRequired().HasColumnType("NVARCHAR(450)");
            //    entity.Property(e => e.PostId).HasColumnName("PostId");
            //    entity.Property(e => e.TributeId).HasColumnName("TributeId");


            //    entity.HasOne(e => e.Post)
            //          .WithMany()
            //          .HasForeignKey(e => e.PostId)
            //          .OnDelete(DeleteBehavior.Restrict);

            //    entity.HasOne(e => e.Tribute)
            //          .WithMany()
            //          .HasForeignKey(e => e.TributeId)
            //          .OnDelete(DeleteBehavior.Restrict);

            //    entity.HasOne(cr => cr.User)
            //   .WithMany()
            //   .HasForeignKey(cr => cr.UserId)
            //   .OnDelete(DeleteBehavior.Restrict)
            //   .HasConstraintName("FK_Comment_AspNetUsers");

            //});
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");
                entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");entity.Property(e => e.Id);

                entity.Property(e => e.Content).HasMaxLength(500);

                entity.Property(e => e.CreatedAt);

                entity.Property(e => e.TributeId)
                    .IsRequired();

                entity.HasOne(e => e.Tribute)
                    .WithMany(e => e.Comments)
                    .HasForeignKey(c => c.TributeId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(cr => cr.User)
            .WithMany()
            .HasForeignKey(cr => cr.UserId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Comment_AspNetUsers");
            });

            modelBuilder.Entity<TributeNotification>(entity =>
            {
                entity.ToTable("TributeNotification");

               
                entity.Property(e => e.Id)
              .ValueGeneratedOnAdd()
              .HasColumnName("Id"); entity.Property(e => e.Id);

                entity.Property(e => e.UserId).IsRequired();

                entity.Property(e => e.TributeId).IsRequired();

                entity.Property(e => e.CommentId).IsRequired();

                entity.Property(e => e.IsRead).IsRequired();

                entity.Property(e => e.CreatedAt);

                entity.HasOne(cr => cr.User)
                    .WithMany()
                    .HasForeignKey(cr => cr.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TributeNotification_AspNetUsers");

                entity.HasOne(cr => cr.Tribute)
                    .WithMany()
                    .HasForeignKey(cr => cr.TributeId)
                    .OnDelete(DeleteBehavior.Cascade);


                entity.HasOne(cr => cr.Comment)
                    .WithMany()
                    .HasForeignKey(cr => cr.CommentId)
                    .OnDelete(DeleteBehavior.Cascade);
                    
            });

            modelBuilder.Entity<NEWNOTIFICATION>(entity =>
            {
                entity.ToTable("NEWNOTIFICATION");


                entity.Property(e => e.Id)
              .ValueGeneratedOnAdd()
              .HasColumnName("Id"); entity.Property(e => e.Id);

                entity.Property(e => e.UserId).IsRequired();

                entity.Property(e => e.IsRead).IsRequired();

                entity.Property(e => e.Content);

                entity.Property(e => e.CreatedAt);

                entity.HasOne(cr => cr.User)
                    .WithMany()
                    .HasForeignKey(cr => cr.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_NEWNOTIFICATION_AspNetUsers");

            });


            //modelBuilder.Entity<Heart>(entity =>
            //{
            //    entity.ToTable("Heart"); // Table name
            //    entity.HasKey(e => e.Id); // Primary key

            //    // Configure properties
            //    entity.Property(e => e.Id).HasColumnName("Id");
            //    entity.Property(e => e.UserId).HasColumnName("UserId").IsRequired().HasColumnType("NVARCHAR(450)");
            //    entity.Property(e => e.PostId).HasColumnName("PostId");
            //    entity.Property(e => e.TributeId).HasColumnName("TributeId");

            //    entity.HasOne(e => e.Post)
            //          .WithMany()
            //          .HasForeignKey(e => e.PostId)
            //          .OnDelete(DeleteBehavior.Restrict);

            //    entity.HasOne(e => e.Tribute)
            //          .WithMany()
            //          .HasForeignKey(e => e.TributeId)
            //          .OnDelete(DeleteBehavior.Restrict);

            //    entity.HasOne(cr => cr.User)
            //  .WithMany()
            //  .HasForeignKey(cr => cr.UserId)
            //  .OnDelete(DeleteBehavior.Restrict)
            //  .HasConstraintName("FK_Heart_AspNetUsers");
            //});

            //modelBuilder.Entity<Media>(entity =>
            //{
            //    entity.ToTable("Media"); // Table name
            //    entity.HasKey(e => e.Id); // Primary key

            //    // Configure properties
            //    entity.Property(e => e.Id).HasColumnName("Id");
            //    entity.Property(e => e.MediaUrl).HasColumnName("MediaUrl").IsRequired().HasColumnType("NVARCHAR(500)");
            //    entity.Property(e => e.MediaType).HasColumnName("MediaType").IsRequired().HasColumnType("NVARCHAR(20)");
            //    entity.Property(e => e.PostId).HasColumnName("PostId");

            //    // Configure foreign key
            //    entity.HasOne(e => e.Post)
            //          .WithMany()
            //          .HasForeignKey(e => e.PostId)
            //          .OnDelete(DeleteBehavior.Restrict);
            //});


            modelBuilder.Entity<Newtest>(entity =>
            {
                entity.Property(e => e.NewTest)
                    .IsRequired()
                    .HasMaxLength(255);

                // Define the foreign key relationship with the User entity
                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Restrict) // Choose the appropriate delete behavior
                    .HasConstraintName("FK_NEWTEST_AspNetUsers"); // Set the constraint name

                // Define any additional configurations for the NEWTEST entity here
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

        public DbSet<PRHDATALIB.Models.TESTBIT>? TESTBIT_1 { get; set; }

        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
