using Microsoft.AspNetCore.Mvc.Filters;
namespace FirstApp.Filters
{
public class CustomResultFilterAttribute : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        // Code to be executed before the action result executes
         Console.WriteLine("Result  Action executing");
    }

    public override void OnResultExecuted(ResultExecutedContext context)
    {
        // Code to be executed after the action result executes
        Console.WriteLine("Result  Action executed");
    }
}
}