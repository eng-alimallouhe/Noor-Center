using LMS.Domain.Entities.Users;

namespace LMS.Domain.Entities.HR
{
    public class Penalty
    {
        //primary key
        public int PenaltyId { get; set; }

        //Foreign Key: EmployeeId ==> one(employee)-to-many(penalties) relationship
        public int EmployeeId { get; set; }

        public decimal Amount { get; set; }
        public string DecisionFileUrl { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        //Soft delete
        public bool IsActive { get; set; }

        //Navigation Property:
        public Employee Employee { get; set; } = new Employee();
    }
}
