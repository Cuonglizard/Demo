using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroRabbit.Domain.Core.Bus;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Mail;
using System.Net;
using Payments.Application.Events;


namespace Payments.Application.EventHandlers
{
    public class ORDEREventHandler : IEventHandler<ORDEREvent>
    {
        private readonly HttpClient _httpClient;


        public ORDEREventHandler(HttpClient httpClient)                 
        {
            _httpClient = httpClient;
        }

        public async Task Handle(ORDEREvent @event)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("duccuong25082001@gmail.com", "THCS Bách Khoa");
                mail.To.Add("tranduccuong25082001@gmail.com");
                mail.Subject = "Hóa Đơn Thanh Toán Đơn hàng";
                mail.Body = $@"
                    <html>
                    <body>
                        <div>
                            <p>Kính gửi a/c Trần Đức Cường!</p>
                            <p>Chúng tôi xin thông báo và xác nhận a/c đã đặt thành công đơn hàng. Email có đính kèm Hóa đơn thanh toán cho a/c.</p>
                            <p>Chi tiết đặt tour</p>
                            <table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%;'>
                                <tr>
                                    <td><strong>Mã giao dịch</strong></td>
                                    <td>{@event.CreateOrder.OrderId}</td>
                                </tr>
                                <tr>
                                    <td><strong>Tổng tiền thanh toán</strong></td>
                                    <td>{@event.CreateOrder.Account}</td>
                                </tr>
                                <tr>
                                    <td><strong>Hình thức thanh toán</strong></td>
                                    <td>Thanh toán online qua VNPay</td>
                                </tr>
                                <tr>
                                    <td><strong>Tình trạng</strong></td>
                                    <td>Đã thanh toán</td>
                                </tr>
                            </table>
                            <p>Chúc anh/chị một ngày mới vui vẻ!<br>
                            Mọi yêu cầu hay thắc mắc cần giải đáp xin vui lòng liên hệ tới hotline : 1900.6420</p>
                            <p>THCS Bách Khoa</p>
                        </div>
                    </body>
                    </html>
                ";
                mail.IsBodyHtml = true;

                // Cấu hình SMTP client và gửi email
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("duccuong25082001@gmail.comn", "ysvc zosv vdtm sgor");
                smtpClient.Send(mail);

                Console.WriteLine("Email sent successfully.");

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}