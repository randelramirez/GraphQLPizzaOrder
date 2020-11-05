using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using GraphQLPizzaOrder.ConsoleClient.Models;
using GraphQLPizzaOrder.Data.Enum;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQLPizzaOrder.ConsoleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await NewOrdersQuery();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            await CreateOrderMutation();
            Console.ReadKey();
        }

        public async static Task NewOrdersQuery()
        {
            using (var graphQLClient = new GraphQLHttpClient("https://localhost:44379/graphql", new NewtonsoftJsonSerializer()))
            {

                var newOrdersQuery = new GraphQLRequest()
                {
                    Query = @"
                    query {
                        newOrders {
                            id
                            addressLine1
                            addressLine2
                            amount
                        }
                }"
                };

                var newOrderResponse = await graphQLClient.SendQueryAsync<NewOrdersResponse>(newOrdersQuery);
                foreach (var newOrderDetail in newOrderResponse.Data.NewOrders)
                {
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine();
                    Console.WriteLine($"{nameof(NewOrderDetail.Id)}: {newOrderDetail.Id}");
                    Console.WriteLine($"{nameof(NewOrderDetail.AddressLine1)}: {newOrderDetail.AddressLine1}");
                    Console.WriteLine($"{nameof(NewOrderDetail.AddressLine2)}: {newOrderDetail.AddressLine2}");
                    Console.WriteLine($"{nameof(NewOrderDetail.Amount)}: {newOrderDetail.Amount}");
                    Console.WriteLine();
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");

                }
            }
        }

        public async static Task CreateOrderMutation()
        {
            using (var graphQLClient = new GraphQLHttpClient("https://localhost:44379/graphql", new NewtonsoftJsonSerializer()))
            {
                var createOrderMutation = new GraphQLRequest()
                {
                    Query = @"mutation ($order:OrderDetailInputType!) {
                            createOrder(orderDetail:$order) {
                                id
                                orderStatus
                                addressLine1
                                addressLine2
                                pizzaDetails {
                                    id
                                    toppings
                                }
                            }
                        }",
                    Variables = new
                    {
                        order = new
                        {
                            addressLine1 = "ICH" + DateTime.Now,
                            addressLine2 = "Brasilia" + DateTime.Now,
                            mobileNo = "999",
                            amount = 500,
                            pizzaDetails = new[]
                            {
                                new 
                                {
                                    name = "Randel's pizza" + DateTime.Now,
                                    price = 10,
                                    size = 5,
                                    toppings = Toppings.ExtraCheese
                                }
                            }
                        }
                    }
                };

               var response = await graphQLClient.SendMutationAsync<CreateOrderResponse>(createOrderMutation);

                Console.WriteLine();
                Console.WriteLine($"{nameof(CreateOrderDetail.Id)}: {response.Data.CreateOrder.Id}");
                Console.WriteLine($"{nameof(CreateOrderDetail.AddressLine1)}: {response.Data.CreateOrder.AddressLine1}");
                Console.WriteLine($"{nameof(CreateOrderDetail.AddressLine2)}: {response.Data.CreateOrder.AddressLine2}");
                Console.WriteLine($"{nameof(CreateOrderDetail.OrderStatus)}: {response.Data.CreateOrder.OrderStatus}");
                foreach (var item in response.Data.CreateOrder.PizzaDetails)
                {
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");
                    Console.WriteLine();
                    Console.WriteLine($"{nameof(Pizzadetail.Id)}: {item.Id}");
                    Console.WriteLine($"{nameof(Pizzadetail.Toppings)}: {item.Toppings}");
                    Console.WriteLine();
                    Console.WriteLine("~~~~~~~~~~~~~~~~~~~~");
                }
              
            }
        }
    }
}
