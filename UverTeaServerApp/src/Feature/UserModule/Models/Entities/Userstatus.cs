using System;
using System.Collections.Generic;

namespace UverTeaServerApp.src.Feature.UserModule.Models.Entities;

public partial class Userstatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
