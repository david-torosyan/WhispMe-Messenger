using Microsoft.AspNetCore.SignalR.Client;

namespace WhispMe.WEB.Services;

public class SignalRService
{
    private readonly HubConnection _hubConnection;

    public SignalRService()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7001/chathub")
            .Build();
    }

    //_hubConnection.On<string>("ReceiveMessage", async (msg) =>
    //{
    //    Console.WriteLine($"Received Message: {msg}");
    //});

    //_hubConnection.Closed += async (error) =>
    //{
    //    Console.WriteLine($"SignalR connection closed: {error?.Message}");
    //    await _hubConnection.StartAsync();
    //};

    //try
    //{
    //    await _hubConnection.StartAsync();
    //    Console.WriteLine("SignalR connection started");
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine($"SignalR connection error: {ex.Message}");
    //}

    public async Task StartAsync()
    {
        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine("SignalR connection started");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SignalR connection error: {ex.Message}");
        }
    }

    public async Task SendMessageAsync(string user, string message)
    {
        await _hubConnection.SendAsync("SendMessage", user, message);
    }

    public string OnMessageReceived()
    {
        string message = "msg";

        _hubConnection.On<string>("ReceiveMessage", async (msg) =>
        {
            Console.WriteLine($"Received Message: {msg}");
            message =  msg;
        });

        return message;
    }
}
