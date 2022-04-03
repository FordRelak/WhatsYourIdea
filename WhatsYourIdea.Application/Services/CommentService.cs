using Domain;
using Domain.Entities;

namespace WhatsYourIdea.Application
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
