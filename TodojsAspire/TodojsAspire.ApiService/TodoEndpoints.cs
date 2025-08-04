using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TodojsAspire.ApiService.Models;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/Todo").WithTags(nameof(Todo));

        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Todo.OrderBy(t=>t.PriorityPosition).ToListAsync();
        })
        .WithName("GetAllTodos")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Todo>, NotFound>> (int id, AppDbContext db) =>
        {
            return await db.Todo.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is Todo model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetTodoById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, Todo todo, AppDbContext db) =>
        {
            var affected = await db.Todo
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                .SetProperty(m => m.Title, todo.Title)
                .SetProperty(m => m.IsComplete, todo.IsComplete)
                .SetProperty(m => m.PriorityPosition, todo.PriorityPosition)
        );

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateTodo")
        .WithOpenApi();

        group.MapPost("/", async (Todo todo, AppDbContext db) =>
        {
            if(todo.PriorityPosition <=0)
            {
                todo.PriorityPosition = await db.Todo.AnyAsync()
                    ? await db.Todo.MaxAsync(t => t.PriorityPosition) + 1 
                    : 1;
            }

            db.Todo.Add(todo);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Todo/{todo.Id}",todo);
        })
        .WithName("CreateTodo")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, AppDbContext db) =>
        {
            var affected = await db.Todo
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();

            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteTodo")
        .WithOpenApi();

        group.MapPost("/move-up/{id:int}", async Task<Results<Ok, NotFound>>(int id,AppDbContext db)=>{
            var todo = await db.Todo.FirstOrDefaultAsync(t=>t.Id == id);
            if(todo == null)
                return TypedResults.NotFound();

            // Find the todo with the largest position less than the current one
            var prevTodo = await db.Todo.Where(t=>t.PriorityPosition < todo.PriorityPosition)
                                   .OrderByDescending(t=>t.PriorityPosition).FirstOrDefaultAsync();

            if (prevTodo == null)
                return TypedResults.Ok();

            //swap positions
            (todo.PriorityPosition, prevTodo.PriorityPosition) = (prevTodo.PriorityPosition, todo.PriorityPosition);
            await db.SaveChangesAsync();
            return TypedResults.Ok();
        }).WithName("MoveTaskUp");

        group.MapPost("/move-down/{id:int}", async Task<Results<Ok, NotFound>> (int id, AppDbContext db) =>
        {
            var todo = await db.Todo.FirstOrDefaultAsync(t=> t.Id == id);
            if(todo == null)
                return TypedResults.NotFound();

            // Find the todo with smallest position greater than the current todo
            var nextTodo = await db.Todo
                        .Where(t=>t.PriorityPosition > todo.PriorityPosition)
                        .OrderBy(t=>t.PriorityPosition)
                        .FirstOrDefaultAsync();
            if(nextTodo == null)
                return TypedResults.Ok(); // Already at the bottom or no next todo

            // swap positions
            (todo.PriorityPosition, nextTodo.PriorityPosition) = (nextTodo.PriorityPosition, todo.PriorityPosition);
            await db.SaveChangesAsync();
            return TypedResults.Ok();

        }).WithName("MoveTaskDown");
    }
}
