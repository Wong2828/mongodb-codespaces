using StudentAPI.Models;
using StudentAPI.Services;

namespace StudentAPI.Endpoints;

public static class StudentEndpoints
{
    public static IEndpointRouteBuilder MapStudents(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/students");

        group.MapGet("/", async (StudentService service) =>
        {
            var list = await service.GetAsync();
            return Results.Ok(list);
        });

        group.MapGet("/{id}", async (StudentService service, string id) =>
        {
            var s = await service.GetAsync(id);
            return s is null ? Results.NotFound() : Results.Ok(s);
        });

        group.MapPost("/", async (StudentService service, Student s) =>
        {
            await service.CreateAsync(s);
            return Results.Created($"/students/{s.Id}", s);
        });

        group.MapPut("/", async (StudentService service, Student updated, string id) =>
        {
            var existing = await service.GetAsync(id);
            if (existing is null) return Results.NotFound();

            updated.Id = existing.Id;
            await service.UpdateAsync(id, updated);
            return Results.NoContent();
        });

        group.MapDelete("/", async (StudentService service, string id) =>
        {
            var existing = await service.GetAsync(id);
            if (existing is null) return Results.NotFound();

            await service.RemoveAsync(id);
            return Results.NoContent();
        });

        group.MapPost("/bulk", async (StudentService service, List<Student> students) =>
        {
            if (students == null || students.Count == 0)
                return Results.BadRequest("Provide a non-empty list of students.");

            await service.CreateManyAsync(students);
            return Results.Created("/students/bulk", students);
        });

        group.MapGet("/lastname/{lastName}", async (StudentService service, string lastName) =>
        {
            var matches = await service.GetByLastNameAsync(lastName);
            return Results.Ok(matches);
        });

        group.MapGet("/younger-than", async (StudentService service, int age) =>
        {
            var matches = await service.GetYoungerThanAsync(age);
            return Results.Ok(matches);
        });

        return routes;
    }
}
