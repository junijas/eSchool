using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace eSchool.Data;

public partial class Lecturer
{
    public int Id { get; set; }
    
    public string? FirstName { get; set; }

    public string? Surname { get; set; }

    public DateTime? Doe { get; set; }
}
