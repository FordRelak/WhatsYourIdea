using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Application.Configurations;

internal class IdeaConfiguration : IEntityTypeConfiguration<Idea>
{
    private const string FK_IDEA_AUTHOR = "authorId";
    private const string IDEA_USER_TABLE_NAME = "idea_user";
    private const string IDEA_TAG_TABLE_NAME = "idea_tag";

    public void Configure(EntityTypeBuilder<Idea> builder)
    {
        builder
            .ToTable("ideas");

        builder
            .HasKey(k => k.Id);

        builder
            .Property(p => p.Id)
            .IsRequired();

        builder.Property(p => p.Hash).IsRequired(false);
        builder.HasIndex(p => p.Hash).IsUnique();

        AddAuthorConfig(builder);
        AddTagsConfig(builder);
        AddTrackingUsersConfig(builder);
    }

    private static void AddTrackingUsersConfig(EntityTypeBuilder<Idea> builder)
    {
        builder.HasMany(i => i.TrackingUsers)
               .WithMany(u => u.TrackedIdeas)
               .UsingEntity(j => j.ToTable(IDEA_USER_TABLE_NAME));
    }

    private static void AddTagsConfig(EntityTypeBuilder<Idea> builder)
    {
        builder.HasMany(i => i.Tags)
               .WithMany(t => t.Ideas)
               .UsingEntity(j => j.ToTable(IDEA_TAG_TABLE_NAME));
    }

    private static void AddAuthorConfig(EntityTypeBuilder<Idea> builder)
    {
        builder.Property<int>(FK_IDEA_AUTHOR);
        builder.HasOne(i => i.Author)
               .WithMany(a => a.Ideas)
               .HasForeignKey(FK_IDEA_AUTHOR);

        //builder.Navigation(x => x.Author).AutoInclude();
    }
}