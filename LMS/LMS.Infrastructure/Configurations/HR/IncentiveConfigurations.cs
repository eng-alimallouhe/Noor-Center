using LMS.Domain.Entities.HR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BrightCenter.Infrastructure.Configurations.EmployeesManagement
{
    public class IncentiveConfigurations :
        IEntityTypeConfiguration<Incentive>
    {
        public void Configure(EntityTypeBuilder<Incentive> builder)
        {
            builder.ToTable("Incentives");

            builder.HasKey(i => i.IncentiveId);

            builder.Property(i => i.IncentiveId)
                    .ValueGeneratedOnAdd();

            builder.Property(i => i.Amount)
                    .HasColumnType("decimal(5,2)")
                    .IsRequired();

            builder.Property(i => i.IsActive)
                    .IsRequired();

            builder.Property(i => i.Date)
                    .IsRequired();

            builder.HasOne(i => i.Employee)
                .WithMany(e => e.Incentives)
                .HasForeignKey(i => i.EmployeeId)
                .IsRequired();
        }
    }
}
