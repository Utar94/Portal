﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portal.Core.Realms;

namespace Portal.Infrastructure.Configurations
{
  internal class RealmConfiguration : AggregateConfiguration<Realm>, IEntityTypeConfiguration<Realm>
  {
    public override void Configure(EntityTypeBuilder<Realm> builder)
    {
      base.Configure(builder);

      builder.HasIndex(x => x.Alias);
      builder.HasIndex(x => x.AliasNormalized).IsUnique();
      builder.HasIndex(x => x.Name);

      builder.Property(x => x.Alias).HasMaxLength(256);
      builder.Property(x => x.AliasNormalized).HasMaxLength(256);
      builder.Property(x => x.Name).HasMaxLength(256);
    }
  }
}
