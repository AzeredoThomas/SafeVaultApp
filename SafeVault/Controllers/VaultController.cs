using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Data;
using SafeVault.Models;
using System.Security.Claims;

[Authorize]
public class VaultController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly EncryptionService _encryption;

    public VaultController(ApplicationDbContext context, EncryptionService encryption)
    {
        _context = context;
        _encryption = encryption;
    }

    public IActionResult Index()
    {
        var username = User.FindFirstValue(ClaimTypes.Name);
        var items = _context.VaultItems
            .Where(v => v.OwnerUsername == username)
            .Select(v => new
            {
                v.OwnerUsername,
                DecryptedData = _encryption.Decrypt(v.EncryptedData)
            }).ToList();

        return View(items); // Du kan skapa en ViewModel om du vill
    }

    [HttpPost]
    public IActionResult Store(VaultInputModel model)
    {
        // ✅ Validera input enligt DataAnnotations
        if (!ModelState.IsValid)
            return View("Create", model);

        // 🔐 Hämta användarnamn från inloggad användare
        var username = User.FindFirstValue(ClaimTypes.Name);
        if (string.IsNullOrEmpty(username))
            return Unauthorized();

        // 🔄 Mappa och kryptera input via VaultMapper
        var vaultItem = VaultMapper.ToVaultItem(model, username, _encryption);

        // 💾 Spara till databasen
        _context.VaultItems.Add(vaultItem);
        _context.SaveChanges();

        // 🔁 Skicka tillbaka till valvets översikt
        return RedirectToAction("Index");
    }


}
