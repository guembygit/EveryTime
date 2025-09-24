using System.Net.Mail;
using System.Net;

namespace PlanningTime.Services
{
    public class MailService
    {
        public void SendLeaveRequestMail(string employeeName, string leaveType, DateTime start, DateTime end)
        {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("b75ceae66189aa", "7e3dea674311ca"),
                EnableSsl = true
            };

            // ✅ Objet du mail
            string subject = $"Nouvelle demande de {leaveType} - {employeeName}";

            // ✅ Logo (doit être accessible via URL ou en CID pour inline)
            string logoUrl = "https://localhost:7022/images/logo.png";

            // ✅ Body HTML
            string body = $@"
            <html>
                <body style='font-family: Arial, sans-serif; color:#333;'>
                    <div style='border:1px solid #ddd; border-radius:8px; padding:20px;'>
                        <div style='text-align:center; margin-bottom:20px;'>
                            <img src='{logoUrl}' alt='Logo Entreprise' style='max-height:60px;' />
                        </div>

                        <h2 style='color:#28a745;'>Nouvelle demande d'absence</h2>
                        <p>
                            Bonjour,<br/><br/>
                            L'employé <strong>{employeeName}</strong> a soumis une demande de 
                            <strong>{leaveType}</strong> du 
                            <strong>{start:dd/MM/yyyy}</strong> au <strong>{end:dd/MM/yyyy}</strong>.
                        </p>

                        <p>
                            Veuillez <a href='https://localhost:7022/AdminEvents' 
                            style='color:#007bff; text-decoration:none; font-weight:bold;'>cliquer ici</a> 
                            pour approuver ou rejeter la demande.
                        </p>

                        <br/>
                        <p style='font-size:12px; color:#888;'>
                            Ceci est un message automatique, merci de ne pas répondre directement à cet email.
                        </p>
                    </div>
                </body>
            </html>";

            var message = new MailMessage
            {
                From = new MailAddress("sergeguemby26@gmail.com", "WattPlanning"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true // ✅ très important
            };

            // destinataire (admin par ex.)
            message.To.Add("guemby26@gmail.com");

            client.Send(message);
        }
        public void SendLeaveRequestMail2(string employeeName, string leaveType, DateTime start, DateTime end)
        {
            var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("b75ceae66189aa", "7e3dea674311ca"),
                EnableSsl = true
            };

            // Objet du mail
            string subject = $"Demande {leaveType} - {employeeName}";

            // ✅ Logo (doit être accessible via URL ou en CID pour inline)
            string logoUrl = "https://localhost:7022/images/logo.png";

            // ✅ Body HTML
            string body = $@"
            <html>
                <body style='font-family: Arial, sans-serif; color:#333;'>
                    <div style='border:1px solid #ddd; border-radius:8px; padding:20px;'>
                        <div style='text-align:center; margin-bottom:20px;'>
                            <img src='{logoUrl}' alt='Logo Entreprise' style='max-height:60px;' />
                        </div>

                        <h2 style='color:#28a745;'>Nouvelle demande d'absence</h2>
                        <p>
                            Bonjour,<br/><br/>
                            L'Admin <strong>{employeeName}</strong> a répondu à votre demande du 
                            <strong>{start:dd/MM/yyyy}</strong> au <strong>{end:dd/MM/yyyy}</strong>.
                        </p>

                        <p>
                            Veuillez <a href='https://localhost:7022/Events/MyEvents' 
                            style='color:#007bff; text-decoration:none; font-weight:bold;'>cliquer ici</a> 
                            pour approuver ou rejeter la demande.
                        </p>

                        <br/>
                        <p style='font-size:12px; color:#888;'>
                            Ceci est un message automatique, merci de ne pas répondre directement à cet email.
                        </p>
                    </div>
                </body>
            </html>";

            var message = new MailMessage
            {
                From = new MailAddress("sergeguemby26@gmail.com", "WattPlanning"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true // ✅ très important
            };

            // destinataire (admin par ex.)
            message.To.Add("guemby26@gmail.com");

            client.Send(message);
        }

    }

}
