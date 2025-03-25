using LMS.Domain.Entities.Users;

namespace LMS.Domain.Entities.HR
{
    public class LeaveBalance
    {
        //primary key
        public int LeaveBalanceId { get; set; }

        //Foreign Key: EmployeeId ==> one(employee)-to-one(leavebalance) relationship
        public int EmployeeId { get; set; }
        public int RemainBalance { get; set; }
        public int BaseBalance { get; set; }
        public int TotalBalance { get; set; }
        public int RoundedBalance { get; set; }
        public int Year { get; set; }

        //Navigation Property:
        public Employee Employee { get; set; } = new Employee();
    }
}
