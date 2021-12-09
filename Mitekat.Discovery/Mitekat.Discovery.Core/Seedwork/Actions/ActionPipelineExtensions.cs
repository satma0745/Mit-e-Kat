namespace Mitekat.Discovery.Core.Seedwork.Actions;

using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mitekat.Discovery.Core.Seedwork.Features;

internal static class ActionPipelineExtensions
{
    public static Task<TResponse> SendAsync<TResponse>(
        this IMediator mediator,
        IRequest<TResponse> request,
        CancellationToken cancellationToken) =>
        mediator.Send(request, cancellationToken);

    public static Task<TResponse> SendAsync<TResponse>(
        this IMediator mediator,
        Func<IRequest<TResponse>> requestFactory,
        CancellationToken cancellationToken)
    {
        var request = requestFactory();
        return mediator.SendAsync(request, cancellationToken);
    }

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

    public static Task<IActionResult> ToActionResult<TResource>(
        this Task<Response<TResource>> responseTask,
        Func<TResource, IActionResult> onSuccess,
        Func<IActionResult> onFailure) =>
        responseTask.ToActionResult(onSuccess, _ => onFailure());

    public static Task<IActionResult> ToActionResult(
        this Task<Response<Unit>> responseTask,
        Func<IActionResult> onSuccess,
        Func<Error, IActionResult> onFailure) =>
        responseTask.ToActionResult(_ => onSuccess(), onFailure);

    public static Task<IActionResult> ToActionResult(
        this Task<Response<Unit>> responseTask,
        Func<IActionResult> onSuccess,
        Func<IActionResult> onFailure) =>
        responseTask.ToActionResult(onSuccess, _ => onFailure());
}
