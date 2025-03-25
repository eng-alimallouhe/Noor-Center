using LMS.Domain.Entities.Users;
using LMS.Domain.Enums.HR;

namespace LMS.Domain.Entities.HR
{
    public class Salary
    {
        //primary key
        public int SalaryId { get; set; }

        //Foreign Key: EmployeeId ==> one(employee)-to-many(salaries) relationship
        public int EmployeeId { get; set; }

        public Month Month { get; set; }
        public decimal Value { get; set; }
        public int Year { get; set; }

        //Soft Delete: 
        public bool IsActive { get; set; }

        //Navigation Property:
        public Employee Employee { get; set; } = new Employee();
    }
}
