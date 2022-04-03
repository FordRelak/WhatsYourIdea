using Domain;
using Domain.Entities;

namespace WhatsYourIdea.Application
{
    public class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _authorRepository;

        public AuthorService(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }
    }
}
