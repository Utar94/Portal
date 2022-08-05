﻿// <auto-generated />
using System;
using Logitar.Portal.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Logitar.Portal.Infrastructure.Migrations
{
    [DbContext(typeof(PortalDbContext))]
    [Migration("20220805142958_ReworkForeignKeys")]
    partial class ReworkForeignKeys
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Logitar.Portal.Core.ApiKeys.ApiKey", b =>
                {
                    b.Property<int>("Sid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Sid"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("CreatedById")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<bool>("IsExpired")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("KeyHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Sid");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("IsExpired");

                    b.HasIndex("Name");

                    b.ToTable("ApiKeys");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Emails.Messages.Message", b =>
                {
                    b.Property<int>("Sid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Sid"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("CreatedById")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<string>("ErrorsSerialized")
                        .HasColumnType("jsonb")
                        .HasColumnName("Errors");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("boolean");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<bool>("IsDemo")
                        .HasColumnType("boolean");

                    b.Property<string>("RealmAlias")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid?>("RealmId")
                        .HasColumnType("uuid");

                    b.Property<string>("RealmName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("RecipientsSerialized")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("Recipients");

                    b.Property<string>("ResultSerialized")
                        .HasColumnType("jsonb")
                        .HasColumnName("Result");

                    b.Property<string>("SenderAddress")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("SenderDisplayName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uuid");

                    b.Property<bool>("SenderIsDefault")
                        .HasColumnType("boolean");

                    b.Property<int>("SenderProvider")
                        .HasColumnType("integer");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("Succeeded")
                        .HasColumnType("boolean");

                    b.Property<string>("TemplateContentType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("TemplateDisplayName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("TemplateId")
                        .HasColumnType("uuid");

                    b.Property<string>("TemplateKey")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<string>("VariablesSerialized")
                        .HasColumnType("jsonb")
                        .HasColumnName("Variables");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Sid");

                    b.HasIndex("HasErrors");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("IsDemo");

                    b.HasIndex("RealmAlias");

                    b.HasIndex("RealmId");

                    b.HasIndex("RealmName");

                    b.HasIndex("SenderAddress");

                    b.HasIndex("SenderDisplayName");

                    b.HasIndex("SenderId");

                    b.HasIndex("SenderProvider");

                    b.HasIndex("Subject");

                    b.HasIndex("Succeeded");

                    b.HasIndex("TemplateContentType");

                    b.HasIndex("TemplateDisplayName");

                    b.HasIndex("TemplateId");

                    b.HasIndex("TemplateKey");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Emails.Senders.Sender", b =>
                {
                    b.Property<int>("Sid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Sid"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("CreatedById")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<string>("DisplayName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<bool>("IsDefault")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int>("Provider")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<int?>("RealmSid")
                        .HasColumnType("integer");

                    b.Property<string>("SettingsSerialized")
                        .HasColumnType("jsonb")
                        .HasColumnName("Settings");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Sid");

                    b.HasIndex("DisplayName");

                    b.HasIndex("EmailAddress");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("IsDefault");

                    b.HasIndex("Provider");

                    b.HasIndex("RealmSid");

                    b.ToTable("Senders");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Emails.Templates.Template", b =>
                {
                    b.Property<int>("Sid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Sid"));

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasDefaultValue("text/plain");

                    b.Property<string>("Contents")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("CreatedById")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("KeyNormalized")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int?>("RealmSid")
                        .HasColumnType("integer");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Sid");

                    b.HasIndex("DisplayName");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Key");

                    b.HasIndex("RealmSid", "KeyNormalized")
                        .IsUnique();

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Realms.PasswordRecoverySender", b =>
                {
                    b.Property<int>("RealmSid")
                        .HasColumnType("integer");

                    b.Property<int>("SenderSid")
                        .HasColumnType("integer");

                    b.HasKey("RealmSid");

                    b.HasIndex("SenderSid");

                    b.ToTable("RealmPasswordRecoverySenders", (string)null);
                });

            modelBuilder.Entity("Logitar.Portal.Core.Realms.PasswordRecoveryTemplate", b =>
                {
                    b.Property<int>("RealmSid")
                        .HasColumnType("integer");

                    b.Property<int>("TemplateSid")
                        .HasColumnType("integer");

                    b.HasKey("RealmSid");

                    b.HasIndex("TemplateSid");

                    b.ToTable("RealmPasswordRecoveryTemplates", (string)null);
                });

            modelBuilder.Entity("Logitar.Portal.Core.Realms.Realm", b =>
                {
                    b.Property<int>("Sid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Sid"));

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("AliasNormalized")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("AllowedUsernameCharacters")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("CreatedById")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("GoogleClientId")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordSettingsSerialized")
                        .HasColumnType("jsonb")
                        .HasColumnName("PasswordSettings");

                    b.Property<bool>("RequireConfirmedAccount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("RequireUniqueEmail")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Url")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Sid");

                    b.HasIndex("Alias");

                    b.HasIndex("AliasNormalized")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Name");

                    b.ToTable("Realms");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Sessions.Session", b =>
                {
                    b.Property<int>("Sid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Sid"));

                    b.Property<string>("AdditionalInformation")
                        .HasColumnType("jsonb");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("CreatedById")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsPersistent")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("KeyHash")
                        .HasColumnType("text");

                    b.Property<DateTime?>("SignedOutAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("SignedOutById")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<int>("UserSid")
                        .HasColumnType("integer");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Sid");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserSid");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Users.ExternalProvider", b =>
                {
                    b.Property<int>("Sid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Sid"));

                    b.Property<DateTime>("AddedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("AddedById")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<string>("DisplayName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int?>("UserSid")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Sid");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("UserSid");

                    b.HasIndex("Key", "Value");

                    b.ToTable("ExternalProviders", (string)null);
                });

            modelBuilder.Entity("Logitar.Portal.Core.Users.User", b =>
                {
                    b.Property<int>("Sid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Sid"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("CreatedById")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.Property<DateTime?>("DisabledAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("DisabledById")
                        .HasColumnType("uuid");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("EmailConfirmedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("EmailConfirmedById")
                        .HasColumnType("uuid");

                    b.Property<string>("EmailNormalized")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<bool>("HasPassword")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<bool>("IsAccountConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsDisabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsEmailConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsPhoneNumberConfirmed")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<string>("Locale")
                        .HasMaxLength(16)
                        .HasColumnType("character varying(16)");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<DateTime?>("PasswordChangedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<DateTime?>("PhoneNumberConfirmedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("PhoneNumberConfirmedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Picture")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<int?>("RealmSid")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("SignedInAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("UpdatedById")
                        .HasColumnType("uuid");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("UsernameNormalized")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("Version")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Sid");

                    b.HasIndex("Email");

                    b.HasIndex("FirstName");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("IsAccountConfirmed");

                    b.HasIndex("IsDisabled");

                    b.HasIndex("LastName");

                    b.HasIndex("MiddleName");

                    b.HasIndex("PhoneNumber");

                    b.HasIndex("Username");

                    b.HasIndex("RealmSid", "UsernameNormalized")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Logitar.Portal.Infrastructure.Entities.BlacklistedJwt", b =>
                {
                    b.Property<long>("Sid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Sid"));

                    b.Property<DateTime?>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.HasKey("Sid");

                    b.HasIndex("ExpiresAt");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("JwtBlacklist");
                });

            modelBuilder.Entity("Logitar.Portal.Infrastructure.Entities.Event", b =>
                {
                    b.Property<long>("Sid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Sid"));

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("uuid");

                    b.Property<string>("AggregateType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("EventData")
                        .IsRequired()
                        .HasColumnType("jsonb");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("OccurredAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

                    b.HasKey("Sid");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Emails.Senders.Sender", b =>
                {
                    b.HasOne("Logitar.Portal.Core.Realms.Realm", "Realm")
                        .WithMany()
                        .HasForeignKey("RealmSid");

                    b.Navigation("Realm");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Emails.Templates.Template", b =>
                {
                    b.HasOne("Logitar.Portal.Core.Realms.Realm", "Realm")
                        .WithMany()
                        .HasForeignKey("RealmSid");

                    b.Navigation("Realm");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Realms.PasswordRecoverySender", b =>
                {
                    b.HasOne("Logitar.Portal.Core.Realms.Realm", "Realm")
                        .WithOne("PasswordRecoverySenderRelation")
                        .HasForeignKey("Logitar.Portal.Core.Realms.PasswordRecoverySender", "RealmSid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Logitar.Portal.Core.Emails.Senders.Sender", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderSid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Realm");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Realms.PasswordRecoveryTemplate", b =>
                {
                    b.HasOne("Logitar.Portal.Core.Realms.Realm", "Realm")
                        .WithOne("PasswordRecoveryTemplateRelation")
                        .HasForeignKey("Logitar.Portal.Core.Realms.PasswordRecoveryTemplate", "RealmSid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Logitar.Portal.Core.Emails.Templates.Template", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateSid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Realm");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Sessions.Session", b =>
                {
                    b.HasOne("Logitar.Portal.Core.Users.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserSid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Users.ExternalProvider", b =>
                {
                    b.HasOne("Logitar.Portal.Core.Users.User", "User")
                        .WithMany("ExternalProviders")
                        .HasForeignKey("UserSid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Users.User", b =>
                {
                    b.HasOne("Logitar.Portal.Core.Realms.Realm", "Realm")
                        .WithMany("Users")
                        .HasForeignKey("RealmSid");

                    b.Navigation("Realm");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Realms.Realm", b =>
                {
                    b.Navigation("PasswordRecoverySenderRelation");

                    b.Navigation("PasswordRecoveryTemplateRelation");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Logitar.Portal.Core.Users.User", b =>
                {
                    b.Navigation("ExternalProviders");

                    b.Navigation("Sessions");
                });
#pragma warning restore 612, 618
        }
    }
}
