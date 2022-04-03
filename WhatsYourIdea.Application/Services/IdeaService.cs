using Domain;
using Domain.Entities;

namespace WhatsYourIdea.Application
{
    public class IdeaService : IIdeaService
    {
        private readonly IRepository<Idea> _ideaRepository;

        public IdeaService(IRepository<Idea> ideaRepository)
        {
            _ideaRepository = ideaRepository;
        }
    }
}
