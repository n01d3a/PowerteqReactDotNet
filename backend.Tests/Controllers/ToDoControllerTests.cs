using backend.Business;
using backend.Controllers;
using backend.Exceptions;
using backend.Transfers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace backend.Tests.Controllers
{
    [TestClass]
    public class ToDoControllerTests
    {
        private Mock<ITodoItemsBusiness> _mockTodoItemsBusiness;
        private Mock<ILogger<ToDoController>> _mockLogger;
        private ToDoController _testTarget;

        [TestInitialize]
        public void Setup()
        {
            _mockTodoItemsBusiness = new Mock<ITodoItemsBusiness>();
            _mockLogger = new Mock<ILogger<ToDoController>>();

            _testTarget = new ToDoController(_mockTodoItemsBusiness.Object, _mockLogger.Object);
        }

        [TestMethod]
        public async Task AddTodoItem_Should_Return_BadRequest()
        {
            const string errorMessage = "Test error message";
            _mockTodoItemsBusiness.Setup(business => business.AddTodoItem(It.IsAny<TodoItemCreateTransfer>())).Throws(new ArgumentException(errorMessage));

            var todoItem = new TodoItemCreateTransfer
            {
                Description = "Description",
                Title = "Title"
            };

            var response = await _testTarget.AddTodoItem(todoItem);

            _mockTodoItemsBusiness.Verify(business => business.AddTodoItem(It.IsAny<TodoItemCreateTransfer>()));

            response.Result.Should().BeOfType(typeof(BadRequestObjectResult));

            var result = (ObjectResult)(response.Result);
            result.StatusCode.Should().Be(400);
            var errorObject = (BadRequestResponseTransfer)(result.Value);
            errorObject.Message.Should().Be(errorMessage);
        }

        [TestMethod]
        public void AddTodoItem_Should_Throw_Original_Exceptioin()
        {
            const string errorMessage = "Test error message";
            _mockTodoItemsBusiness.Setup(business => business.AddTodoItem(It.IsAny<TodoItemCreateTransfer>())).Throws(new Exception(errorMessage));

            var todoItem = new TodoItemCreateTransfer
            {
                Description = "Description",
                Title = "Title"
            };

            Func<Task> action = async () => await _testTarget.AddTodoItem(todoItem);

            action.Should().Throw<Exception>().WithMessage(errorMessage);
        }

        [TestMethod]
        public async Task AddTodoItem_Should_Return_OK()
        {
            DateTime createdDate = DateTime.Now;
            const string description = "Test description";
            const int id = 1;
            const string title = "TestTitle";

            _mockTodoItemsBusiness.Setup(business => business.AddTodoItem(It.IsAny<TodoItemCreateTransfer>())).Returns(Task.FromResult(new TodoItemResponseTransfer
            {
                CreatedDate = createdDate,
                Description = description,
                Id = id,
                Title = title
            }));

            var todoItem = new TodoItemCreateTransfer
            {
                Description = description,
                Title = title
            };

            var response = await _testTarget.AddTodoItem(todoItem);

            _mockTodoItemsBusiness.Verify(business => business.AddTodoItem(It.IsAny<TodoItemCreateTransfer>()));

            response.Result.Should().BeOfType(typeof(OkObjectResult));

            var returnedTodoItem = (TodoItemResponseTransfer)((ObjectResult)(response.Result)).Value;
            returnedTodoItem.CreatedDate.Should().Be(createdDate);
            returnedTodoItem.Description.Should().Be(description);
            returnedTodoItem.Id.Should().Be(id);
            returnedTodoItem.Title.Should().Be(title);
        }

        [TestMethod]
        public async Task UpdateTodoItem_Should_Return_BasRequest()
        {
            const string errorMessage = "Test error message";
            _mockTodoItemsBusiness.Setup(business => business.UpdateTodoItem(It.IsAny<TodoItemUpdateTransfer>())).Throws(new ArgumentException(errorMessage));

            var todoItem = new TodoItemUpdateTransfer
            {
                Description = "Description",
                Title = "Title"
            };

            var response = await _testTarget.UpdateTodoItem(todoItem);

            _mockTodoItemsBusiness.Verify(business => business.UpdateTodoItem(It.IsAny<TodoItemUpdateTransfer>()));

            response.Result.Should().BeOfType(typeof(BadRequestObjectResult));

            var result = (ObjectResult)(response.Result);
            result.StatusCode.Should().Be(400);
            var errorObject = (BadRequestResponseTransfer)(result.Value);
            errorObject.Message.Should().Be(errorMessage);
        }

        [TestMethod]
        public async Task UpdateTodoItem_Should_Return_NotFound()
        {
            const string errorMessage = "Test error message";
            _mockTodoItemsBusiness.Setup(business => business.UpdateTodoItem(It.IsAny<TodoItemUpdateTransfer>())).Throws(new NotFoundException(errorMessage));

            var todoItem = new TodoItemUpdateTransfer
            {
                Description = "Description",
                Title = "Title"
            };

            var response = await _testTarget.UpdateTodoItem(todoItem);

            _mockTodoItemsBusiness.Verify(business => business.UpdateTodoItem(It.IsAny<TodoItemUpdateTransfer>()));

            response.Result.Should().BeOfType(typeof(NotFoundObjectResult));

            var result = (ObjectResult)(response.Result);
            result.StatusCode.Should().Be(404);
            result.Value.Should().Be(errorMessage);
        }

        [TestMethod]
        public void UpdateTodoItem_Should_Throw_Original_Exceptioin()
        {
            const string errorMessage = "Test error message";
            _mockTodoItemsBusiness.Setup(business => business.UpdateTodoItem(It.IsAny<TodoItemUpdateTransfer>())).Throws(new Exception(errorMessage));

            var todoItem = new TodoItemUpdateTransfer
            {
                Description = "Description",
                Title = "Title"
            };

            Func<Task> action = async () => await _testTarget.UpdateTodoItem(todoItem);

            action.Should().Throw<Exception>().WithMessage(errorMessage);
        }

        [TestMethod]
        public async Task UpdateTodoItem_Should_Return_OK()
        {
            DateTime createdDate = DateTime.Now;
            const string description = "Test description";
            const int id = 1;
            const string title = "TestTitle";

            _mockTodoItemsBusiness.Setup(business => business.AddTodoItem(It.IsAny<TodoItemUpdateTransfer>())).Returns(Task.FromResult(new TodoItemResponseTransfer
            {
                CreatedDate = createdDate,
                Description = description,
                Id = id,
                Title = title
            }));

            var todoItem = new TodoItemUpdateTransfer
            {
                Id = id,
                Description = description,
                Title = title
            };

            var response = await _testTarget.AddTodoItem(todoItem);

            _mockTodoItemsBusiness.Verify(business => business.AddTodoItem(It.IsAny<TodoItemUpdateTransfer>()));

            response.Result.Should().BeOfType(typeof(OkObjectResult));

            var returnedTodoItem = (TodoItemResponseTransfer)((ObjectResult)(response.Result)).Value;
            returnedTodoItem.CreatedDate.Should().Be(createdDate);
            returnedTodoItem.Description.Should().Be(description);
            returnedTodoItem.Id.Should().Be(id);
            returnedTodoItem.Title.Should().Be(title);
        }

        [TestMethod]
        public async Task DeleteTodoItem_Should_Return_BadRequest()
        {
            const string errorMessage = "Test error message";
            _mockTodoItemsBusiness.Setup(business => business.DeleteTodoItem(It.IsAny<int>())).Throws(new ArgumentException(errorMessage));

            var response = await _testTarget.DeleteTodoItem(1);

            _mockTodoItemsBusiness.Verify(business => business.DeleteTodoItem(It.IsAny<int>()));

            response.Should().BeOfType(typeof(BadRequestObjectResult));

            var result = (BadRequestObjectResult)response;
            result.StatusCode.Should().Be(400);
            var errorObject = (BadRequestResponseTransfer)(result.Value);
            errorObject.Message.Should().Be(errorMessage);
        }

        [TestMethod]
        public async Task DeleteTodoItem_Should_Return_NotFound()
        {
            const string errorMessage = "Test error message";
            _mockTodoItemsBusiness.Setup(business => business.DeleteTodoItem(It.IsAny<int>())).Throws(new NotFoundException(errorMessage));

            var response = await _testTarget.DeleteTodoItem(1);

            _mockTodoItemsBusiness.Verify(business => business.DeleteTodoItem(It.IsAny<int>()));

            response.Should().BeOfType(typeof(NotFoundObjectResult));

            var result = (NotFoundObjectResult)response;
            result.StatusCode.Should().Be(404);
            result.Value.Should().Be(errorMessage);
        }

        [TestMethod]
        public async Task DeleteTodoItem_Should_Return_OK()
        {
            _mockTodoItemsBusiness.Setup(business => business.DeleteTodoItem(It.IsAny<int>())).Returns(Task.CompletedTask);

            var response = await _testTarget.DeleteTodoItem(1);

            _mockTodoItemsBusiness.Verify(business => business.DeleteTodoItem(It.IsAny<int>()));

            response.Should().BeOfType(typeof(OkResult));

            var result = (OkResult)response;
            result.StatusCode.Should().Be(200);
        }
    }
}
