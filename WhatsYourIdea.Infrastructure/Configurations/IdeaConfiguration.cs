using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Configurations;

internal class IdeaConfiguration : IEntityTypeConfiguration<Idea>
{
    private const string FK_IDEA_AUTHOR = "fk_idea_author";
    private const string FK_IDEA_COMMENT = "fk_idea_comment";
    private const string IDEA_USER_TABLE_NAME = "idea_user";
    private const string IDEA_TAG_TABLE_NAME = "idea_tag";

    public void Configure(EntityTypeBuilder<Idea> builder)
    {
        builder
            .ToTable("idea");

        builder
            .HasKey(k => k.Id);

        builder
            .Property(p => p.Id)
            .IsRequired();

        AddAuthorConfig(builder);
        AddTagsConfig(builder);
        AddCommentsConfig(builder);
        AddTrackingUsersConfig(builder);
    }

    private static void AddTrackingUsersConfig(EntityTypeBuilder<Idea> builder)
    {
        builder.HasMany(i => i.TrackingUsers)
               .WithMany(u => u.TrackedIdeas)
               .UsingEntity(j => j.ToTable(IDEA_USER_TABLE_NAME));
    }

    private static void AddCommentsConfig(EntityTypeBuilder<Idea> builder)
    {
        builder.Property<Guid>(FK_IDEA_COMMENT);
        builder.HasMany(i => i.Comments)
               .WithOne(c => c.Idea)
               .HasForeignKey(FK_IDEA_COMMENT);
    }

    private static void AddTagsConfig(EntityTypeBuilder<Idea> builder)
    {
        builder.HasMany(i => i.Tags)
               .WithMany(t => t.Ideas)
               .UsingEntity(j => j.ToTable(IDEA_TAG_TABLE_NAME));
    }

    private static void AddAuthorConfig(EntityTypeBuilder<Idea> builder)
    {
        builder.Property<Guid>(FK_IDEA_AUTHOR);
        builder.HasOne(i => i.Author)
               .WithMany(a => a.Ideas)
               .HasForeignKey(FK_IDEA_AUTHOR);
    }
}