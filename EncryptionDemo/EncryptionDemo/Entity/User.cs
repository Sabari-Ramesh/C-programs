﻿using System.ComponentModel.DataAnnotations;

namespace EncryptionDemo.Entity
{
    public class User
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string encryptedData { get; set; }

    }
}
