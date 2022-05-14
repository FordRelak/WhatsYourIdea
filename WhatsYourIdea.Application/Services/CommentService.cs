using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WhatsYourIdea.Applications.Auth;
using WhatsYourIdea.Applications.Services.Exceptions;

namespace WhatsYourIdea.Applications.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<Idea> _ideaRepository;
        private readonly IUserService _userService;

        public CommentService(IRepository<Comment> commentRepository,
                              IRepository<Idea> ideaRepository,
                              IUserService userService)
        {
            _commentRepository = commentRepository;
            _ideaRepository = ideaRepository;
            _userService = userService;
        }

        public async Task AddCommentToIdea(string hash, string text, string name)
        {
            var user = await _userService.GetUserAsync(name);

            _ = user ?? throw new NotFoundException();

            var idea = await _ideaRepository.GetOneAsync(
                x => x.Hash == hash && !x.IsPrivate && x.IsVerifed,
                x => x.Include(x => x.Comments)
                );

            _ = idea ?? throw new NotFoundException();

            var comment = new Comment()
            {
                Text = text,
                Idea = idea,
                User = user.UserProfile
            };

            await _commentRepository.AddOrUpdateAsync(comment);
        }
    }
}