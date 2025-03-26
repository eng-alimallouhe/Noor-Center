﻿using LMS.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Configurations.Users
{
    public class OtpConfigurations : IEntityTypeConfiguration<OtpCode>
    {
        public void Configure(EntityTypeBuilder<OtpCode> builder)
        {
            builder.HasKey(x => x.OtpCodeId);
            
            builder.Property(x => x.OtpCodeId)
                    .ValueGeneratedOnAdd();
            
            builder.Property(x => x.OtpCodeValue)
                    .HasMaxLength(8)
                    .IsRequired();
            
            builder.Property(x => x.ExpiredAt)
                    .IsRequired();
            
            builder.Property(x => x.FailedAttempts)
                    .IsRequired();

            builder.Property(x => x.UserId)
                    .IsRequired();
            
            builder.HasOne(x => x.User)
                    .WithOne()
                    .HasForeignKey<OtpCode>(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
