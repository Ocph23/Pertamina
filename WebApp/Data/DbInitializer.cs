using System.Threading.Tasks;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Data
{
    public class DbInitializer
    {
        public static async Task<bool> Initialize(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            context.Database.EnsureCreated();
            if (!context.Roles.Any())
            {

                context.Roles.Add(new IdentityRole { Name = "administrator", NormalizedName = "ADMINISTRATOR" });
                context.Roles.Add(new IdentityRole { Name = "admin", NormalizedName = "ADMIN" });
                context.Roles.Add(new IdentityRole { Name = "karyawan", NormalizedName = "KARYAWAN" });
                context.Roles.Add(new IdentityRole { Name = "manager", NormalizedName = "MANAGER" });
                context.SaveChanges();
            }


            if (!context.Users.Any())
            {
                try
                {
                    var user = new IdentityUser { UserName = "administrator" };
                    var createResult = await userManager.CreateAsync(user, "Administrator123#");
                    await userManager.AddToRoleAsync(user, "administrator");
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception(ex.Message);
                }
            }

            return true;

        }
    }
}