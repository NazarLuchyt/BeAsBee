using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BeAsBee.API.Areas.v1.Models.Message;
using BeAsBee.API.Helpers;
using BeAsBee.Domain.Entities;
using BeAsBee.Domain.Interfaces.Services;
using BeAsBee.Domain.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BeAsBee.API.Hubs {
    [Authorize( AuthenticationSchemes = "Bearer" )]
    public class ChatHub : Hub {
        private readonly IChatService _chatService;
        private readonly IConnectionMapping _connectionMapping;
        private readonly IMapper Mapper;
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public ChatHub ( IMessageService messageService, IMapper mapper, IUserService userService, IChatService chatService, IConnectionMapping connectionMapping ) {
            _messageService = messageService;
            Mapper = mapper;
            _userService = userService;
            _chatService = chatService;
            _connectionMapping = connectionMapping;
        }

        // public async Task SendAsync ( MessageDTO messageDTO )
        public async Task SendAsync ( CreateMessageBindingModel message ) {
            if ( !_connectionMapping.UserInChatExist( message.ChatId.ToString(), Context.UserIdentifier ) ) {
                throw new HubException( "Error!!" );
            }

            var user = await _userService.GetByIdAsync( Guid.Parse( Context.UserIdentifier ) );
            if ( user == null ) {
                await Clients.Group( message.ChatId.ToString() ).SendAsync( "OnSend", Context.ConnectionId, string.Format( Translations.COMMON_ERROR, "Can not found current user!" ) );
            }

            var modelEntity = Mapper.Map<MessageEntity>( message );
            modelEntity.UserId = user?.Id;

            var result = await _messageService.CreateAsync( modelEntity );
            if ( !result.IsSuccess ) {
                throw result.Exception;
            }

            var viewModel = Mapper.Map<MessageViewModel>( await _messageService.GetByIdAsync( result.Value.Id ) );
            await Clients.Group( viewModel.ChatId.ToString() ).SendAsync( "OnSend", Context.ConnectionId, viewModel );
        }

        public async Task CreateNewChat ( string chatId ) {
            var chat = await _chatService.GetByIdAsync( Guid.Parse( chatId ) );
            foreach ( var user in chat.UserChats.Where( user => user.Id != Guid.Parse( Context.UserIdentifier ) ) ) {
                await Clients.User( user.Id.ToString() ).SendAsync( "OnChatCreated", chat );
            }
        }

        public async Task StartChatForNewUsers ( Guid chatId, Guid[] newUserGuids ) {
            var result = await _chatService.AddUsersAsync( chatId, newUserGuids.ToList() );
            if ( !result.IsSuccess ) {
                throw new HubException( result.Exception.Message );
            }

            var chat = await _chatService.GetByIdAsync( chatId );
            foreach ( var newUserGuid in newUserGuids ) {
                await Clients.User( newUserGuid.ToString() ).SendAsync( "OnChatCreated", chat );
            }
        }

        public async Task RemoveUsersFromChat ( Guid chatId, Guid[] removeUserGuids ) {
            var result = await _chatService.RemoveUsersAsync( chatId, removeUserGuids.ToList() );
            if ( !result.IsSuccess ) {
                throw new HubException( result.Exception.Message );
            }

            var chat = await _chatService.GetByIdAsync( chatId );
            IReadOnlyList<string> users = removeUserGuids.Select( id => id.ToString() ).ToArray();
            await Clients.Users( users ).SendAsync( "OnUserKicked", chat, Translations.KICKED_BY_ADMIN );
            foreach ( var user in users ) {
                await Groups.RemoveFromGroupAsync( _connectionMapping.GetConnectionIdFromChatByUserId( chatId.ToString(), user ), chatId.ToString() );
                _connectionMapping.DeleteUserFromChat( chatId.ToString(), user );
            }

            await Clients.Group( chatId.ToString() ).SendAsync( "OnRemoveUsers", chat, result.Value );
        }

        public async Task DisconnectUserFromChat ( string chatId ) {
            _connectionMapping.DeleteUserFromChat( chatId, Context.UserIdentifier );
            await Groups.RemoveFromGroupAsync( Context.ConnectionId, chatId );
        }

        public async Task ConnectToChatAsync ( string chatId ) {
            _connectionMapping.AddUserToChat( chatId, Context.ConnectionId, Context.UserIdentifier );
            // _connections.Add( chatId, Context.ConnectionId );
            await Groups.AddToGroupAsync( Context.ConnectionId, chatId );
        }
    }
}