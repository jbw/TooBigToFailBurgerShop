## Building migrations

Current solution could be better and not have the application cater for database migrations as much. 
Maybe migrations can be manually handled or by another tool. 

### Creating a migration

Using dotnet ef when Ordering.API is the startup project

```sh
cd Ordering.Infrastructure
dotnet ef --startup-project ../Ordering.API/ migrations add MyMigration  
```

This will create migrations in the Ordering.Infrastructure project.

### Applying migrations

#### Orders
```sh
cd Ordering.Infrastructure
dotnet ef database update
```

#### Saga State
```sh
cd Ordering.StateService
dotnet ef database update

```

#### Order events

* Create a blank `order_events` database and run `event_store.sql` against it.
