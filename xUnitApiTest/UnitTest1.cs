using GroceryStoreAPI.Controllers;
using GroceryStoreAPI.Models;
using GroceryStoreAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Xunit;

namespace xUnitApiTest
{
    public class UnitTest1
    {
        CustomersController _controller;
        ICustomerService _service;
        
        public UnitTest1()
        {
            _controller = new CustomersController(_service);

        }

        [Fact]
        public void GetWhenCalled_ReturnsAllItems()
        {
            // Act
            var okResult = _controller.GetCustomers();

            //Assert
            var customers = Assert.IsType<List<Customer>>(okResult);
            Assert.Equal(3, customers.Count);
        }

        [Fact]
        public void GetById_Existing_ReturnRightItem()
        {
            // Arrange
            int id = 2;

            // Act
            var okResult = _controller.GetCustomer(id);

            //Assert
            Assert.IsType<Customer>(okResult);
            Assert.Equal(id, okResult.id);
        }

        [Fact]
        public void Add_ValidObjectPassed_ReturnsCreatedResponse()
        {
            // Arrange
            Customer customer = new Customer()
            {
                id = 4,
                name = "Jenny"
            };

            // Act
            var createdResponse = _controller.PostCustomer(customer);

            // Assert
            Assert.IsType<Customer>(createdResponse);
        }

        [Fact]
        public void Update_ValidObjectPassed_ReturnsOkResponse()
        {
            // Arrange
            Customer customer = new Customer()
            {
                id = 4,
                name = "New Name"
            };

            // Act
            var updatedResponse = _controller.PutCustomer(customer.id, customer);

            // Assert
            Assert.IsType<OkResult>(updatedResponse);
        }

        [Fact]
        public void Delete_ExistingId_DeletesOneItem()
        {
            // Arrange
            int id = 3;

            //Act
            var okResult = _controller.DeleteCustomer(id);

            // Assert
            Assert.IsType<OkResult>(okResult);

        }
    }
}
