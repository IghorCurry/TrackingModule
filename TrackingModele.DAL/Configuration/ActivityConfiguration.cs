using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TrackingModele.DAL.Entities;

namespace TrackingModele.DAL.Configuration
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> modelBuilder)
        {
            modelBuilder
            .HasOne(a => a.Employee)
            .WithMany()
            .HasForeignKey(a => a.EmployeeId);

            modelBuilder
                    .HasOne(a => a.Project)
                    .WithMany()
                    .HasForeignKey(a => a.ProjectId);

        }
    }

}
