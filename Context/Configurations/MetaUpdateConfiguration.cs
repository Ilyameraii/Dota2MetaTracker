using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Context.Configurations;

/// <summary>
///     Конфигурация сущности обновления статистики
/// </summary>
public class MetaUpdateConfiguration : IEntityTypeConfiguration<MetaUpdate>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<MetaUpdate> builder)
    {
        builder.ToTable("MetaUpdates");

        builder.HasKey(metaUpdate => metaUpdate.Id);

        builder.Property(metaUpdate => metaUpdate.DateTime)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.HasIndex(metaUpdate => metaUpdate.DateTime);

        builder.HasMany(metaUpdate => metaUpdate.HeroStats)
            .WithOne(heroStat => heroStat.MetaUpdate)
            .HasForeignKey(heroStat => heroStat.MetaUpdateId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
