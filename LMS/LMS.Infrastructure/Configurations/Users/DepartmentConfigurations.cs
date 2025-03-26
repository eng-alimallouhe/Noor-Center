using LMS.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations.Users
{
    public class DepartmentConfigurations :
        IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.DepartmentId);
            
            builder.Property(d => d.DepartmentId)
                    .ValueGeneratedOnAdd();

            builder.Property(d => d.DepartmentName)
                    .IsRequired()
                    .HasMaxLength(60);
            
            builder.Property(d => d.Description)
                    .IsRequired()
                    .HasMaxLength(512);
            
            builder.Property(d => d.IsActive)
                    .IsRequired();
            
            builder.Property(d => d.CreatedAt)
                    .IsRequired();
            
            builder.Property(d => d.UpdatedAt)
                    .IsRequired();
        }
    }
}
