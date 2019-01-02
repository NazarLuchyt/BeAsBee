using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.API.Areas.v1.Models.Message;
using BeAsBee.Domain.Entities;
using BeAsBee.Domain.Interfaces.Services;
using BeAsBee.Domain.Services;
using Microsoft.AspNetCore.SignalR;

namespace BeAsBee.API.Hubs {
    public class ChatHub : Hub {
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;

        public ChatHub ( IMessageService messageService, IMapper mapper ) {
            _messageService = messageService;
            _mapper = mapper;
        }

        // public async Task SendAsync ( MessageDTO messageDTO )
        public async Task SendAsync ( CreateMessageBindingModel message ) {
            var modelEntity = _mapper.Map<MessageEntity>( message );
            var result = await _messageService.CreateAsync( modelEntity );
            if ( !result.IsSuccess ) {
                throw result.Exception;
            }

            var viewModel = _mapper.Map<MessageViewModel>( result.Value );
            await Clients.Group( viewModel.ChatId.ToString() ).SendAsync( "OnSend", Context.ConnectionId, viewModel );

            // await Clients.All.SendAsync( "sendToAll", viewModel );
        }

        public async Task ConnectToChatAsync ( string chatId ) {
            await Groups.AddToGroupAsync( Context.ConnectionId, chatId );
            //var userId = Context.User?.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sid)?.Value; // Get user id from token Sid claim
            //var user = await _unitOfWork.UserManager.FindByIdAsync(userId);
            //if ( user != null ) {
            //    await Groups.AddAsync(Context.ConnectionId, chatId);
            //    //await _cache.Database.ListRightPushAsync("UserChats"+ user.Id, chatId);
            //    //await _cache.Database.ListRightPushAsync("UsersStatus" + user.Id, (int)user.Status);
            //    await _cache.Database.StringSetAsync(Context.ConnectionId, chatId);
            //    await _cache.Database.StringSetAsync($"uc{userId}{chatId}", (int) UserStatusType.Online);
            //    await Clients.Group(chatId).InvokeAsync("OnConnectToChat", Context.ConnectionId, _mapper.Map<UserDTO>(user));
            //    await Clients.Group(chatId).InvokeAsync("OnUserStatusChange", Context.ConnectionId, userId, UserStatusType.Online);
        }
    }
}