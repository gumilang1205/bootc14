public class UserService
{
    private readonly IEmailService  _emailService;

    // Menerima IEmailService melalui Dependency Injection
    public UserService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public bool RegisterUser(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            return false;
        }

        // Proses menyimpan user ke database berhasil...

        // Panggil dependensi untuk mengirim email
        return _emailService.SendWelcomeEmail(username);
    }
}