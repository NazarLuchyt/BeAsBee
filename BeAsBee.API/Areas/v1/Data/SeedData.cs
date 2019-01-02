using System;
using System.Collections.Generic;
using System.Linq;
using BeAsBee.Infrastructure.Sql.Models;
using BeAsBee.Infrastructure.Sql.Models.Context;

namespace BeAsBee.API.Areas.v1.Data {
    public static class SeedData {
        public static void Initialize ( ApplicationContext db ) {
            db.Database.EnsureCreated();

            if ( db.Users.Any() ) {
                return;
            }

            #region Users

            var user1 = new User {
                Id = Guid.NewGuid(),
                Email = "test1",
                Password = "test1",
                FirstName = "Павло",
                SecondName = "Дідик"
            };
            var user2 = new User {
                Id = Guid.NewGuid(),
                Email = "test2",
                Password = "test2",
                FirstName = "Андрій",
                SecondName = "Лопатін"
            };
            var user3 = new User {
                Id = Guid.NewGuid(),
                Email = "test3",
                Password = "test3",
                FirstName = "Сергій",
                SecondName = "Скорилко"
            };
            var user4 = new User {
                Id = Guid.NewGuid(),
                Email = "test4",
                Password = "test4",
                FirstName = "Назар",
                SecondName = "Лучит"
            };

            db.Users.Add( user1 );
            db.Users.Add( user2 );
            db.Users.Add( user3 );
            db.Users.Add( user4 );

            #endregion

            #region Chats

            var chat1 = new Chat {
                Id = Guid.NewGuid(),
                UserId = user1.Id,
                Name = "Група КН - 321",
                Messages = new List<Message> {
                    new Message {
                        Id = Guid.NewGuid(),
                        MessageText = "Привіт, коли потрібно на пари?",
                        UserName = user1.FirstName + " " + user1.SecondName,
                        ReceivedTime = new DateTimeOffset( new DateTime( 2010, 10, 10 ) ).AddDays( 2 ),
                        UserId = user1.Id
                    },
                    new Message {
                        Id = Guid.NewGuid(),
                        MessageText = "Привіт завтра на 12!",
                        UserName = user2.FirstName + " " + user2.SecondName,
                        ReceivedTime = new DateTimeOffset( new DateTime( 2010, 10, 10 ) ).AddDays( 2 ),
                        UserId = user2.Id
                    },
                    new Message {
                        Id = Guid.NewGuid(),
                        MessageText = "Дякую, до завтра!",
                        UserName = user3.FirstName + " " + user3.SecondName,
                        ReceivedTime = new DateTimeOffset( new DateTime( 2010, 10, 10 ) ).AddDays( 32 ),
                        UserId = user3.Id
                    }
                }
            };
            var chat2 = new Chat {
                Id = Guid.NewGuid(),
                UserId = user1.Id,
                Name = user2.FirstName + " " + user2.SecondName,
                Messages = new List<Message> {
                    new Message {
                        Id = Guid.NewGuid(),
                        MessageText = "Привіт, ідеш гуляти?",
                        UserName = user1.FirstName + " " + user1.SecondName,
                        ReceivedTime = new DateTimeOffset( new DateTime( 2010, 10, 10 ) ).AddDays( 52 ),
                        UserId = user1.Id
                    },
                    new Message {
                        Id = Guid.NewGuid(),
                        UserName = user2.FirstName + " " + user2.SecondName,
                        MessageText = "Так звичайно. Пішли!",
                        ReceivedTime = new DateTimeOffset( new DateTime( 2010, 10, 10 ) ).AddDays( 2 ),
                        UserId = user2.Id
                    }
                }
            };
            db.Chats.Add( chat1 );
            db.Chats.Add( chat2 );

            #endregion

            #region Initialize many to many (not all)

            #region Chat1

            chat1.UserChats.Add( new UserChat {ChatId = chat1.Id, UserId = user1.Id} );
            chat1.UserChats.Add( new UserChat {ChatId = chat1.Id, UserId = user3.Id} );
            chat1.UserChats.Add( new UserChat {ChatId = chat1.Id, UserId = user2.Id} );

            #endregion

            #region Chat2

            chat2.UserChats.Add( new UserChat {ChatId = chat2.Id, UserId = user1.Id} );
            chat2.UserChats.Add( new UserChat {ChatId = chat2.Id, UserId = user2.Id} );

            #endregion

            db.SaveChanges();

            #endregion
        }
    }
}