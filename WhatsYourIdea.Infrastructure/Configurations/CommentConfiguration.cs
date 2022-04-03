using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        private const string FK_COMMENT_IDEA = "fk_comment_idea";
        private const string FK_COMMENT_USER = "fk_comment_user";

        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("comment");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).IsRequired();
            AddUserConfig(builder);
            AddIdeaConfig(builder);
        }

        private static void AddIdeaConfig(EntityTypeBuilder<Comment> builder)
        {
            builder.Property<Guid>(FK_COMMENT_IDEA);
            builder.HasOne(c => c.Idea)
                   .WithMany(i => i.Comments)
                   .HasForeignKey(FK_COMMENT_IDEA);
        }

        private static void AddUserConfig(EntityTypeBuilder<Comment> builder)
        {
            builder.Property<Guid>(FK_COMMENT_USER);
            builder.HasOne(c => c.User)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(FK_COMMENT_USER);
        }
    }
}