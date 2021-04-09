using backend.Contexts;
using backend.Exceptions;
using backend.Models;
using backend.Transfers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Business
{
    public interface ITodoItemsBusiness
    {
        Task<IList<TodoItemResponseTransfer>> GetAllTodoItems();
        Task<TodoItemResponseTransfer> AddTodoItem(TodoItemCreateTransfer newTodoItem);
        Task<TodoItemResponseTransfer> UpdateTodoItem(TodoItemUpdateTransfer updatedTodoItem);
        Task DeleteTodoItem(int todoItemId);
    }

    public class TodoItemsBusiness : ITodoItemsBusiness
    {
        #region Constructors

        public TodoItemsBusiness(TodoContext todoContext)
        {
            _todoContext = todoContext ?? throw new ArgumentNullException(nameof(todoContext));
        }

        #endregion Constructors

        #region Fields

        private readonly TodoContext _todoContext;

        #endregion Fields

        #region ITodoItemsBusiness

        public async Task<IList<TodoItemResponseTransfer>> GetAllTodoItems()
        {
            return (await _todoContext.TodoItems.ToListAsync())
                .Select(MapTodoToTransfer)
                .ToList();
        }

        public async Task<TodoItemResponseTransfer> AddTodoItem(TodoItemCreateTransfer newTodoItem)
        {
            ValidateTodoItem(newTodoItem);

            var valueTask = await _todoContext.TodoItems.AddAsync(new TodoItem
            {
                Title = newTodoItem.Title,
                Description = newTodoItem.Description
            });

            if (await _todoContext.SaveChangesAsync() != 1)
            {
                throw new Exception("The new Todo was not saved. Please contact support for assistance.");
            }

            return MapTodoToTransfer(valueTask.Entity);
        }

        public async Task<TodoItemResponseTransfer> UpdateTodoItem(TodoItemUpdateTransfer updatedTodoItem)
        {
            ValidateTodoItem(updatedTodoItem);

            var itemToUpdate = await _todoContext.TodoItems.FindAsync(updatedTodoItem.Id);
            if (itemToUpdate is null)
            {
                throw new NotFoundException($"No Todo with Id {updatedTodoItem.Id}.");
            }

            itemToUpdate.Title = updatedTodoItem.Title;
            itemToUpdate.Description = updatedTodoItem.Description;

            if (await _todoContext.SaveChangesAsync() != 1)
            {
                throw new Exception("The new Todo was not saved. Please contact support for assistance.");
            }

            return MapTodoToTransfer(itemToUpdate);
        }

        public async Task DeleteTodoItem(int todoItemId)
        {
            var itemToDelete = await _todoContext.TodoItems.FindAsync(todoItemId);
            if (itemToDelete is null)
            {
                throw new NotFoundException($"No Todo with Id {todoItemId}.");
            }

            _todoContext.TodoItems.Remove(itemToDelete);
            await _todoContext.SaveChangesAsync();
        }

        #endregion ITodoItemsBusiness

        #region Private Methods

        private void ValidateTodoItem(TodoItemCreateTransfer todoItem)
        {
            if (todoItem is null)
            {
                throw new ArgumentException("Todo was not provided.");
            }

            if (string.IsNullOrEmpty(todoItem.Title))
            {
                throw new ArgumentException("Todo must have a title.");
            }
        }

        private TodoItemResponseTransfer MapTodoToTransfer(TodoItem todoItem)
        {
            return new TodoItemResponseTransfer
            {
                CreatedDate = todoItem.CreatedDate,
                Id = todoItem.Id,
                Description = todoItem.Description,
                Title = todoItem.Title
            };
        }

        #endregion Private Methods
    }
}
