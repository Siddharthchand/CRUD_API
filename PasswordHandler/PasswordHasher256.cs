using System.Text;
using System.Security.Cryptography;

namespace User_API.PasswordHandler
{
    public interface PasswordHasher256
    {
        public string HashPassword(string Password);
        public bool verifyPasswords(string Password1, string Password2);

    }
    public class PasswordHasher : PasswordHasher256
    {
        public string HashPassword(string Password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(Password); //password to byte

                byte[] hashBytes = sha256.ComputeHash(passwordBytes); //generate hash of password

                // Convert the hash bytes to a hexadecimal string
                StringBuilder hashStringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashStringBuilder.Append(b.ToString("x2")); // Convert each byte to a 2-digit hexadecimal
                }

                return hashStringBuilder.ToString(); // Return the hashed password as a hexadecimal string
            }
        }
        public bool verifyPasswords(string Password1, string Password2)
        {
            return Password1.Equals(Password2);
        }
    }
}