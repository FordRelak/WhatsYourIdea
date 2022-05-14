using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WhatsYourIdea.Applications.Hasher;
using WhatsYourIdea.Infrastructure.Identity;

namespace WhatsYourIdea.Infrastructure.Extensions
{
    public static class MigrationApplier
    {
        private static IServiceScope _serviceScope;
        private static EfDbContext _context;
        private static HasherService _hasherService;

        public static IHost ApplyMigrations(this IHost host, bool isSeed)
        {
            using var scope = host.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<EfDbContext>();
            _hasherService = scope.ServiceProvider.GetRequiredService<HasherService>();
            _serviceScope = scope;
            _context = context;
            context.Database.Migrate();
            if(isSeed)
            {
                SeedData();
            }
            return host;
        }

        private static void SeedData()
        {
            AddUsers();
            AddRoles().Wait();
            AddTags();
            AddIdeas();
            AddComments();
            AddTracked();
        }

        private static async Task AddRoles()
        {
            var roleManager = _serviceScope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = _serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            await roleManager.CreateAsync(new ApplicationRole()
            {
                Name = "admin"
            });
            var user = await userManager.FindByNameAsync("user_1");
            await userManager.AddToRoleAsync(user, "admin");
        }

        private static void AddTracked()
        {
            var userProfile1 = _context.UserProfiles.First(x => x.Id == 1);
            var userProfile2 = _context.UserProfiles.First(x => x.Id == 2);

            var idea1 = _context.Ideas.First(x => x.Id == 1);
            var idea2 = _context.Ideas.First(x => x.Id == 2);

            userProfile1.TrackedIdeas.Add(idea1);
            userProfile1.TrackedIdeas.Add(idea2);

            userProfile2.TrackedIdeas.Add(idea2);

            _context.SaveChanges();
        }

        private static void AddComments()
        {
            var db = _context.Set<Comment>();

            var comment1 = CreateComment(1, 2, 1);
            var comment2 = CreateComment(2, 2, 2);
            var comment3 = CreateComment(3, 1, 1);

            db.AddRange(comment1, comment2, comment3);

            _context.SaveChanges();

            static Comment CreateComment(int index, int user, int idea)
            {
                return new()
                {
                    Text = $"comment_{index}",
                    Idea = _context.Set<Idea>().First(x => x.Id == idea),
                    User = _context.Set<UserProfile>().First(x => x.Id == user)
                };
            }
        }

        private static void AddIdeas()
        {
            var db = _context.Set<Idea>();

            var idea1 = CreateIdea(1);
            idea1.IsPrivate = true;
            idea1.IsVerifed = false;
            var idea2 = CreateIdea(2);
            idea2.IsPrivate = false;
            idea2.IsVerifed = true;
            var idea3 = CreateIdea(3);
            idea3.IsPrivate = false;
            idea3.IsVerifed = false;

            db.AddRange(idea1, idea2, idea3);
            _context.SaveChanges();
            idea1.Hash = _hasherService.Encode(idea1.Id);
            idea2.Hash = _hasherService.Encode(idea2.Id);
            idea3.Hash = _hasherService.Encode(idea3.Id);
            _context.SaveChanges();

            static Idea CreateIdea(int index)
            {
                return new()
                {
                    Author = _context.Set<Author>().First(x => x.Id == 1),
                    ShortDescription = $"short_{index}",
                    FullDesctiption = $"full_{index}",
                    Title = $"title_{index}",
                    Tags = _context.Set<Tag>().ToList(),
                };
            }
        }

        private static void AddUsers()
        {
            var usermanager = _serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var user1 = CreateUser(1);
            user1.UserProfile.Author = new Author();
            user1.UserProfile.UserName = "user_1";
            var user2 = CreateUser(2);
            user2.UserProfile.UserName = "user_2";
            usermanager.CreateAsync(user1, "123").Wait();
            usermanager.CreateAsync(user2, "1234").Wait();

            static ApplicationUser CreateUser(int index)
            {
                return new ApplicationUser()
                {
                    UserName = $"user_{index}",
                    Email = $"user{index}@user{index}",
                    UserProfile = new(),
                };
            }
        }

        private static void AddTags()
        {
            var tagDb = _context.Set<Tag>();
            var tag1 = CreateTag(1);
            var tag2 = CreateTag(2);
            var tag3 = CreateTag(3);

            tagDb.AddRange(tag1, tag2, tag3);
            _context.SaveChanges();

            static Tag CreateTag(int index)
            {
                return new Tag { Name = $"tag_{index}" };
            }
        }
    }
}