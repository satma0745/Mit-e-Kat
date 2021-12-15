namespace Mitekat.Seedwork.Features.Actions;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Seedwork.Features.Requesting;

public static class ActionPipelineExtensions
{
    public static Task<TResponse> SendAsync<TResponse>(
        this IMediator mediator,
        IRequest<TResponse> request,
        CancellationToken cancellationToken) =>
        mediator.Send(request, cancellationToken);

    public static async Task<IActionResult> ToActionResult<TResource>(
        this Task<Response<TResource>> responseTask,
        Func<TResource, IActionResult> onSuccess,
        Func<Error, IActionResult> onFailure)
    {
        var response = await responseTask;
        
        return response.IsSuccess
            ? onSuccess(response.Resource)
            : onFailure(response.Error);
    }
}
