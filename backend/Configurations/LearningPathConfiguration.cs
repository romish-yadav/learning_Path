using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LearnPath.API.Configurations;

public class LearningPathConfiguration : IEntityTypeConfiguration<Entities.LearningPath>
{
    public void Configure(EntityTypeBuilder<Entities.LearningPath> builder)
    {
        builder.HasKey(lp => lp.Id);
        builder.Property(lp => lp.Title).HasMaxLength(200).IsRequired();
        builder.Property(lp => lp.Description).HasMaxLength(2000);
        builder.Property(lp => lp.ThumbnailUrl).HasMaxLength(500);
        builder.Property(lp => lp.Tags).HasMaxLength(500);

        builder.HasMany(lp => lp.Modules)
            .WithOne(m => m.LearningPath)
            .HasForeignKey(m => m.LearningPathId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(lp => lp.Ratings)
            .WithOne(r => r.LearningPath)
            .HasForeignKey(r => r.LearningPathId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(lp => lp.CreatorId);
        builder.HasIndex(lp => lp.IsPublished);
    }
}
