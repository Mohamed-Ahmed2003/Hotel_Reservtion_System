using Hotel_Reservtion_System.Entity;
using Hotel_Reservtion_System.ServicesContracts;
using MailKit.Net.Smtp;

namespace Hotel_Reservtion_System.Services
{
    public class EmailService : IEmailServices
    {
        public async Task sendEmail(string userEmail, Invoice invoice)
        {
            var email = new MimeKit.MimeMessage();
            email.From.Add(MimeKit.MailboxAddress.Parse(Environment.GetEnvironmentVariable("EMAIL_USER")));
            email.To.Add(MimeKit.MailboxAddress.Parse(userEmail)); 
            email.Subject = "Hotel Reservation Invoice";
            email.Body = new MimeKit.TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
                <h1>Invoice Details</h1>
                <p>Booking ID: {invoice.booking.id}</p>
                <p>User: {invoice.booking.user.name} ({invoice.booking.user.email})</p>
                <p>Room: {invoice.booking.room.roomCode} - {invoice.booking.room.roomType}</p>
                <p>Check-In: {invoice.booking.checkIn}</p>
                <p>Check-Out: {invoice.booking.checkOut}</p>
                <p>Total Amount: ${invoice.totalAmount}</p>
                <p>Tax Amount: ${invoice.tax}</p>
                <p>Discount: ${invoice.discount}</p>
                <p>Final Amount: ${invoice.finalAmount}</p>
                <br/>
                <p>Thank you for choosing our hotel!</p>"
            };
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(Environment.GetEnvironmentVariable("EMAIL_USER"), Environment.GetEnvironmentVariable("EMAIL_PASSWORD"));
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
