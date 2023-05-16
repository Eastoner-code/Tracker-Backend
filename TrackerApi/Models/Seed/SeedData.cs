using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using TrackerApi.Models;

namespace TrackerApi.Models.Seed
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            Task.Run(async () =>
            {
                using var scope = serviceProvider.CreateScope();
                var context = scope.ServiceProvider.GetService<TrackerContext>();

                string[] roles = new string[] { "SuperAdmin", "Admin", "AbsenceApprover", "User", "Supervisor", "Recruiter" };

                foreach (string role in roles)
                {
                    var roleStore = new IdentityAuthRoleStore(context);

                    if (!context.Roles.Any(r => r.Name == role))
                    {
                        await roleStore.CreateAsync(new IdentityAuthRole(role));
                    }
                }


                var admin = new IdentityAuthUser
                {
                    FirstName = "Admin",
                    LastName = "Adminovich",
                    Email = "admin@eastoner.net",
                    NormalizedEmail = "ADMIN@EASTONER.NET",
                    UserName = "admin@eastoner.net",
                    NormalizedUserName = "ADMIN@EASTONER.NET",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    StartDateUtc = DateTime.UtcNow.AddYears(-1),
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };

                await CreateUser(context, admin, scope, roles, "!Admin11");

                var users = new List<IdentityAuthUser>()
                {
                    GetUser("Eduard","Mosesov",  "e.mosesov@eastoner.net", new DateTime(2020, 9, 7,22,0,0,DateTimeKind.Utc)),
                    GetUser("Yurii","Koval",  "ykoval@eastoner.net", new DateTime(2019, 3, 11,22,0,0,DateTimeKind.Utc)),
                    GetUser("Denis","Strilets",  "dens@eastoner.net", new DateTime(2018, 11, 5,22,0,0,DateTimeKind.Utc)),
                    GetUser("Vasyl","Zadorozhnyy",  "v.zadorozhnyy@eastoner.net", new DateTime(2019, 8, 18,22,0,0,DateTimeKind.Utc)),
                    GetUser("Nazar","Mykytyshyn",  "nmykytyshyn@eastoner.net", new DateTime(2019,11, 18,22,0,0,DateTimeKind.Utc)),
                    GetUser("Oleh","Leskiv",  "oleg@eastoner.net", new DateTime(2019, 1, 16,22,0,0,DateTimeKind.Utc)),
                    GetUser("Mykola","Vanko",  "m.vanko@eastoner.net", new DateTime(2020, 7, 6,22,0,0,DateTimeKind.Utc)),
                    GetUser("Mykhailo", "Bandura", "m.bandura@eastoner.net", DateTime.UtcNow)
                };

                foreach (var user in users)
                {
                    await CreateUser(context, user, scope, new[] { "User" }, "!User111");
                }

                await CreateProjectAndAssignUserAsync(context, users[0]);
                await context.SaveChangesAsync();
            });

        }

        private static async Task CreateProjectAndAssignUserAsync(TrackerContext context, IdentityAuthUser user)
        {
            var project = new Project { Name = "TestProject", CreatedAtUtc = DateTime.UtcNow, UpdatedAtUtc = DateTime.UtcNow, IsArchive = false, Meta = "{\"test\":\"test\"}" };
            await context.Project.AddAsync(project);
            var userProject = new UserProject { User = user, Project = project };
            await context.UserProject.AddAsync(userProject);
            await CreateActivitiesAsync(context, user,project);
        }

        private static async Task CreateActivitiesAsync(TrackerContext context, IdentityAuthUser user, Project project)
        {
            var workedFrom = new DateTime(2021, 11, 7, 15, 0, 0);
            var workedTo = new DateTime(2021, 11, 7, 20, 0, 0);
            List<Activity> activities = new List<Activity>()
            {
                new Activity{ User = user, Project = project, CreatedAtUtc = DateTime.UtcNow, WorkedFromUtc = workedFrom, WorkedToUtc = workedTo, Duration = 300 },
                new Activity{ User = user, Project = project, CreatedAtUtc = DateTime.UtcNow, WorkedFromUtc = workedFrom.AddDays(1), WorkedToUtc = workedTo.AddDays(1), Duration = 300 }
            };
            await context.AddRangeAsync(activities);
        }

        private static IdentityAuthUser GetUser(string firstName, string lastName, string email, DateTime startDate)
        {
            return new IdentityAuthUser
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = email,
                NormalizedUserName = email.ToUpper(),
                PhoneNumber = "+1" + LongRandom(10000000000, 99999999999, new Random()),
                EmailConfirmed = true,
                StartDateUtc = startDate,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                TimeZone = "Europe/Kiev"
            };
        }

        private static long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);

            return (Math.Abs(longRand % (max - min)) + min);
        }

        private static async Task CreateUser(TrackerContext context, IdentityAuthUser user, IServiceScope scope,
            string[] roles, string simplePassword)
        {
            if (!context.Users.Any(u => u.UserName == user.UserName))
            {
                var password = new PasswordHasher<IdentityAuthUser>();
                var hashed = password.HashPassword(user, simplePassword);
                user.PasswordHash = hashed;

                var userStore = new IdentityAuthUserStore(context);
                await userStore.CreateAsync(user);
            }

            await AssignRoles(scope.ServiceProvider, user.Email, roles);
        }

        public static async Task<IdentityResult> AssignRoles(IServiceProvider services, string email, string[] roles)
        {
            var userManager = services.GetService<UserManager<IdentityAuthUser>>();
            var user = await userManager.FindByEmailAsync(email);
            var result = await userManager.AddToRolesAsync(user, roles);

            return result;
        }
    }
}