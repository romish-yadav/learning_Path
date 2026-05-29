using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LearnPath.API.Entities;

namespace LearnPath.API.Configurations;

public class ModuleConfiguration : IEntityTypeConfiguration<Module>
{
    public void Configure(EntityTypeBuilder<Module> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Title).HasMaxLength(200).IsRequired();
        builder.Property(m => m.Description).HasMaxLength(2000);
        builder.Property(m => m.ResourceUrl).HasMaxLength(500);

        builder.HasIndex(m => new { m.LearningPathId, m.OrderIndex });
    }
}

public class ModuleDependencyConfiguration : IEntityTypeConfiguration<ModuleDependency>
{
    public void Configure(EntityTypeBuilder<ModuleDependency> builder)
    {
        builder.HasKey(md => md.Id);

        builder.HasOne(md => md.Module)
            .WithMany(m => m.Dependents)
            .HasForeignKey(md => md.ModuleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(md => md.PrerequisiteModule)
            .WithMany(m => m.Prerequisites)
            .HasForeignKey(md => md.PrerequisiteModuleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(md => new { md.ModuleId, md.PrerequisiteModuleId }).IsUnique();
    }
}
