namespace UverTeaServerApp.src.Feature.FertilizerModule.Models.Entities;

public partial class UnitOfMeasure
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Fertilizer> Fertilizers { get; set; } = new List<Fertilizer>();
}