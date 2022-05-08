using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder.ToTable("userprofiles");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).IsRequired();

            builder.Navigation(u => u.Author).AutoInclude();
            builder.Navigation(u => u.TrackedIdeas).AutoInclude();
            builder.Navigation(u => u.Comments).AutoInclude();
        }
    }
}