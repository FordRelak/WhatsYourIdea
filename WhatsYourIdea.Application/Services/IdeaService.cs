using AutoMapper;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WhatsYourIdea.Applications.Auth;
using WhatsYourIdea.Applications.DTO;
using WhatsYourIdea.Applications.Hasher;

namespace WhatsYourIdea.Applications.Services
{
    public class IdeaService : IIdeaService
    {
        private readonly HasherService _hasherService;
        private readonly IRepository<Idea> _ideaRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IUserService _userService;

        public IdeaService(
            IRepository<Idea> ideaRepository,
            IUserService userService,
            IMapper mapper,
            IRepository<Tag> tagRepository,
            HasherService hasherService)
        {
            _ideaRepository = ideaRepository;
            _userService = userService;
            _mapper = mapper;
            _tagRepository = tagRepository;
            _hasherService = hasherService;
        }

        public async Task<IEnumerable<IdeaDto>> GetIdeasAsync(string option, string username = null)
        {
            var ideas = new List<Idea>();

            ideas = option switch
            {
                "popular" => (await GetPopularIdeas()).ToList(),
                _ => (await GetNewIdeas()).ToList(),
            };

            var dtos = _mapper.Map<List<IdeaDto>>(ideas);

            if(username is not null)
            {
                var user = await _userService.GetUserAsync(username);
                SetTrackedIdeas(dtos, user);
            }

            return dtos;
        }

        private static void SetTrackedIdeas(List<IdeaDto> dtos, Infrastructure.Identity.ApplicationUser user)
        {
            var trackedIds = user.UserProfile.TrackedIdeas.Select(x => x.Id);
            dtos.ForEach(x =>
            {
                if(trackedIds.Contains(x.Id))
                    x.IsTracked = true;
            });
        }

        public async Task SaveIdea(CreateIdeaDto ideaDto, CancellationToken cancellationToken)
        {
            if(ideaDto.HashId is null)
            {
                await AddIdeaAsync(ideaDto, cancellationToken);
            }
            else
            {
                UpdateIdea(ideaDto, cancellationToken);
            }
        }

        public async Task<IEnumerable<IdeaDto>> GetIdeasByUser(string username)
        {
            var user = await _userService.GetUserAsync(username);

            if(user.UserProfile.Author is not null)
            {
                var ideas = (await _ideaRepository.GetAsync(
                    x => x.Author.Id == user.UserProfile.Author.Id,
                    x => x.Include(x => x.Author),
                    x => x.OrderByDescending(x => x.Created)
                    ));
                var dtos = _mapper.Map<List<IdeaDto>>(ideas);
                SetTrackedIdeas(dtos, user);
            }

            return Array.Empty<IdeaDto>();
        }

        private async Task AddIdeaAsync(CreateIdeaDto ideaDto, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserAsync(ideaDto.UserName);

            user.UserProfile.Author ??= new Author();

            var newIdea = _mapper.Map<Idea>(ideaDto);
            user.UserProfile.Author.Ideas.Add(newIdea);
            await _ideaRepository.AddOrUpdateAsync(newIdea);

            newIdea.Tags = await GetTagsAsync(ideaDto.NewTags, ideaDto.TagIds);
            newIdea.Hash = _hasherService.Encode(newIdea.Id);
            await _ideaRepository.AddOrUpdateAsync(newIdea);
        }

        private async Task<IEnumerable<Idea>> GetNewIdeas()
        {
            return await _ideaRepository.GetAsync(x => x.IsVerifed && !x.IsPrivate,
                x => x.Include(x => x.TrackingUsers)
                .Include(x => x.Tags)
                .Include(x => x.Comments),
                x => x.OrderByDescending(x => x.TrackingUsers.Count));
        }

        private async Task<IEnumerable<Idea>> GetPopularIdeas()
        {
            return await _ideaRepository.GetAsync(x => x.IsVerifed && !x.IsPrivate,
                x => x.Include(x => x.TrackingUsers)
                .Include(x => x.Tags)
                .Include(x => x.Comments),
                x => x.OrderByDescending(x => x.Created));
        }

        private async Task<IList<Tag>> GetTagsAsync(string newTags, int[] tagIds)
        {
            var tags = new List<Tag>();
            if(!string.IsNullOrEmpty(newTags))
            {
                var splitedTag = newTags.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach(var tag in splitedTag)
                {
                    var dbTag = await _tagRepository.GetOneAsync(x => x.Name == tag);
                    if(dbTag != null)
                    {
                        tags.Add(dbTag);
                    }
                    else
                    {
                        tags.Add(new Tag
                        {
                            Name = tag.Trim("#$%^&*()+=-[]';,./{}|:<>?~".ToCharArray()),
                        });
                    }
                }
            }

            var dbTags = await _tagRepository.GetAsync(x => tagIds.Contains(x.Id));
            tags.AddRange(dbTags.ToList());
            tags = tags.DistinctBy(x => x.Name).ToList();

            return tags;
        }

        private void UpdateIdea(CreateIdeaDto ideaDto, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}