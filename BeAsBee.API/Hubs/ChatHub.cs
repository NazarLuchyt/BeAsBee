using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.API.Areas.v1.Models.Message;
using BeAsBee.Domain.Entities;
using BeAsBee.Domain.Interfaces.Services;
using BeAsBee.Domain.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BeAsBee.API.Hubs {
    [Authorize( AuthenticationSchemes = "Bearer" )]
    public class ChatHub : Hub {
        private readonly IChatService _chatService;
        private readonly IMapper _mapper;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public ChatHub ( IMessageService messageService, IMapper mapper, IUserService userService, IChatService chatService ) {
            _messageService = messageService;
            _mapper = mapper;
            _userService = userService;
            _chatService = chatService;
        }

        // public async Task SendAsync ( MessageDTO messageDTO )
        public async Task SendAsync ( CreateMessageBindingModel message ) {
            var user = await _userService.GetByIdAsync( Guid.Parse( Context.UserIdentifier ) );
            if ( user == null ) {
                await Clients.Group( message.ChatId.ToString() ).SendAsync( "OnSend", Context.ConnectionId, string.Format( Translations.COMMON_ERROR, "Can not found current user!" ) );
            }

            var modelEntity = _mapper.Map<MessageEntity>( message );
            modelEntity.UserId = user?.Id;

            var result = await _messageService.CreateAsync( modelEntity );
            if ( !result.IsSuccess ) {
                throw result.Exception;
            }

            var viewModel = _mapper.Map<MessageViewModel>( await _messageService.GetByIdAsync( result.Value.Id ) );
            await Clients.Group( viewModel.ChatId.ToString() ).SendAsync( "OnSend", Context.ConnectionId, viewModel );
        }

        public async Task CreateNewChat ( string chatId ) {
            var chat = await _chatService.GetByIdAsync( Guid.Parse( chatId ) );
            foreach ( var user in chat.UserChats.Where( user => user.Id != Guid.Parse( Context.UserIdentifier ) ) ) {
                await Clients.User( user.Id.ToString() ).SendAsync( "OnChatCreated", chat );
            }
        }

        public async Task ConnectToChatAsync ( string chatId ) {
            await Groups.AddToGroupAsync( Context.ConnectionId, chatId );
            // await Clients.User(  ).SendAsync( "OnSend", Context.ConnectionId, viewModel );

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