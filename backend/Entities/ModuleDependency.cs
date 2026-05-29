namespace LearnPath.API.Entities;

public class ModuleDependency
{
    public Guid Id { get; set; }
    public Guid ModuleId { get; set; }
    public Module Module { get; set; } = null!;

    public Guid PrerequisiteModuleId { get; set; }
    public Module PrerequisiteModule { get; set; } = null!;
}
