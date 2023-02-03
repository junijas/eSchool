using eSchool.Data;
using Microsoft.EntityFrameworkCore;

namespace eSchool.Models
{

    [Keyless]
    public class ClassEnrollmentViewModel
    {
        public ClassViewModel  Class { get; set; }

        public List<StudentEnrollmentViewModel> Students { get; set; } = new
               List<StudentEnrollmentViewModel>();
    }
}
