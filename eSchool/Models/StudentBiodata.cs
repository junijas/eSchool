using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


namespace eSchool.Data
{
    public class StudentBiodata
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]     
    public string? FirstName { get; set; }

    public string? Surname { get; set; }
        [Display(Name = "Date of Birth")]
    public DateTime? Dob { get; set; }
    }

    [ModelMetadataType(typeof(StudentBiodata))]
    public partial class Student {}
}