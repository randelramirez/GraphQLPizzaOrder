*ensure current project target before running in PMC is GraphQLPizzaOrder.Data*
Add-Migration InitializeDatabase 

*not working*
dotnet-ef migrations add InitializeDatabase --project GraphQLPizzaOrder.Data --start-up-project GraphQLPizzaOrder.API --context PizzaOrderContext