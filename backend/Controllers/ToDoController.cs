using backend.Business;
using backend.Exceptions;
using backend.Transfers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoController : ControllerBase
    {
        #region Fields

        private readonly ITodoItemsBusiness _todoItemsBusiness;
        private readonly ILogger<ToDoController> _logger;

        #endregion Fields

        #region Constructors
        public ToDoController(ITodoItemsBusiness todoItemsBusiness, ILogger<ToDoController> logger)
        {
            _todoItemsBusiness = todoItemsBusiness ?? throw new ArgumentNullException(nameof(todoItemsBusiness));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion Constructors

        #region Endpoints

        [HttpGet]
        public async Task<ActionResult<IList<TodoItemResponseTransfer>>> GetItems()
        {
            try
            {
                //Would probably look into pagination if it is expected for there to be a lot of Todo items.
                return Ok(await _todoItemsBusiness.GetAllTodoItems());
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new BadRequestResponseTransfer
                {
                    Message = exception.Message
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unhandled exception occured while retrieving Todo items.");
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemResponseTransfer>> AddTodoItem(TodoItemCreateTransfer newTodoItem)
        {
            try
            {
                return Ok(await _todoItemsBusiness.AddTodoItem(newTodoItem));
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new BadRequestResponseTransfer
                {
                    Message = exception.Message
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unhandled exception occured while adding a new todo item.");
                throw;
            }
        }

        [HttpPut]
        public async Task<ActionResult<TodoItemResponseTransfer>> UpdateTodoItem(TodoItemUpdateTransfer updatedTodoItem)
        {
            try
            {
                return Ok(await _todoItemsBusiness.UpdateTodoItem(updatedTodoItem));
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new BadRequestResponseTransfer
                {
                    Message = exception.Message
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unhandled exception occured while adding a new todo item.");
                throw;
            }
        }

        [HttpDelete]
        [Route("{todoItemId}")]
        public async Task<ActionResult> DeleteTodoItem(int todoItemId)
        {
            try
            {
                await _todoItemsBusiness.DeleteTodoItem(todoItemId);
                return Ok();
            }
            catch (NotFoundException exception)
            {
                return NotFound(exception.Message);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new BadRequestResponseTransfer
                {
                    Message = exception.Message
                });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unhandled exception occured while deleting the todo item.");
                throw;
            }
        }

        #endregion Endpoints
    }
}
