using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using WhispMe.BLL.Interfaces;
using WhispMe.DTO.DTOs;

namespace WhispMe.API.Hubs;

public class ChatHub : Hub
{
    public readonly static List<UserDto> _Connections = new();
    private readonly static Dictionary<string, string> _ConnectionsMap = [];

    private readonly IUserService _userService;

    public ChatHub(IUserService userService)
    {
        _userService = userService;
    }

    public async Task SendPrivate(string receiverName, string message)
    {
        if (_ConnectionsMap.TryGetValue(receiverName, out string userId))
        {
            // Who is the sender;
            var sender = _Connections.Where(u => u.UserName == IdentityName).First();

            if (!string.IsNullOrEmpty(message.Trim()))
            {
                // Build the message
                var messageViewModel = new MessageDto()
                {
                    Content = Regex.Replace(message, @"<.*?>", string.Empty),
                    SenderEmail = sender.UserName,
                    SenderFullName = sender.FullName,
                    Avatar = sender.Avatar,
                    Room = "",
                    Timestamp = DateTime.Now
                };

                // Send the message
                await Clients.Client(userId).SendAsync("newMessage", messageViewModel);
                await Clients.Caller.SendAsync("newMessage", messageViewModel);
            }
        }
    }

    public async Task Join(string roomName)
    {
        try
        {
            var user = _Connections.Where(u => u.UserName == IdentityName).FirstOrDefault();
            if (user != null && user.CurrentRoom != roomName)
            {
                // Remove user from others list
                if (!string.IsNullOrEmpty(user.CurrentRoom))
                    await Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

                // Join to new chat room
                await Leave(user.CurrentRoom);
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
                user.CurrentRoom = roomName;

                // Tell others to update their list of users
                await Clients.OthersInGroup(roomName).SendAsync("addUser", user);
            }
        }
        catch (Exception ex)
        {
            await Clients.Caller.SendAsync("onError", "You failed to join the chat room!" + ex.Message);
        }
    }

    public async Task Leave(string roomName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
    }

    public IEnumerable<UserDto> GetUsers(string roomName)
    {
        return _Connections.Where(u => u.CurrentRoom == roomName).ToList();
    }

    public override async Task OnConnectedAsync()
    {
        try
        {
            var userViewModel = await _userService.GetUserByIdentityName(IdentityName);
            userViewModel.Device = GetDevice();
            userViewModel.CurrentRoom = "";

            if (!_Connections.Any(u => u.UserName == IdentityName))
            {
                _Connections.Add(userViewModel);
                _ConnectionsMap.Add(IdentityName, Context.ConnectionId);
            }

            Clients.Caller.SendAsync("getProfileInfo", userViewModel);
        }
        catch (Exception ex)
        {
            Clients.Caller.SendAsync("onError", "OnConnected:" + ex.Message);
        }

        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        try
        {
            var user = _Connections.Where(u => u.UserName == IdentityName).First();
            _Connections.Remove(user);

            // Tell other users to remove you from their list
            Clients.OthersInGroup(user.CurrentRoom).SendAsync("removeUser", user);

            // Remove mapping
            _ConnectionsMap.Remove(user.UserName);
        }
        catch (Exception ex)
        {
            Clients.Caller.SendAsync("onError", "OnDisconnected: " + ex.Message);
        }

        return base.OnDisconnectedAsync(exception);
    }

    private string IdentityName
    {
        get { return Context.User.Identity.Name; }
    }

    private string GetDevice()
    {
        var device = Context.GetHttpContext().Request.Headers["Device"].ToString();
        if (!string.IsNullOrEmpty(device) && (device.Equals("Desktop") || device.Equals("Mobile")))
            return device;

        return "Web";
    }
}
