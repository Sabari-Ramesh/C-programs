using System.ComponentModel.DataAnnotations;

namespace EncryptionDemo.DTO
{
    public class UserDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string encryptedData { get; set; }
    }
}
