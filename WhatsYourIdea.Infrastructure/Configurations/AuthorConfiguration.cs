using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        private const string FK_USER_AUTHOR = "userProfileId";

        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("authors");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).IsRequired();

            AddUserProfileConfig(builder);
        }

        private static void AddUserProfileConfig(EntityTypeBuilder<Author> builder)
        {
            builder.Property<int>(FK_USER_AUTHOR);
            builder.HasOne(a => a.UserProfile)
                   .WithOne(u => u.Author)
                   .HasForeignKey<Author>(FK_USER_AUTHOR);

            builder.Navigation(x => x.UserProfile).AutoInclude();
        }
    }
}