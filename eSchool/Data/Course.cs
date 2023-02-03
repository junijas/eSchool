using System;
using System.Collections.Generic;

namespace eSchool.Data;

public partial class Course
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Codes { get; set; }

    public int? Credits { get; set; }

   public virtual ICollection<Class> Classes { get; } = new List<Class>();
}
