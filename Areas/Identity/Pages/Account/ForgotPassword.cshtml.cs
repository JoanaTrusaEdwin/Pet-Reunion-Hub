// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using MimeKit;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
namespace Pet_Reunion_Hub.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }
                Console.WriteLine($"Original Email: {Input.Email}");
                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                var emailSettings = _configuration.GetSection("EmailSettings");
                var mailServer = emailSettings["MailServer"];
                var mailPort = int.Parse(emailSettings["MailPort"]);
                var senderName = emailSettings["SenderName"];
                var sender = emailSettings["Sender"];
                var password = emailSettings["Password"];

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, sender));
                message.To.Add(new MailboxAddress(user.UserName, user.Email));
                message.Subject = "Reset Password";

                var builder = new BodyBuilder { TextBody = $"Please reset your password by clicking here {HtmlEncoder.Default.Encode(callbackUrl)}" };
                message.Body = builder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(mailServer, mailPort, MailKit.Security.SecureSocketOptions.Auto);
                    client.Authenticate(sender, password);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
