CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211209180922_AddMeetups') THEN
    CREATE TABLE meetups (
        id uuid NOT NULL,
        title text NULL,
        description text NULL,
        speaker text NULL,
        duration interval NOT NULL,
        start_time timestamp with time zone NOT NULL,
        CONSTRAINT pk_meetups PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211209180922_AddMeetups') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20211209180922_AddMeetups', '6.0.0');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211213082755_AddSignedUpUsers') THEN
    CREATE TABLE signed_up_users (
        id uuid NOT NULL,
        meetup_id uuid NULL,
        CONSTRAINT pk_signed_up_users PRIMARY KEY (id),
        CONSTRAINT fk_meetups_signed_up_users FOREIGN KEY (meetup_id) REFERENCES meetups (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211213082755_AddSignedUpUsers') THEN
    CREATE UNIQUE INDEX uix_signed_up_users_meetup_id ON signed_up_users (meetup_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211213082755_AddSignedUpUsers') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20211213082755_AddSignedUpUsers', '6.0.0');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211215194403_FixSignedUpUsersPK') THEN
    ALTER TABLE signed_up_users DROP CONSTRAINT fk_meetups_signed_up_users;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211215194403_FixSignedUpUsersPK') THEN
    ALTER TABLE signed_up_users DROP CONSTRAINT pk_signed_up_users;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211215194403_FixSignedUpUsersPK') THEN
    ALTER TABLE signed_up_users ALTER COLUMN meetup_id SET NOT NULL;
    ALTER TABLE signed_up_users ALTER COLUMN meetup_id SET DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211215194403_FixSignedUpUsersPK') THEN
    ALTER TABLE signed_up_users ADD CONSTRAINT pk_signed_up_users PRIMARY KEY (id, meetup_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211215194403_FixSignedUpUsersPK') THEN
    ALTER TABLE signed_up_users ADD CONSTRAINT fk_meetups_signed_up_users FOREIGN KEY (meetup_id) REFERENCES meetups (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211215194403_FixSignedUpUsersPK') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20211215194403_FixSignedUpUsersPK', '6.0.0');
    END IF;
END $EF$;
COMMIT;

