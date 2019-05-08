using System;
using System.Collections.Generic;

namespace BeAsBee.API.Helpers {
    public class ConnectionMapping : IConnectionMapping {
        private readonly Dictionary<string, Dictionary<string, string>> _connections = new Dictionary<string, Dictionary<string, string>>();

        public void AddUserToChat ( string chatId, string connectionId, string userId ) {
            try {
                if ( _connections.ContainsKey( chatId ) ) {
                    if ( !_connections[chatId].ContainsKey( userId ) ) {
                        _connections[chatId].Add( userId, connectionId );
                    } else {
                        _connections[chatId][userId] = connectionId;
                    }
                } else {
                    _connections.Add( chatId, new Dictionary<string, string>() );
                    _connections[chatId].Add( userId, connectionId );
                }
            } catch ( Exception ex ) {
                var eaa = ex;
            }
        }

        public void DeleteUserFromChat ( string chatId, string userId ) {
            _connections[chatId].Remove( userId );
        }

        public bool UserInChatExist ( string chatId, string userId ) {
            return _connections[chatId].ContainsKey( userId );
        }

        public string GetConnectionIdFromChatByUserId ( string chatId, string userId ) {
            if ( _connections.ContainsKey( chatId ) ) {
                if ( _connections[chatId].ContainsKey( userId ) ) {
                    return _connections[chatId][userId];
                }
            }
            throw new Exception( "Incorrect data!" );
        }

        public void AddChat ( string chatId ) {
            _connections.Add( chatId, new Dictionary<string, string>() );
        }
    }

    public interface IConnectionMapping {
        // void AddChat ( string chatId );
        void AddUserToChat ( string chatId, string connectionId, string userId );

        //   bool ChatExist ( string chatId );
        bool UserInChatExist ( string chatId, string userId );
        void DeleteUserFromChat ( string chatId, string userId );
        string GetConnectionIdFromChatByUserId ( string chatId, string userId );
    }
}