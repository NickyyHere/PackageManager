using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace PackageManage.API.Controllers;

public static class ControllerExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsFailed)
        {
            return new BadRequestObjectResult(result.Errors);
        }
        return new OkObjectResult(result);
    }
    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsFailed)
        {
            return new BadRequestObjectResult(result.Errors);
        }
        return new OkObjectResult(result);
    }
}