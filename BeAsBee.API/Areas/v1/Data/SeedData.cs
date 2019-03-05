using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeAsBee.Infrastructure.Sql.Models;
using BeAsBee.Infrastructure.Sql.Models.Context;
using BeAsBee.Infrastructure.Sql.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace BeAsBee.API.Areas.v1.Data {
    public static class SeedData {
        public static async Task Initialize ( ApplicationContext db, RoleManager<Role> roleManager, UserManager<User> userManager ) {
            db.Database.EnsureCreated();

            if ( db.Users.Any() ) {
                return;
            }

            #region Roles

            await roleManager.CreateAsync( new Role {Name = "Admin"} );
            await roleManager.CreateAsync( new Role {Name = "User"} );

            #endregion

            #region Users

            var user1 = new User {
                UserName = "test1",
                Email = "test1@gmail.com",
                FirstName = "Павло",
                SecondName = "Дідик"
            };
            var user2 = new User {
                UserName = "test2",
                Email = "test2@gmail.com",
                FirstName = "Андрій",
                SecondName = "Лопатін"
            };
            var user3 = new User {
                UserName = "test3",
                Email = "test3@gmail.com",
                FirstName = "Сергій",
                SecondName = "Скорилко"
            };
            var user4 = new User {
                UserName = "test4",
                Email = "test4@gmail.com",
                FirstName = "Назар",
                SecondName = "Лучит"
            };

            await userManager.CreateAsync( user1, "test1" );
            await userManager.CreateAsync( user2, "test2" );
            await userManager.CreateAsync( user3, "test3" );
            await userManager.CreateAsync( user4, "test4" );

            await userManager.AddToRoleAsync( user1, "User" );
            await userManager.AddToRoleAsync( user2, "User" );
            await userManager.AddToRoleAsync( user3, "User" );
            await userManager.AddToRoleAsync( user4, "User" );

            #endregion

            db.SaveChanges();

            #region Chats

            var chat1 = new Chat {
                Id = Guid.NewGuid(),
                UserId = user1.Id,
                Name = "Група КН - 321",
                Messages = new List<Message> {
                    new Message {
                        Id = Guid.NewGuid(),
                        MessageText = "Привіт, коли потрібно на пари?",
                        ReceivedTime = new DateTimeOffset( new DateTime( 2010, 10, 10 ) ).AddDays( 2 ),
                        UserId = user1.Id
                    },
                    new Message {
                        Id = Guid.NewGuid(),
                        MessageText = "Привіт завтра на 12!",
                        ReceivedTime = new DateTimeOffset( new DateTime( 2010, 10, 10 ) ).AddDays( 2 ),
                        UserId = user2.Id
                    },
                    new Message {
                        Id = Guid.NewGuid(),
                        MessageText = "Дякую, до завтра!",
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
                        ReceivedTime = new DateTimeOffset( new DateTime( 2010, 10, 10 ) ).AddDays( 52 ),
                        UserId = user1.Id
                    },
                    new Message {
                        Id = Guid.NewGuid(),
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