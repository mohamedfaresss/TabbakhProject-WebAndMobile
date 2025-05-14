namespace DataAcess.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmailAsync(string email, string resetLink);
    }

    public class EmailService : IEmailService
    {
        // Add dependencies (e.g., SMTP settings or third-party service like SendGrid)
        public async Task SendResetPasswordEmailAsync(string email, string resetLink)
        {
            // Example: Use SMTP or a service like SendGrid
            Console.WriteLine($"Sending email to {email} with reset link: {resetLink}");
            // Implement actual email sending logic here
            // Example using SMTP (replace with your configuration):
            /*
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("your-email@gmail.com", "your-password"),
                EnableSsl = true,
            };
            var mailMessage = new MailMessage
            {
                From = new MailAddress("your-email@gmail.com"),
                Subject = "Password Reset Request",
                Body = $"Please click the following link to reset your password: {resetLink}",
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);
            await smtpClient.SendMailAsync(mailMessage);
            */
            await Task.CompletedTask;
        }
    }
}