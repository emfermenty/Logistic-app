using LogisticService.Application.Services;
using LogisticService.Domain.Models.Shipping.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace LogisticService.Domain.Observer;

public class DatabaseObserver : IShippingObserver
{
    private readonly IServiceScopeFactory _scopeFactory;

    public DatabaseObserver(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task OnShippingStatusChanged(Shipping shipping, string oldStatus, string newStatus)
    {
        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IShippingsRepository>();

        await repository.ChangeStatusAsync(shipping, newStatus);
        Console.WriteLine($"📝 DB Log: {shipping.TrackingNumber} {oldStatus} TO {newStatus}");
    }

    public async Task OnShippingCreated(Shipping shipping)
    {
        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IShippingsRepository>();

        Console.WriteLine($"📝 DB Log: New shipping {shipping.TrackingNumber} created");
    }

    public async Task OnShippingCompleted(Shipping shipping)
    {
        using var scope = _scopeFactory.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<IShippingsRepository>();

        Console.WriteLine($"📝 DB Log: Shipping {shipping.TrackingNumber} completed");
    }
}