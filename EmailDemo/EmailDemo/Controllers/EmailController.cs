using AutoMapper;
using EmailDemo.Helper;
using EmailDemo.Model;
using EmailDemo.Model.Entity;
using EmailDemo.Service;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Reflection.Metadata.Ecma335;

namespace EmailDemo.Controllers
{
    public class EmailController : ControllerBase
    {
        private readonly EmailSettings emailSettings;
        private readonly UserInfoService userInfoservice;
        public readonly IMapper mapper;

        public EmailController(IOptions<EmailSettings> emailSettings,UserInfoService service,IMapper mapper)
        {
            this.emailSettings = emailSettings.Value;
            userInfoservice = service;
            this.mapper = mapper;
        }


        //SEND Email TO User....

        [HttpPost("sendemail")]
        public IActionResult sendEmail([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.userEmail))
            {
                return BadRequest("Invalid user data.");
            }
            try
            {
                string subject = "Hello " + user.userName + " ! " + user.subject;
                SendEmailWithMailKit(user.userEmail, subject, user.body);
                return Ok($"Email sent successfully to {user.userEmail}");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //ADD User in DataBase in SQLSEVER...

        [HttpPost("adduser")]
        public async Task<IActionResult> AddUser([FromBody] UserInfoDTO userInfoDTO) {
            if (userInfoDTO == null) {

                return BadRequest("Enter the Valid User!!!!");
            }
            var user = mapper.Map<UserInfo>(userInfoDTO);
            bool flag = await userInfoservice.AddUserInfo(user);
            if (flag == true) { return Ok("UserInfo Added SucessFully!!!");
            }
            return BadRequest("Error in Add UserInfo !!!");

        }

        //Send Email for all User Present in Database
        [HttpGet("sendmailtoalluser")]
        public async Task<IActionResult> sendMailToAllUser()
        {
            try {
                bool flag = await userInfoservice.sendMailToAllUser();
                if (flag == true) {
                    return Ok("Mail Send Successfully for All user present in Database!!!");
                }
                return BadRequest("Mail send to all user Unsucessfullly!!!");
            } catch (Exception ex) { 
            return BadRequest(ex.Message);
            }
        }

        //verify the user
        [HttpPost("verifymail")]
        public async Task<IActionResult> validateEmail(string email, string token) {
            bool flag = await userInfoservice.validateEmail(email, token);
            Console.WriteLine($"Controller - Email: {email}, Token: {token}");
            return (flag == true ? Ok("Email Verify Successfully!!!!") : BadRequest("In valid User!!!"));
        }


        //Send Email

        private void SendEmailWithMailKit(string toEmail, string subject, string body)
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
                client.Connect(emailSettings.smtpHost, emailSettings.smtpPort, emailSettings.EnableSsl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None);
                client.Authenticate(emailSettings.Username, emailSettings.password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
