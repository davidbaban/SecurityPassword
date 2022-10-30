namespace SecurityPassword;

public class Program
{
    public static void Main(string[] args)
    {
        var generator = new OneTimePasswordGenerator();

        Console.WriteLine("Please introduce your user id: ");
        var userId = Console.ReadLine();

        if (string.IsNullOrEmpty(userId))
        {
            Console.WriteLine("The userId cannot be empty! Please introduce a value!");
            return;
        }

        var generatedPassword = generator.GenerateOneTimePassword(userId, DateTime.Now);
        Console.WriteLine($"The generated one time password is: {generatedPassword}!");

        Console.WriteLine("Please validate your one time password: ");
        var introducedPassword = Console.ReadLine();

        var validationMessage = generator.ValidateSecurityPassword(userId, introducedPassword ?? string.Empty) ? "valid!" : "not valid!";
        Console.WriteLine($"The introduced password is {validationMessage}!");
    }
}

public class OneTimePasswordGenerator
{
    public SecurityService SecurityService;

    public OneTimePasswordGenerator()
    {
        SecurityService = new SecurityService();
    }

    public string GenerateOneTimePassword(string userId, DateTime dateTime)
    {
        var securityPassword = SecurityService.GenerateOneTimePassword(userId, dateTime.ToString("MM/dd/yyyy hh:mm:ss"));

        return securityPassword;
    }

    public bool ValidateSecurityPassword(string userId, string requestSecurityPassword)
    {
        var range = Enumerable.Range(0, 30);

        foreach (var interval in range)
        {
            var candidatePassword = GenerateOneTimePassword(userId, DateTime.Now.AddSeconds(-1 * interval));
            if (candidatePassword == requestSecurityPassword)
                return true;
        }

        return false;
    }
}

