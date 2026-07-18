using Microsoft.AspNetCore.Mvc;

namespace ModularMonolith.BuildingBlocks.Errors;

public static class ControllerExtensions
{
    public static ActionResult<T> ToActionResult<T>(this ControllerBase controller, Result<T> result)
    {
        if (result.IsSuccess) return controller.StatusCode(result.StatusCode, result.Value);
        return controller.Problem(title: result.Error!.Code, detail: result.Error.Message, statusCode: result.StatusCode);
    }
}
