//using EmailDemo.Data;
//using EmailDemo.Helper;
//using EmailDemo.Model.Entity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using MimeKit;
//using MailKit.Net.Smtp;
//using System.Data;
//using EmailDemo.Eception;
//using System.Collections.Concurrent;

//namespace EmailDemo.Repository
//{
//    public class UserInfoRepo : IUserInfo
//    {
//        private readonly AppDbContext appDbContext;
//        private readonly EmailSettings emailSettings;
//        private readonly ConcurrentDictionary<string, (string Token, DateTime Expire)> emailToken = new();

//        public UserInfoRepo(AppDbContext appDbContext, IOptions<EmailSettings> emailSettings) { 
//         this.appDbContext = appDbContext;
//            this.emailSettings = emailSettings.Value;
//        }


//        //Generate Email Token
//        public string generateTokenForEmail(string email)
//        {
//            // Normalize email to lowercase
//            email = email.ToLower();

//            string token = Guid.NewGuid().ToString();
//            DateTime expire = DateTime.UtcNow.AddMinutes(30); //AddDays(5).
//            emailToken[email] = (token, expire);

//            Console.WriteLine($"Repo - Generated Token for Email: {email}, Token: {token}, Expires: {expire}");
//            return token;
//        }

//        //Validate the UserEmail
//        public async Task<bool> validateEmail(string email, string userToken)
//        {
//            // Normalize email to lowercase
//            email = email.ToLower();
//            Console.WriteLine($"Repo - Validating Email: {email}, Token: {userToken}");

//            // Log all entries in emailToken for debugging
//            //foreach (var entry in emailToken)
//            //{
//            //    Console.WriteLine($"Repo - Stored Email: {entry.Key}, Token: {entry.Value.Token}, Expires: {entry.Value.Expire}");
//            //}

//            if (emailToken.TryGetValue(email, out var storedToken))
//            {
//                Console.WriteLine($"Repo - Stored Token: {storedToken.Token}, User Token: {userToken}");
//                if (storedToken.Token == userToken && storedToken.Expire > DateTime.UtcNow)
//                {
//                    emailToken.TryRemove(email, out _);
//                    return true;
//                }
//            }

//            return false;
//        }

//        //Add user toDb
//        public async Task<bool> AddUser(UserInfo userInfo)
//        {
//           await appDbContext.UserInfo.AddAsync(userInfo);
//           int row = appDbContext.SaveChanges();
//            return row> 0;
//        }

//        //Send Email To All User
//        public async Task<bool> sendmailToAllUser()
//        {
//             List<UserInfo> userInfos = await appDbContext.UserInfo.ToListAsync();

//            if (userInfos == null || userInfos.Count == 0) {
//                throw new UserException("User not found in database!!!");
//            }
//            int batchSize = 2;
//            int maxRetry = 3;

//            //split User into Batches
//            var batches = userInfos.Select((user, index) => new {user,index}).
//                GroupBy(x => (x.index / batchSize))
//                .Select(g => g.Select(x => x.user).ToList())
//                .ToList();

//            foreach (var batch in batches) { 
//            List<Task<bool>> emailTask = new List<Task<bool>>();
//                foreach (UserInfo user in batch) {
//                    // Generate a token for the user
//                    string token =generateTokenForEmail(user.email);
//                    // Construct the confirmation link
//                    string confirmationLink = $"https://yourwebsite.com/verify-email?email={user.email}&token={token}";
//                    // Create the email body
//                    string subject = "Please verify your email";
//                    string body = $"Hello {user.name},\n\nPlease verify your email by clicking the following link: {confirmationLink}";
//                    emailTask.Add(SendEmailWithRetryAsync(user.email, subject, body, maxRetry));
//                }

//                bool[] result = await Task.WhenAll(emailTask);
//                for (int i = 0; i < result.Length; i++)
//                {
//                    if (!result[i])
//                    {
//                        Console.WriteLine($"Failed to send email to {batch[i].email}");
//                    }
//                }
//            }
//            return true;
//        }

//        //Send mail With Count The No of Retry..
//        private async Task<bool> SendEmailWithRetryAsync(string email, string subject, string body, int maxRetry)
//        {
//            int retryCount = 0;
//            while (retryCount < maxRetry) {
//                try
//                {
//                    await SendEmailWithMailKit(email, subject, body);
//                    return true;
//                }
//                catch (Exception ex) { 
//                retryCount++;
//                    retryCount++;
//                    Console.WriteLine($"Attempt {retryCount} failed for {email}: {ex.Message}");

//                    if (retryCount >= maxRetry)
//                    {
//                        Console.WriteLine($"Failed to send email to {email} after {maxRetry} attempts.");
//                        return false;
//                    }
//                    await Task.Delay(2000);
//                }
//            }
//            return true;
//        }


//        //Send Mail TO User..
//        private async Task SendEmailWithMailKit(string toEmail, string subject, string body)
//        {
//            var message = new MimeMessage();
//            message.From.Add(new MailboxAddress("HII", emailSettings.Username));
//            message.To.Add(MailboxAddress.Parse(toEmail));
//            message.Subject = subject;
//            message.Body = new TextPart("plain")
//            {
//                Text = body
//            };
//            using (var client = new SmtpClient())
//            {
//               await client.ConnectAsync(emailSettings.smtpHost, emailSettings.smtpPort, emailSettings.EnableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None);
//               await client.AuthenticateAsync(emailSettings.Username, emailSettings.password);
//               await client.SendAsync(message);
//               await client.DisconnectAsync(true);
//            }
//        }
//    }
//}




using EmailDemo.Data;
using EmailDemo.Helper;
using EmailDemo.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using System.Data;
using EmailDemo.Eception;
using EmailDemo.Service;

namespace EmailDemo.Repository
{
    public class UserInfoRepo : IUserInfo
    {
        private readonly AppDbContext appDbContext;
        private readonly EmailSettings emailSettings;
        private readonly EmailTokenService emailTokenService;

        public UserInfoRepo(AppDbContext appDbContext, IOptions<EmailSettings> emailSettings, EmailTokenService emailTokenService)
        {
            this.appDbContext = appDbContext;
            this.emailSettings = emailSettings.Value;
            this.emailTokenService = emailTokenService;
        }

        // Generate Email Token
        public string generateTokenForEmail(string email)
        {
            return emailTokenService.GenerateTokenForEmail(email);
        }

        // Validate the UserEmail
        public async Task<bool> validateEmail(string email, string userToken)
        {
            return emailTokenService.ValidateEmail(email, userToken);
        }

        // Add user to Db
        public async Task<bool> AddUser(UserInfo userInfo)
        {
            await appDbContext.UserInfo.AddAsync(userInfo);
            int row = appDbContext.SaveChanges();
            return row > 0;
        }

        // Send Email To All Users
        public async Task<bool> sendmailToAllUser()
        {
            List<UserInfo> userInfos = await appDbContext.UserInfo.ToListAsync();
            if (userInfos == null || userInfos.Count == 0)
            {
                throw new UserException("User not found in database!!!");
            }

            int batchSize = 2;
            int maxRetry = 3;

            // Split users into batches
            var batches = userInfos.Select((user, index) => new { user, index })
                                   .GroupBy(x => (x.index / batchSize))
                                   .Select(g => g.Select(x => x.user).ToList())
                                   .ToList();

            foreach (var batch in batches)
            {
                List<Task<bool>> emailTask = new List<Task<bool>>();
                foreach (UserInfo user in batch)
                {
                    // Generate a token for the user
                    string token = generateTokenForEmail(user.email);

                    // Construct the confirmation link
                    string confirmationLink = $"https://yourwebsite.com/verify-email?email={user.email}&token={token}";

                    // Create the email body
                    string subject = "Please verify your email";
                    string body = $"Hello {user.name},\n\nPlease verify your email by clicking the following link: {confirmationLink}";

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

        // Send mail with retry count
        private async Task<bool> SendEmailWithRetryAsync(string email, string subject, string body, int maxRetry)
        {
            int retryCount = 0;
            while (retryCount < maxRetry)
            {
                try
                {
                    await SendEmailWithMailKit(email, subject, body);
                    return true;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Console.WriteLine($"Attempt {retryCount} failed for {email}: {ex.Message}");
                    if (retryCount >= maxRetry)
                    {
                        Console.WriteLine($"Failed to send email to {email} after {maxRetry} attempts.");
                        return false;
                    }
                    await Task.Delay(2000);
                }
            }
            return true;
        }

        // Send mail using MailKit
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