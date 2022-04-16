using Domain;
using Domain.Entities;

namespace WhatsYourIdea.Applications.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> commentRepository;

        public CommentService(IRepository<Comment> commentRepository)
        {
            this.commentRepository = commentRepository;
        }
    }
}