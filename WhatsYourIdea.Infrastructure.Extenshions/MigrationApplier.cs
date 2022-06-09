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

        private static string loremTitle = "Lorem Ipsum is simply dummy text of the printing and typesetting industry.";
        private static string loremSubTitle = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.";
        private static string loremText = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";

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
            var random = new Random();
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
            var idea4 = CreateIdea(4);
            idea4.IsPrivate = false;
            idea4.IsVerifed = true;
            var idea5 = CreateIdea(5);
            idea5.IsPrivate = false;
            idea5.IsVerifed = true;
            var idea6 = CreateIdea(6);
            idea6.IsPrivate = false;
            idea6.IsVerifed = true;
            var idea7 = CreateIdea(7);
            idea7.IsPrivate = false;
            idea7.IsVerifed = true;
            var idea8 = CreateIdea(8);
            idea8.IsPrivate = false;
            idea8.IsVerifed = true;

            db.AddRange(idea1, idea2, idea3, idea4, idea5, idea6, idea7, idea8);
            _context.SaveChanges();
            idea1.Hash = _hasherService.Encode(idea1.Id);
            idea2.Hash = _hasherService.Encode(idea2.Id);
            idea3.Hash = _hasherService.Encode(idea3.Id);
            idea4.Hash = _hasherService.Encode(idea4.Id);
            idea5.Hash = _hasherService.Encode(idea5.Id);
            idea6.Hash = _hasherService.Encode(idea6.Id);
            idea7.Hash = _hasherService.Encode(idea7.Id);
            idea8.Hash = _hasherService.Encode(idea8.Id);
            _context.SaveChanges();

            Idea CreateIdea(int index)
            {
                var rnd1 = random.Next(1, _context.Set<ApplicationUser>().Count() + 1);
                var rnd2 = random.Next(_context.Set<Tag>().Count() + 1);
                return new()
                {
                    Author = _context.Set<Author>().First(x => x.Id == rnd1),
                    ShortDescription = loremSubTitle,
                    FullDesctiption = loremText,
                    Title = loremTitle,
                    Tags = _context.Set<Tag>().Take(rnd2).ToList(),
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
            user2.UserProfile.Author = new Author();
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