using System;
using System.Collections.Generic;
using TellDontAskKata.Main.Domain;
using TellDontAskKata.Main.Repository;
using TellDontAskKata.Main.UseCase;
using TellDontAskKata.Tests.Doubles;
using Xunit;

namespace TellDontAskKata.Tests.UseCase
{
    public class OrderCreationUseCaseTest
    {
        private readonly TestOrderRepository _orderRepository;
        private readonly IProductCatalog _productCatalog;
        private readonly OrderCreationUseCase _useCase;
        private Category _foodCategory;
        private Product _saladProduct;
        private Product _tomatoProduct;

        public OrderCreationUseCaseTest()
        {
            _foodCategory = new Category("food", 10m);
            _saladProduct = new Product("salad", 3.56m, _foodCategory);
            _tomatoProduct = new Product("tomato", 4.65m, _foodCategory);

            _productCatalog = new InMemoryProductCatalog(new List<Product>
            {
                _saladProduct,
                _tomatoProduct
            });

            _orderRepository = new TestOrderRepository();

            _useCase = new OrderCreationUseCase(_orderRepository, _productCatalog);
        }


        [Fact]
        public void SellMultipleItems()
        {
            var saladRequest = new SellItemRequest
            {
                ProductName = "salad",
                Quantity = 2
            };

            var tomatoRequest = new SellItemRequest
            {
                ProductName = "tomato",
                Quantity = 3
            };

            var request = new SellItemsRequest
            {
                Requests = new List<SellItemRequest> { saladRequest, tomatoRequest }
            };

            _useCase.Run(request);

            Order insertedOrder = _orderRepository.GetSavedOrder();
            Assert.Equal(OrderStatus.Created, insertedOrder.Status);
            Assert.Equal(23.20m, insertedOrder.Total);
            Assert.Equal(2.13m, insertedOrder.Tax);
            Assert.Equal("EUR", insertedOrder.Currency);

            Assert.Collection(insertedOrder.Items,
                item => Assert.Equal(new OrderItem(_saladProduct, 2), item),
                item => Assert.Equal(new OrderItem(_tomatoProduct, 3), item));
        }

        [Fact]
        public void UnknownProduct()
        {
            var request = new SellItemsRequest
            {
                Requests = new List<SellItemRequest> { 
                    new SellItemRequest { ProductName = "unknown product"}
                }
            };

            Action actionToTest = () => _useCase.Run(request);

            Assert.Throws<UnknownProductException>(actionToTest);
        }



    }
}
