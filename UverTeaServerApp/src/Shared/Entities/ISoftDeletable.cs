namespace UverTeaServerApp.Shared.Entities;

/// <summary>
/// Marker and contract for entities supporting soft-deletion.
/// </summary>
public interface ISoftDeletable
{
    bool IsDeleted { get; set; }
}
