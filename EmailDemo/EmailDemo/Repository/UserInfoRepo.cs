using EmailDemo.Data;
using EmailDemo.Helper;
using EmailDemo.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System.Data;
using EmailDemo.Eception;

namespace EmailDemo.Repository
{
    public class UserInfoRepo : IUserInfo
    {
        private readonly AppDbContext appDbContext;
        private readonly EmailSettings emailSettings;

        public UserInfoRepo(AppDbContext appDbContext, IOptions<EmailSettings> emailSettings) { 
         this.appDbContext = appDbContext;
            this.emailSettings = emailSettings.Value;
        }
      public async Task<bool> AddUser(UserInfo userInfo)
        {
           await appDbContext.UserInfo.AddAsync(userInfo);
           int row = appDbContext.SaveChanges();
            return row> 0;
        }

        //Send Email To All User
        public async Task<bool> sendmailToAllUser()
        {
             List<UserInfo> userInfos = await appDbContext.UserInfo.ToListAsync();

            if (userInfos == null || userInfos.Count == 0) {
                throw new UserException("User not found in database!!!");
            }
            int batchSize = 2;
            int maxRetry = 3;

            //split User into Batches
            var batches = userInfos.Select((user, index) => new {user,index}).
                GroupBy(x => (x.index / batchSize))
                .Select(g => g.Select(x => x.user).ToList())
                .ToList();

            foreach (var batch in batches) { 
            List<Task<bool>> emailTask = new List<Task<bool>>();
                foreach (UserInfo user in batch) {
                    string subject = "Hi Just for testing";
                    string body = $"Hello {user.name}, \n\n THIS IS IMPORTANT FOR YOU";
                    emailTask.Add(SendEmailWithRetryAsync(user.email, subject, body, maxRetry));
                }

                bool[] result = await Task.WhenAll(emailTask);
                for (int i = 0; i < result.Length; i++)
                {
                    if (!result[i])
                    {
                        Console.WriteLine($"Failed to send email to {batch[i].email}");
                    }
                }
            }
            return true;
        }

        //Send mail With Count The No of Retry..
        private async Task<bool> SendEmailWithRetryAsync(string email, string subject, string body, int maxRetry)
        {
            int retryCount = 0;
            while (retryCount < maxRetry) {
                try
                {
                    await SendEmailWithMailKit(email, subject, body);
                    return true;
                }
                catch (Exception ex) { 
                retryCount++;
                    retryCount++;
                    Console.WriteLine($"Attempt {retryCount} failed for {email}: {ex.Message}");

                    if (retryCount >= maxRetry)
                    {
                        // Log the failure after exhausting retries
                        Console.WriteLine($"Failed to send email to {email} after {maxRetry} attempts.");
                        return false;
                    }

                    // Wait before retrying (e.g., 2 seconds)
                    await Task.Delay(2000);
                }
            }
            return true;
        }
       

        //Send Mail TO User..
        private async Task SendEmailWithMailKit(string toEmail, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HII", emailSettings.Username));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };
            using (var client = new SmtpClient())
            {
               await client.ConnectAsync(emailSettings.smtpHost, emailSettings.smtpPort, emailSettings.EnableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None);
               await client.AuthenticateAsync(emailSettings.Username, emailSettings.password);
               await client.SendAsync(message);
               await client.DisconnectAsync(true);
            }
        }
    }
}
