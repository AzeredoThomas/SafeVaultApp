namespace SafeVault.Models
{
    public class VaultItem
    {
        public int Id { get; set; }
        public string OwnerUsername { get; set; }
        public string EncryptedData { get; set; }
    }
}
