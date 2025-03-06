using System.Collections.Concurrent;

namespace EmailDemo.Service
{
    public class EmailTokenService
    {
        // The ConcurrentDictionary to store email tokens
        private readonly ConcurrentDictionary<string, (string Token, DateTime Expire)> emailToken = new();

        // Generate a token for an email
        public string GenerateTokenForEmail(string email)
        {
            email = email.ToLower();
            string token = Guid.NewGuid().ToString();
            DateTime expire = DateTime.UtcNow.AddMinutes(30); // AddDays(5).
            emailToken[email] = (token, expire);
            return token;
        }

        // Validate an email token
        public bool ValidateEmail(string email, string userToken)
        {
            email = email.ToLower();
            if (emailToken.TryGetValue(email, out var storedToken))
            {
                if (storedToken.Token == userToken && storedToken.Expire > DateTime.UtcNow)
                {
                    emailToken.TryRemove(email, out _);
                    return true;
                }
            }
            return false;
        }
    }
}
