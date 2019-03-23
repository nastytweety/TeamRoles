using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TeamRoles.Models;

namespace TeamRoles.Hubs
{
    public class MessageRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //public async Task<bool> SendMessage(string senderUsername, string receiverUsername, string body)
        //{
        //    try
        //    {
        //        var sender = await db.Users.Where(x => x.UserName == senderUsername).FirstOrDefaultAsync();
        //        var receiver = await db.Users.Where(x => x.UserName == receiverUsername).FirstOrDefaultAsync();
        //        Message message = new Message { Body = body, Date = DateTime.Now, Read = false, Sender = sender, Receiver = receiver };
        //        db.Messages.Add(message);
        //        await db.SaveChangesAsync();
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}

        //public async Task<int> SendMessage(string senderUsername, string receiverUsername, string body)
        //{
        //    try
        //    {
        //        var sender = await db.Users.Where(x => x.UserName == senderUsername).FirstOrDefaultAsync();
        //        var receiver = await db.Users.Where(x => x.UserName == receiverUsername).FirstOrDefaultAsync();
        //        Message message = new Message { Body = body, Date = DateTime.Now, Read = false, Sender = sender, Receiver = receiver };
        //        db.Messages.Add(message);
        //        await db.SaveChangesAsync();
        //        var messageSent = db.Messages.Attach(message);
        //        var messageId = messageSent.Id;
        //        return messageId;
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}

        public async Task<int> SendMessage(string senderUsername, string receiverUsername, string body)
        {
            try
            {
                var sender = await db.Users.Where(x => x.UserName == senderUsername).FirstOrDefaultAsync();
                var receiver = await db.Users.Where(x => x.UserName == receiverUsername).FirstOrDefaultAsync();
                Message message = new Message { Body = body, Date = DateTime.Now, Read = false, Sender = sender, Receiver = receiver };
                db.Messages.Add(message);
                await db.SaveChangesAsync();
                var messageSent = db.Messages.Attach(message);
                var messageId = messageSent.Id;
                return messageId;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<object> GetMessages(string currentUserId, string username)
        {
            try
            {
                var user = await db.Users.Where(x => x.Id == currentUserId).FirstOrDefaultAsync();
                var receivedMessages = await db.Messages.Include("Receiver").Include("Sender").Where(x => x.Receiver.Id == user.Id && x.Sender.UserName == username).Select(z => new { Body = z.Body, Date = z.Date, Received = true, SenderAvatar = z.Sender.ProfilePic }).ToListAsync();
                var sentMessages = await db.Messages.Include("Receiver").Include("Sender").Where(x => x.Sender.Id == user.Id && x.Receiver.UserName == username).Select(z => new { Body = z.Body, Date = z.Date, Received = false, SenderAvatar = z.Sender.ProfilePic }).ToListAsync();
                var allMessages = receivedMessages.Concat(sentMessages).OrderBy(x => x.Date).ToList();
                return new { allMessages };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<object> MessagesCount(string id)
        {
            try
            {
                var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                var sentMessagesCount = await db.Messages.Where(x => x.Sender.Id == user.Id).CountAsync();
                var receivedMessagesCount = await db.Messages.Where(x => x.Receiver.Id == user.Id).CountAsync();
                var allMessagesCount = await db.Messages.CountAsync();
                return new { sentMessagesCount, receivedMessagesCount, allMessagesCount };
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<object> UnreadMessagesCount(string receiverId, string senderUsername)
        {
            try
            {
                var user = await db.Users.Where(x => x.Id == receiverId).FirstOrDefaultAsync();
                var unreadMessagesCount = await db.Messages.Where(x => x.Receiver.Id == user.Id && x.Sender.UserName == senderUsername && x.Read == false).CountAsync();
                return unreadMessagesCount;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<int>> MessagesCountForStatistics(string id)
        {
            try
            {
                var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
                var sentMessagesCount = await db.Messages.Where(x => x.Sender.Id == user.Id).CountAsync();
                var receivedMessagesCount = await db.Messages.Where(x => x.Receiver.Id == user.Id).CountAsync();
                var allMessagesCount = sentMessagesCount + receivedMessagesCount;
                List<int> messagesCount = new List<int> { sentMessagesCount, receivedMessagesCount, allMessagesCount };
                return messagesCount;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // for log
        public async Task<List<Message>> GetMyMessages(string id)
        {
            var user = await db.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            var messages = await db.Messages.Include("Sender").Include("Receiver").Where(x => x.Sender.Id == id || x.Receiver.Id == id).ToListAsync();
            return messages;
        }

        // for God

        public async Task<List<Message>> GodMessages()
        {
            var messages = await db.Messages.Include("Sender").Include("Receiver").ToListAsync();
            return messages;
        }
    }
}