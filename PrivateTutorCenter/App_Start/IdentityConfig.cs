using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using PrivateTutorCenter.Models;
using PrivateTutorCenter.Helper;

namespace PrivateTutorCenter
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private PrivateTutorUserStore _store;
        public ApplicationUserManager(PrivateTutorUserStore store)
            : base(store)
        {
            _store = store;
        }

        public override Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            try
            {
                PrivateTutorContext context = (PrivateTutorContext)_store.Context;
                var newUser = context.Users.Create();
                newUser.Email = user.Email;
                newUser.UserName = user.Email;
                newUser.PasswordHash = PasswordHasher.HashPassword(password);

                if (user.role == "student")
                {
                    var role = context.Roles.Where(r => r.Name == "Student").First();
                    newUser.Roles.Add(new IdentityUserRole { RoleId = role.Id, UserId = newUser.Id });
                    context.Users.Add(newUser);

                    context.SaveChanges();
                }
                else if (user.role == "teacher")
                {
                    var role = context.Roles.Where(r => r.Name == "Teacher").First();
                    newUser.Roles.Add(new IdentityUserRole { RoleId = role.Id, UserId = newUser.Id });
                    context.Users.Add(newUser);

                    context.SaveChanges();
                }

                return Task.FromResult(IdentityResult.Success);
            }
            catch (Exception)
            {
                return Task.FromResult(IdentityResult.Failed("DB Error"));
            }

        }
        public override Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authenticationType)
        {
            ClaimsIdentity identity = new ClaimsIdentity(authenticationType);
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));

            var roles = _store.GetRolesAsync(user).Result;
            foreach (string role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));

            identity.AddClaims(claims);

            return Task.FromResult(identity);

        }
        public override Task<ApplicationUser> FindAsync(string userName, string password)
        {

            string hashedPassword = PasswordHasher.HashPassword(password);
            return _store.Users.Where(u => u.Email == userName && u.PasswordHash == hashedPassword).FirstOrDefaultAsync();

        }

        public override Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return _store.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new PrivateTutorUserStore(context.Get<PrivateTutorContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            manager.PasswordHasher = new PrivateTutorHasher();

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            return manager;
        }
    }

    public class PrivateTutorUserStore : UserStore<ApplicationUser>
    {
        public PrivateTutorUserStore(PrivateTutorContext context) : base(context)
        {

        }

    }
    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {

        }

        public override Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            var user = UserManager.Find(userName, password);
            if (user == null)
                return Task.FromResult(SignInStatus.Failure);
            else
                return Task.FromResult(SignInStatus.Success);
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }



}
