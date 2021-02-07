## Building migrations

Current solution could be better and not have the application cater for database migrations as much. 
Maybe migrations can be manually handled or by another tool. 

### Creating a migration

Using dotnet ef when Ordering.API is the startup project

```sh
cd Ordering.Infrastructure
dotnet ef --startup-project ../Ordering.API/ add migrations MyMigration  
```

This will create migrations in the Ordering.Infrastructure project.

### Applying migrations

```
cd Ordering.Infrastructure
dotnet ef database update
```