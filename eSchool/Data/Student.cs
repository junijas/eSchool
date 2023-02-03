using System;
using System.Collections.Generic;

namespace eSchool.Data;

public partial class Student
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? Surname { get; set; }

    public DateTime? Dob { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; } = new List<Enrollment>();
}
