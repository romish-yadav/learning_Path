using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LearnPath.API.Entities;

namespace LearnPath.API.Configurations;

public class ClassroomConfiguration : IEntityTypeConfiguration<Classroom>
{
    public void Configure(EntityTypeBuilder<Classroom> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Name).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Description).HasMaxLength(1000);
        builder.Property(c => c.JoinCode).HasMaxLength(20);

        builder.HasIndex(c => c.JoinCode).IsUnique();
        builder.HasIndex(c => c.InstructorId);
    }
}

public class UserClassroomConfiguration : IEntityTypeConfiguration<UserClassroom>
{
    public void Configure(EntityTypeBuilder<UserClassroom> builder)
    {
        builder.HasKey(uc => new { uc.UserId, uc.ClassroomId });

        builder.HasOne(uc => uc.User)
            .WithMany(u => u.UserClassrooms)
            .HasForeignKey(uc => uc.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(uc => uc.Classroom)
            .WithMany(c => c.UserClassrooms)
            .HasForeignKey(uc => uc.ClassroomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ClassroomLearningPathConfiguration : IEntityTypeConfiguration<ClassroomLearningPath>
{
    public void Configure(EntityTypeBuilder<ClassroomLearningPath> builder)
    {
        builder.HasKey(clp => new { clp.ClassroomId, clp.LearningPathId });

        builder.HasOne(clp => clp.Classroom)
            .WithMany(c => c.ClassroomLearningPaths)
            .HasForeignKey(clp => clp.ClassroomId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(clp => clp.LearningPath)
            .WithMany(lp => lp.ClassroomLearningPaths)
            .HasForeignKey(clp => clp.LearningPathId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
