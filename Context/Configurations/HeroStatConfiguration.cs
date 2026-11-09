using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Context.Configurations;

/// <summary>
///     Конфигурация сущности статистики персонажа
/// </summary>
public class HeroStatConfiguration : IEntityTypeConfiguration<HeroStat>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<HeroStat> builder)
    {
        builder.ToTable("HeroStats");

        builder.HasKey(heroStat => heroStat.Id);

        builder.Property(heroStat => heroStat.Rank)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(heroStat => heroStat.Role)
            .HasConversion<int>()
            .IsRequired();

        builder.HasIndex(heroStat => new
        {
            heroStat.MetaUpdateId,
            heroStat.HeroId,
            heroStat.Rank,
            heroStat.Role
        }).IsUnique();
    }
}
