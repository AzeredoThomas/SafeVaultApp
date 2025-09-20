using SafeVault.Models;

public static class VaultMapper
{
    public static VaultItem ToVaultItem(VaultInputModel input, string ownerUsername, EncryptionService encryption)
    {
        var cleanInput = InputSanitizer.Clean(input.Secret);
        var encrypted = encryption.Encrypt(cleanInput);

        return new VaultItem
        {
            OwnerUsername = ownerUsername,
            EncryptedData = encrypted
        };
    }
}
