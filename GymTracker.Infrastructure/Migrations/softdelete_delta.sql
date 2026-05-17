CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
ALTER TABLE exercises ADD deleted_at timestamp with time zone;

ALTER TABLE exercises ADD is_deleted boolean NOT NULL DEFAULT FALSE;

ALTER TABLE routines ADD deleted_at timestamp with time zone;

ALTER TABLE routines ADD is_deleted boolean NOT NULL DEFAULT FALSE;

ALTER TABLE users ADD deleted_at timestamp with time zone;

ALTER TABLE users ADD is_deleted boolean NOT NULL DEFAULT FALSE;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260517220640_AddSoftDeleteToCoreEntities', '10.0.8');

COMMIT;

