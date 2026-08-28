namespace UverTeaServerApp.Shared.Entities;

public interface IAuditableEntity
{
    DateTime Createdat { get; set; }
    DateTime? Updatedat { get; set; }
}