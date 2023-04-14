using firstapi.Helpter;

namespace firstapi.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(Mailrequest mailrequest);
    }
}
