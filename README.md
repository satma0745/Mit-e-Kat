# Mit-e-Kat

Application backend for organizing and conducting meetups.


## Dependencies

### Development dependencies

- .Net 6.0+ (with the dotnet-ef tool)
- Docker (with the docker-compose)
- PostgreSQL 12.0+

### Production dependencies

- Docker (with the docker-compose)
- PostgreSQL 12.0+ (with the psql tool)


## How to run

### Run production

Install all [Production dependencies](#Production-dependencies).

Then you need to clone the repo:
```
git clone https://github.com/satma0745/Mit-e-Kat.git
```

Create an environment file for the docker-compose (I personally prefer to name
it `docker-compose.env`). That file must contain all variables mentioned in the
`docker-compose.yml` configuration. I would recommend the following values, but
you can use the ones you like best:
```shell
JWT_SECRET="32-character long secret used to sign jwt tokens"
JWT_ACCESS_TOKEN_LIFETIME="15"
JWT_REFRESH_TOKEN_LIFETIME="7"

AUTH_APP_EXTERNAL_PORT="5050"
AUTH_DB_EXTERNAL_PORT="5051"
AUTH_DB_INTERNAL_PORT="5052"
AUTH_DB_PASSWORD="postgres"

DISCOVERY_APP_EXTERNAL_PORT="5150"
DISCOVERY_DB_EXTERNAL_PORT="5151"
DISCOVERY_DB_INTERNAL_PORT="5152"
DISCOVERY_DB_PASSWORD="postgres"
```

Now you need to start up all containers using the docker-compose configuration
and environment file you just created:
```
docker-compose --env-file "./docker-compose.env" up --build
```
> **Note**: On first launch, this process can take up to several minutes. Please
be patient.

Now we need to apply migrations to the databases. Connect to the `Auth` database
with psql tool using port and password from the `docker-compose.env` variables
file (`5051` and `postgres` in the example above):
```
Server [localhost]:
Database [postgres]:
Port [5432]: 5051
Username [postgres]:
Password for the user postgres: postgres
```
> **Note**: Password will not be displayed as you type.

Run the `migrations-script.sql` file located in the` Mitekat.Auth` solution
directory:
```
\i 'C:/absolute/path/Mit-e-Kat/Mitekat.Auth/migrations-script.sql'
```
> **Note**: You need to provide an absolute path to the file using only forward
slashes. Also, the path must be enclosed in single quotes.

Now do the same thing, but for the `Discovery` database this time:
```
Server [localhost]:
Database [postgres]:
Port [5432]: 5151
Username [postgres]:
Password for the user postgres: postgres

\i 'C:/absolute/path/Mit-e-Kat/Mitekat.Discovery/migrations-script.sql'
```

Now you can open both applications in the browser (or using any other HTTP
client). Swagger documentation is available at `http://localhost:port/swagger`,
ports are specified in the `docker-compose.env` file and the default values for
them are `5050` and `5150` for the `Auth` and `Discovery` applications
respectively.
