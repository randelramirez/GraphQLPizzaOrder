*ensure current project target before running in PMC is GraphQLPizzaOrder.Data*
Add-Migration InitializeDatabase 

*not working*
dotnet-ef migrations add InitializeDatabase --project GraphQLPizzaOrder.Data --start-up-project GraphQLPizzaOrder.API --context PizzaOrderContext


query  getNewOrders{
  
  newOrders {
    id
    orderStatus
    addressLine1
    pizzaDetails {
      id
      name
      orderDetailId
      toppings
      price
    }
  }
}

mutation UpdateStatus{
  updateStatus(id: 1, status: InKitchen) {
    id
    orderStatus
  }
}

query getPizzaDetails {
  pizzaDetails(id: 7) {
    id
    name
    toppings
  }
}

query  GetSpecificOrder{
  orderDetails(id: 7){
    addressLine1
  	  id
    mobileNo
    pizzaDetails {
      id
      name
    }
  }

}

mutation NewOrder($orderDetail: OrderDetailInputType!) {
  createOrder(orderDetail: $orderDetail){
    id
    addressLine1
    amount
    mobileNo
    pizzaDetails {
      id
      name
      orderDetailId
      price
      toppings
    }
    date
  }
}

mutation DeletePizzaDetail($id: Int!){
  deletePizzaDetail(id: $id){
    id
    addressLine1
    amount
    date
    pizzaDetails {
      id
      name
      orderDetailId
    }
  
  }
}


#get orders with status = Delivered
query  GetCompletedOrders{
  completedOrders(first: 1, orderBy: {field: AMOUNT direction: ASC} ){
    pageInfo {
      hasNextPage
      hasPreviousPage
      startCursor
      endCursor
    }
    totalCount
    edges {
      cursor
      node {
        id
        addressLine1
        amount
      }
    }
  }
}

mutation Update{
  updateStatus(id: 2 status: Delivered){
    addressLine1
    id
    orderStatus
  }
}









variables
{
  "orderDetail": {
    "addressLine1": "8 Brasilia",
    "mobileNo": "823042",
    "amount": 999,
    "pizzaDetails": [{
      "name": "Gem's",
      "price": 2000,
      "size": 99,
      "toppings": "PEPPERONI"
    }]
  }
}


Subscriptions
subscription ListenOnNewOrdersCreated {
  ordderCreated {
    orderId
  }
}


#OrderStatus updates to InKitchen will be listened (other types of status will not be)
subscription ListenOnOrderStatusUpdateBasedFromParameter {
  statusUpdate(status: InKitchen) {
   orderId
    orderStatus
  }
}




