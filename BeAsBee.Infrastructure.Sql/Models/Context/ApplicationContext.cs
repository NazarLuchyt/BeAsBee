using Microsoft.EntityFrameworkCore;

namespace BeAsBee.Infrastructure.Sql.Models.Context {
    public class ApplicationContext : DbContext {
        public ApplicationContext ( DbContextOptions<ApplicationContext> options )
            : base( options ) {
        }

        public DbSet<UserChat> UserChat { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating ( ModelBuilder modelBuilder ) {
            base.OnModelCreating( modelBuilder );

            //modelBuilder.Entity<Contact>()
            //    .HasMany(c => c.AttachedFiles)
            //    .WithOne(e => e.Contact);

            //modelBuilder.Entity<Vacancy>()
            //    .HasMany(c => c.Contacts)
            //    .WithOne(a => a.Vacancy);


            //modelBuilder.Entity<CaseStudyImage>()
            //    .HasOne(b => b.CaseStudy)
            //    .WithMany(a => a.Images)
            //    .OnDelete(DeleteBehavior.SetNull);

            //#region CaseStudyExpertise many to many

            //modelBuilder.Entity<CaseStudyExpertise>()
            //    .HasKey(t => new { t.CaseStudyId, t.ExpertiseId });

            //modelBuilder.Entity<CaseStudyExpertise>()
            //    .HasOne(sc => sc.CaseStudy)
            //    .WithMany(s => s.CaseStudyExpertises)
            //    .HasForeignKey(sc => sc.CaseStudyId);

            //modelBuilder.Entity<CaseStudyExpertise>()
            //    .HasOne(sc => sc.Expertise)
            //    .WithMany(c => c.CaseStudyExpertises)
            //    .HasForeignKey(sc => sc.ExpertiseId);

            //#endregion


            modelBuilder.Entity<UserChat>().HasKey( e => new {e.UserId, e.ChatId} );

            modelBuilder.Entity<UserChat>()
                .HasOne(sc => sc.User)
                .WithMany(s => s.UserChats)
                .HasForeignKey(sc => sc.UserId);

            modelBuilder.Entity<UserChat>()
                .HasOne(sc => sc.Chat)
                .WithMany(c => c.UserChats)
                .HasForeignKey(sc => sc.ChatId);

            //modelBuilder.Entity<User>() // User 1 => N UserChats
            //    .HasMany( u => u.UserChats )
            //    .WithOne( uc => uc.User )
            //    .HasForeignKey( uc => uc.UserId );

            //modelBuilder.Entity<User>() // User 1 => N UserChats
            //    .ToTable( "Users" );

            //modelBuilder.Entity<User>() //User 1 =>
            //    .HasMany( u => u.Chats ) //=> N Chats each =>
            //    .WithOne( c => c.User ) //=> 1 User
            //    .HasForeignKey( c => c.UserId );

            modelBuilder.Entity<User>() //User 1 =>
                .HasMany( u => u.Messages ) //=> N Messages each =>
                .WithOne( m => m.User ) //=> 1 User
                .HasForeignKey( m => m.UserId );

            modelBuilder.Entity<User>() //User 1 =>
                .HasMany(u => u.Chats) //=> N Messages each =>
                .WithOne(m => m.User) //=> 1 User
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Chat>() //Chat 1 = > N Messages (Chat has many messages and one message has 1 chat)
                .HasMany( c => c.Messages )
                .WithOne( m => m.Chat )
                .HasForeignKey( m => m.ChatId )
                .OnDelete(DeleteBehavior.SetNull);


            // User 1 => N UserChats N => 1 Chat  // how the bridge connection works (users to chats)

        }
    }
}