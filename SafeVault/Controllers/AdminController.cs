using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Data;
using SafeVault.Models;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;

    public AdminController(UserManager<IdentityUser> userManager,
                           RoleManager<IdentityRole> roleManager,
                           ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var users = _userManager.Users.ToList();
        var userRoles = new Dictionary<string, IList<string>>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userRoles[user.UserName] = roles;
        }

        ViewBag.UserRoles = userRoles;
        ViewBag.VaultItems = _context.VaultItems.ToList();

        return View(users);
    }
}
