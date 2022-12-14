CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

CREATE TABLE "Actors" (
    "Sid" integer GENERATED BY DEFAULT AS IDENTITY,
    "Id" uuid NOT NULL,
    "Type" integer NOT NULL,
    "Name" character varying(256) NOT NULL,
    "IsDeleted" boolean NOT NULL,
    "Email" character varying(256) NULL,
    "Picture" character varying(2048) NULL,
    CONSTRAINT "PK_Actors" PRIMARY KEY ("Sid")
);

CREATE TABLE "ApiKeys" (
    "Sid" integer GENERATED BY DEFAULT AS IDENTITY,
    "KeyHash" text NOT NULL,
    "Name" character varying(256) NOT NULL,
    "Description" text NULL,
    "ExpiresAt" timestamp with time zone NULL,
    "IsExpired" boolean NOT NULL DEFAULT FALSE,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_ApiKeys" PRIMARY KEY ("Sid")
);

CREATE TABLE "Events" (
    "Sid" bigint GENERATED BY DEFAULT AS IDENTITY,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "OccurredAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "UserId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "EventType" character varying(256) NOT NULL,
    "EventData" jsonb NOT NULL,
    "AggregateType" character varying(256) NOT NULL,
    "AggregateId" uuid NOT NULL,
    CONSTRAINT "PK_Events" PRIMARY KEY ("Sid")
);

CREATE TABLE "JwtBlacklist" (
    "Sid" bigint GENERATED BY DEFAULT AS IDENTITY,
    "Id" uuid NOT NULL,
    "ExpiresAt" timestamp with time zone NULL,
    CONSTRAINT "PK_JwtBlacklist" PRIMARY KEY ("Sid")
);

CREATE TABLE "Logs" (
    "Sid" bigint GENERATED BY DEFAULT AS IDENTITY,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "Method" character varying(10) NOT NULL,
    "Url" character varying(2048) NOT NULL,
    "IpAddress" character varying(40) NULL,
    "StatusCode" integer NULL,
    "StartedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "EndedAt" timestamp with time zone NULL,
    "Duration" interval NULL,
    "ActorId" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "SessionId" uuid NULL,
    "UserId" uuid NULL,
    "Errors" jsonb NULL,
    "HasErrors" boolean NOT NULL DEFAULT FALSE,
    "Succeeded" boolean NOT NULL DEFAULT FALSE,
    "Request" jsonb NULL,
    CONSTRAINT "PK_Logs" PRIMARY KEY ("Sid")
);

CREATE TABLE "Messages" (
    "Sid" integer GENERATED BY DEFAULT AS IDENTITY,
    "IsDemo" boolean NOT NULL,
    "Subject" character varying(256) NOT NULL,
    "Body" text NOT NULL,
    "Recipients" jsonb NOT NULL,
    "SenderId" uuid NOT NULL,
    "SenderIsDefault" boolean NOT NULL,
    "SenderProvider" integer NOT NULL,
    "SenderAddress" character varying(256) NOT NULL,
    "SenderDisplayName" character varying(256) NULL,
    "RealmId" uuid NULL,
    "RealmAlias" character varying(256) NULL,
    "RealmName" character varying(256) NULL,
    "TemplateId" uuid NOT NULL,
    "TemplateKey" character varying(256) NOT NULL,
    "TemplateContentType" character varying(256) NOT NULL,
    "TemplateDisplayName" character varying(256) NULL,
    "IgnoreUserLocale" boolean NOT NULL,
    "Locale" character varying(16) NULL,
    "Variables" jsonb NULL,
    "Errors" jsonb NULL,
    "HasErrors" boolean NOT NULL,
    "Result" jsonb NULL,
    "Succeeded" boolean NOT NULL,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Messages" PRIMARY KEY ("Sid")
);

CREATE TABLE "Realms" (
    "Sid" integer GENERATED BY DEFAULT AS IDENTITY,
    "Alias" character varying(256) NOT NULL,
    "AliasNormalized" character varying(256) NOT NULL,
    "Name" character varying(256) NOT NULL,
    "Description" text NULL,
    "AllowedUsernameCharacters" text NULL,
    "RequireConfirmedAccount" boolean NOT NULL DEFAULT FALSE,
    "RequireUniqueEmail" boolean NOT NULL DEFAULT FALSE,
    "DefaultLocale" character varying(16) NULL,
    "Url" character varying(2048) NULL,
    "PasswordSettings" jsonb NULL,
    "GoogleClientId" character varying(256) NULL,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Realms" PRIMARY KEY ("Sid")
);

CREATE TABLE "Dictionaries" (
    "Sid" integer GENERATED BY DEFAULT AS IDENTITY,
    "RealmSid" integer NULL,
    "Locale" character varying(16) NOT NULL,
    "Entries" jsonb NULL,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Dictionaries" PRIMARY KEY ("Sid"),
    CONSTRAINT "FK_Dictionaries_Realms_RealmSid" FOREIGN KEY ("RealmSid") REFERENCES "Realms" ("Sid")
);

CREATE TABLE "Senders" (
    "Sid" integer GENERATED BY DEFAULT AS IDENTITY,
    "RealmSid" integer NULL,
    "IsDefault" boolean NOT NULL DEFAULT FALSE,
    "EmailAddress" character varying(256) NOT NULL,
    "DisplayName" character varying(256) NULL,
    "Provider" integer NOT NULL DEFAULT 0,
    "Settings" jsonb NULL,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Senders" PRIMARY KEY ("Sid"),
    CONSTRAINT "FK_Senders_Realms_RealmSid" FOREIGN KEY ("RealmSid") REFERENCES "Realms" ("Sid")
);

CREATE TABLE "Templates" (
    "Sid" integer GENERATED BY DEFAULT AS IDENTITY,
    "RealmSid" integer NULL,
    "Key" character varying(256) NOT NULL,
    "KeyNormalized" character varying(256) NOT NULL,
    "Subject" character varying(256) NOT NULL,
    "ContentType" character varying(256) NOT NULL DEFAULT 'text/plain',
    "Contents" text NOT NULL,
    "DisplayName" character varying(256) NULL,
    "Description" text NULL,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Templates" PRIMARY KEY ("Sid"),
    CONSTRAINT "FK_Templates_Realms_RealmSid" FOREIGN KEY ("RealmSid") REFERENCES "Realms" ("Sid")
);

CREATE TABLE "Users" (
    "Sid" integer GENERATED BY DEFAULT AS IDENTITY,
    "RealmSid" integer NULL,
    "Username" character varying(256) NOT NULL,
    "UsernameNormalized" character varying(256) NOT NULL,
    "PasswordChangedAt" timestamp with time zone NULL,
    "PasswordHash" text NULL,
    "HasPassword" boolean NOT NULL DEFAULT FALSE,
    "Email" character varying(256) NULL,
    "EmailNormalized" character varying(256) NULL,
    "EmailConfirmedAt" timestamp with time zone NULL,
    "EmailConfirmedById" uuid NULL,
    "IsEmailConfirmed" boolean NOT NULL DEFAULT FALSE,
    "PhoneNumber" text NULL,
    "PhoneNumberConfirmedAt" timestamp with time zone NULL,
    "PhoneNumberConfirmedById" uuid NULL,
    "IsPhoneNumberConfirmed" boolean NOT NULL DEFAULT FALSE,
    "IsAccountConfirmed" boolean NOT NULL DEFAULT FALSE,
    "FirstName" character varying(128) NULL,
    "MiddleName" character varying(128) NULL,
    "LastName" character varying(128) NULL,
    "Locale" character varying(16) NULL,
    "Picture" character varying(2048) NULL,
    "SignedInAt" timestamp with time zone NULL,
    "DisabledAt" timestamp with time zone NULL,
    "DisabledById" uuid NULL,
    "IsDisabled" boolean NOT NULL DEFAULT FALSE,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Sid"),
    CONSTRAINT "FK_Users_Realms_RealmSid" FOREIGN KEY ("RealmSid") REFERENCES "Realms" ("Sid")
);

CREATE TABLE "RealmPasswordRecoverySenders" (
    "RealmSid" integer NOT NULL,
    "SenderSid" integer NOT NULL,
    CONSTRAINT "PK_RealmPasswordRecoverySenders" PRIMARY KEY ("RealmSid"),
    CONSTRAINT "FK_RealmPasswordRecoverySenders_Realms_RealmSid" FOREIGN KEY ("RealmSid") REFERENCES "Realms" ("Sid") ON DELETE CASCADE,
    CONSTRAINT "FK_RealmPasswordRecoverySenders_Senders_SenderSid" FOREIGN KEY ("SenderSid") REFERENCES "Senders" ("Sid") ON DELETE CASCADE
);

CREATE TABLE "RealmPasswordRecoveryTemplates" (
    "RealmSid" integer NOT NULL,
    "TemplateSid" integer NOT NULL,
    CONSTRAINT "PK_RealmPasswordRecoveryTemplates" PRIMARY KEY ("RealmSid"),
    CONSTRAINT "FK_RealmPasswordRecoveryTemplates_Realms_RealmSid" FOREIGN KEY ("RealmSid") REFERENCES "Realms" ("Sid") ON DELETE CASCADE,
    CONSTRAINT "FK_RealmPasswordRecoveryTemplates_Templates_TemplateSid" FOREIGN KEY ("TemplateSid") REFERENCES "Templates" ("Sid") ON DELETE CASCADE
);

CREATE TABLE "ExternalProviders" (
    "Sid" integer GENERATED BY DEFAULT AS IDENTITY,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "AddedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "AddedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "RealmSid" integer NOT NULL,
    "UserSid" integer NOT NULL,
    "Key" character varying(256) NOT NULL,
    "Value" character varying(256) NOT NULL,
    "DisplayName" character varying(256) NULL,
    CONSTRAINT "PK_ExternalProviders" PRIMARY KEY ("Sid"),
    CONSTRAINT "FK_ExternalProviders_Realms_RealmSid" FOREIGN KEY ("RealmSid") REFERENCES "Realms" ("Sid") ON DELETE CASCADE,
    CONSTRAINT "FK_ExternalProviders_Users_UserSid" FOREIGN KEY ("UserSid") REFERENCES "Users" ("Sid") ON DELETE CASCADE
);

CREATE TABLE "Sessions" (
    "Sid" integer GENERATED BY DEFAULT AS IDENTITY,
    "UserSid" integer NOT NULL,
    "KeyHash" text NULL,
    "IsPersistent" boolean NOT NULL DEFAULT FALSE,
    "SignedOutAt" timestamp with time zone NULL,
    "SignedOutById" uuid NULL,
    "IsActive" boolean NOT NULL DEFAULT FALSE,
    "IpAddress" character varying(40) NULL,
    "AdditionalInformation" jsonb NULL,
    "Id" uuid NOT NULL DEFAULT (uuid_generate_v4()),
    "CreatedAt" timestamp with time zone NOT NULL DEFAULT (now()),
    "CreatedById" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
    "UpdatedAt" timestamp with time zone NULL,
    "UpdatedById" uuid NULL,
    "Version" integer NOT NULL DEFAULT 0,
    CONSTRAINT "PK_Sessions" PRIMARY KEY ("Sid"),
    CONSTRAINT "FK_Sessions_Users_UserSid" FOREIGN KEY ("UserSid") REFERENCES "Users" ("Sid")
);

CREATE UNIQUE INDEX "IX_Actors_Id" ON "Actors" ("Id");

CREATE INDEX "IX_Actors_Type" ON "Actors" ("Type");

CREATE UNIQUE INDEX "IX_ApiKeys_Id" ON "ApiKeys" ("Id");

CREATE INDEX "IX_ApiKeys_IsExpired" ON "ApiKeys" ("IsExpired");

CREATE INDEX "IX_ApiKeys_Name" ON "ApiKeys" ("Name");

CREATE UNIQUE INDEX "IX_Dictionaries_Id" ON "Dictionaries" ("Id");

CREATE UNIQUE INDEX "IX_Dictionaries_RealmSid_Locale" ON "Dictionaries" ("RealmSid", "Locale");

CREATE UNIQUE INDEX "IX_Events_Id" ON "Events" ("Id");

CREATE UNIQUE INDEX "IX_ExternalProviders_Id" ON "ExternalProviders" ("Id");

CREATE UNIQUE INDEX "IX_ExternalProviders_RealmSid_Key_Value" ON "ExternalProviders" ("RealmSid", "Key", "Value");

CREATE INDEX "IX_ExternalProviders_UserSid" ON "ExternalProviders" ("UserSid");

CREATE INDEX "IX_JwtBlacklist_ExpiresAt" ON "JwtBlacklist" ("ExpiresAt");

CREATE UNIQUE INDEX "IX_JwtBlacklist_Id" ON "JwtBlacklist" ("Id");

CREATE UNIQUE INDEX "IX_Logs_Id" ON "Logs" ("Id");

CREATE INDEX "IX_Messages_HasErrors" ON "Messages" ("HasErrors");

CREATE UNIQUE INDEX "IX_Messages_Id" ON "Messages" ("Id");

CREATE INDEX "IX_Messages_IsDemo" ON "Messages" ("IsDemo");

CREATE INDEX "IX_Messages_RealmAlias" ON "Messages" ("RealmAlias");

CREATE INDEX "IX_Messages_RealmId" ON "Messages" ("RealmId");

CREATE INDEX "IX_Messages_RealmName" ON "Messages" ("RealmName");

CREATE INDEX "IX_Messages_SenderAddress" ON "Messages" ("SenderAddress");

CREATE INDEX "IX_Messages_SenderDisplayName" ON "Messages" ("SenderDisplayName");

CREATE INDEX "IX_Messages_SenderId" ON "Messages" ("SenderId");

CREATE INDEX "IX_Messages_SenderProvider" ON "Messages" ("SenderProvider");

CREATE INDEX "IX_Messages_Subject" ON "Messages" ("Subject");

CREATE INDEX "IX_Messages_Succeeded" ON "Messages" ("Succeeded");

CREATE INDEX "IX_Messages_TemplateContentType" ON "Messages" ("TemplateContentType");

CREATE INDEX "IX_Messages_TemplateDisplayName" ON "Messages" ("TemplateDisplayName");

CREATE INDEX "IX_Messages_TemplateId" ON "Messages" ("TemplateId");

CREATE INDEX "IX_Messages_TemplateKey" ON "Messages" ("TemplateKey");

CREATE INDEX "IX_RealmPasswordRecoverySenders_SenderSid" ON "RealmPasswordRecoverySenders" ("SenderSid");

CREATE INDEX "IX_RealmPasswordRecoveryTemplates_TemplateSid" ON "RealmPasswordRecoveryTemplates" ("TemplateSid");

CREATE INDEX "IX_Realms_Alias" ON "Realms" ("Alias");

CREATE UNIQUE INDEX "IX_Realms_AliasNormalized" ON "Realms" ("AliasNormalized");

CREATE UNIQUE INDEX "IX_Realms_Id" ON "Realms" ("Id");

CREATE INDEX "IX_Realms_Name" ON "Realms" ("Name");

CREATE INDEX "IX_Senders_DisplayName" ON "Senders" ("DisplayName");

CREATE INDEX "IX_Senders_EmailAddress" ON "Senders" ("EmailAddress");

CREATE UNIQUE INDEX "IX_Senders_Id" ON "Senders" ("Id");

CREATE INDEX "IX_Senders_IsDefault" ON "Senders" ("IsDefault");

CREATE INDEX "IX_Senders_Provider" ON "Senders" ("Provider");

CREATE INDEX "IX_Senders_RealmSid" ON "Senders" ("RealmSid");

CREATE UNIQUE INDEX "IX_Sessions_Id" ON "Sessions" ("Id");

CREATE INDEX "IX_Sessions_UserSid" ON "Sessions" ("UserSid");

CREATE INDEX "IX_Templates_DisplayName" ON "Templates" ("DisplayName");

CREATE UNIQUE INDEX "IX_Templates_Id" ON "Templates" ("Id");

CREATE INDEX "IX_Templates_Key" ON "Templates" ("Key");

CREATE UNIQUE INDEX "IX_Templates_RealmSid_KeyNormalized" ON "Templates" ("RealmSid", "KeyNormalized");

CREATE INDEX "IX_Users_Email" ON "Users" ("Email");

CREATE INDEX "IX_Users_FirstName" ON "Users" ("FirstName");

CREATE UNIQUE INDEX "IX_Users_Id" ON "Users" ("Id");

CREATE INDEX "IX_Users_IsAccountConfirmed" ON "Users" ("IsAccountConfirmed");

CREATE INDEX "IX_Users_IsDisabled" ON "Users" ("IsDisabled");

CREATE INDEX "IX_Users_LastName" ON "Users" ("LastName");

CREATE INDEX "IX_Users_MiddleName" ON "Users" ("MiddleName");

CREATE INDEX "IX_Users_PhoneNumber" ON "Users" ("PhoneNumber");

CREATE UNIQUE INDEX "IX_Users_RealmSid_UsernameNormalized" ON "Users" ("RealmSid", "UsernameNormalized");

CREATE INDEX "IX_Users_Username" ON "Users" ("Username");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220812215937_InitialMigration', '6.0.7');

COMMIT;
