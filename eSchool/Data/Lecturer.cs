using System;
using System.Collections.Generic;

namespace eSchool.Data;

public partial class Lecturer
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? Surname { get; set; }

    public DateTime? Doe { get; set; }

    public virtual ICollection<Class> Classes { get; } = new List<Class>();
}
