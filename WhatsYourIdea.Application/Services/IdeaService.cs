using AutoMapper;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using WhatsYourIdea.Applications.Auth;
using WhatsYourIdea.Applications.DTO;
using WhatsYourIdea.Applications.Hasher;
using WhatsYourIdea.Applications.Services.Exceptions;
using WhatsYourIdea.Infrastructure.Identity;

namespace WhatsYourIdea.Applications.Services
{
    public class IdeaService : IIdeaService
    {
        private readonly HasherService _hasherService;
        private readonly IRepository<Idea> _ideaRepository;
        private readonly IMapper _mapper;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IUserService _userService;

        public IdeaService(
            IRepository<Idea> ideaRepository,
            IUserService userService,
            IMapper mapper,
            IRepository<Tag> tagRepository,
            HasherService hasherService,
            IRepository<UserProfile> userProfileRepository)
        {
            _ideaRepository = ideaRepository;
            _userService = userService;
            _mapper = mapper;
            _tagRepository = tagRepository;
            _userProfileRepository = userProfileRepository;
            _hasherService = hasherService;
        }

        public async Task DeleteIdeaAsync(string hash, string userName)
        {
            var user = await _userService.GetUserAsync(userName);

            _ = user ?? throw new NotFoundException();

            var idea = await _ideaRepository.GetOneAsync(x => x.Hash == hash, x => x.Include(x => x.Author));

            _ = idea ?? throw new NotFoundException();

            if(user.UserProfile.Author.Id != idea.Author.Id)
            {
                throw new NotHaveRightsException();
            }

            await _ideaRepository.Remove(idea);
            return;
        }

        public async Task DeleteIdeaAsync(string hash)
        {
            var idea = await _ideaRepository.GetOneAsync(x => x.Hash == hash);
            await _ideaRepository.Remove(idea);
        }

        public async Task<IdeaDetailedDto> GetIdeaForEdit(string hash, string userName)
        {
            var user = await _userService.GetUserAsync(userName);

            _ = user ?? throw new NotFoundException();

            _ = user.UserProfile.Author ?? throw new NotHaveRightsException();

            var idea = await _ideaRepository.GetOneAsync(x => x.Hash == hash,
                x => x.Include(x => x.Comments)
                        .ThenInclude(x => x.User)
                    .Include(x => x.Author)
                    .Include(x => x.Tags)
                    .Include(x => x.TrackingUsers));

            _ = idea ?? throw new NotFoundException();

            if(user.UserProfile.Author.Id != idea.Author.Id)
            {
                throw new NotHaveRightsException();
            }

            return _mapper.Map<IdeaDetailedDto>(idea);
        }

        public async Task<IEnumerable<IdeaDto>> GetIdeasAsync(string option, string username = null)
        {
            var ideas = option switch
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

        public async Task<IEnumerable<IdeaDto>> GetIdeasByTag(string tagName, string userName)
        {
            var ideas = await _ideaRepository.GetAsync(x => x.IsVerifed
                && !x.IsPrivate
                && x.Tags.Any(x => x.Name == tagName),
                    x => x.Include(x => x.TrackingUsers)
                    .Include(x => x.Tags)
                    .Include(x => x.Comments),
                    x => x.OrderByDescending(x => x.TrackingUsers.Count));

            var dtos = _mapper.Map<List<IdeaDto>>(ideas);
            if(userName is not null)
            {
                var user = await _userService.GetUserAsync(userName);

                if(user is not null)
                {
                    SetTrackedIdeas(dtos, user);
                }
            }

            return dtos;
        }

        public async Task<IEnumerable<IdeaDto>> GetIdeasByUser(string username)
        {
            var user = await _userService.GetUserAsync(username);

            if(user.UserProfile.Author is not null)
            {
                var ideas = await _ideaRepository.GetAsync(
                        x => x.Author.Id == user.UserProfile.Author.Id,
                        x => x.Include(x => x.Author),
                        x => x.OrderByDescending(x => x.Created)
                    );
                var dtos = _mapper.Map<List<IdeaDto>>(ideas);
                SetTrackedIdeas(dtos, user);
                return dtos;
            }

            return Array.Empty<IdeaDto>();
        }

        public async Task<IEnumerable<IdeaDto>> GetPrivateIdeas(string userName)
        {
            var user = await _userService.GetUserAsync(userName);

            if(user.UserProfile.Author is not null)
            {
                var ideas = await _ideaRepository.GetAsync(
                        x => x.Author.Id == user.UserProfile.Author.Id && x.IsPrivate,
                        x => x
                            .Include(x => x.Author)
                            .Include(x => x.TrackingUsers)
                            .Include(x => x.Tags)
                            .Include(x => x.Comments),
                        x => x.OrderByDescending(x => x.Created)
                    );

                var dtos = _mapper.Map<List<IdeaDto>>(ideas);
                SetTrackedIdeas(dtos, user);
                return dtos;
            }

            return Array.Empty<IdeaDto>();
        }

        public async Task<IdeaDetailedDto> GetPublicIdeaByHash(string hash, string userName)
        {
            var idea = await _ideaRepository.GetOneAsync(
                x => x.Hash == hash && !x.IsPrivate && x.IsVerifed,
                x => x.Include(x => x.Comments)
                        .ThenInclude(x => x.User)
                    .Include(x => x.Author)
                    .Include(x => x.Tags)
                    .Include(x => x.TrackingUsers));

            _ = idea ?? throw new NotFoundException();

            var dto = _mapper.Map<IdeaDetailedDto>(idea);

            if(userName is not null)
            {
                var user = await _userService.GetUserAsync(userName);

                if(user is not null)
                {
                    SetTrackedIdeas(dto, user);
                }
            }

            return dto;
        }

        public async Task<IEnumerable<IdeaDto>> GetPublishedIdeas(string userName)
        {
            var user = await _userService.GetUserAsync(userName);

            if(user.UserProfile.Author is not null)
            {
                var ideas = await _ideaRepository.GetAsync(
                        x => x.Author.Id == user.UserProfile.Author.Id && x.IsVerifed,
                        x => x
                            .Include(x => x.Author)
                            .Include(x => x.TrackingUsers)
                            .Include(x => x.Tags)
                            .Include(x => x.Comments),
                        x => x.OrderByDescending(x => x.Created)
                    );

                var dtos = _mapper.Map<List<IdeaDto>>(ideas);
                SetTrackedIdeas(dtos, user);
                return dtos;
            }

            return Array.Empty<IdeaDto>();
        }

        public async Task<IEnumerable<IdeaDto>> GetTrackedIdeas(string userName)
        {
            var user = await _userService.GetUserAsync(userName);

            var userProfile = await _userProfileRepository.GetOneAsync(
                    x => x.Id == user.UserProfile.Id,
                    x => x
                        .Include(y => y.TrackedIdeas)
                            .ThenInclude(x => x.TrackingUsers)
                        .Include(y => y.TrackedIdeas)
                            .ThenInclude(x => x.Tags)
                        .Include(y => y.TrackedIdeas)
                            .ThenInclude(x => x.Comments)
                        .Include(y => y.TrackedIdeas)
                            .ThenInclude(x => x.Author)
                );

            var dtos = _mapper.Map<List<IdeaDto>>(userProfile.TrackedIdeas);

            SetTrackedIdeas(dtos, user);

            return dtos;
        }

        public async Task<IEnumerable<IdeaDto>> GetUnPublicIdeas()
        {
            var ideas = await _ideaRepository.GetAsync(
                x => !x.IsPrivate && !x.IsVerifed,
                x =>
                    x.Include(x => x.Author)
                    .Include(x => x.Comments)
                    .Include(x => x.TrackingUsers)
                    .Include(x => x.Tags),
                x => x.OrderBy(x => x.IsVerifed)
                );

            return _mapper.Map<List<IdeaDto>>(ideas);
        }

        public async Task PublicIdeaAsync(string hash, string name)
        {
            var idea = await _ideaRepository.GetOneAsync(x => x.Hash == hash);

            _ = idea ?? throw new NotFoundException();

            var user = await _userService.GetUserAsync(name);

            _ = user ?? throw new NotFoundException();

            _ = user.UserProfile.Author ?? throw new NotHaveRightsException();

            if(user.UserProfile.Author.Id != idea.Author.Id)
            {
                throw new NotHaveRightsException();
            }

            idea.IsPrivate = false;
            idea.IsVerifed = false;

            await _ideaRepository.AddOrUpdateAsync(idea);
            return;
        }

        public async Task SaveIdea(CreateIdeaDto ideaDto, CancellationToken cancellationToken)
        {
            if(ideaDto.HashId is null)
            {
                await AddIdeaAsync(ideaDto, cancellationToken);
            }
            else
            {
                await UpdateIdeaAsync(ideaDto, cancellationToken);
            }
        }

        public async Task<IEnumerable<IdeaDto>> SearchIdea(string search, string name)
        {
            var ideas = await _ideaRepository.GetAsync(x => (EF.Functions.Like(x.Title, $"%{search}%")
                || EF.Functions.Like(x.ShortDescription, $"%{search}%")
                || EF.Functions.Like(x.FullDesctiption, $"%{search}%"))
                && (x.IsVerifed && !x.IsPrivate),
                x => x.Include(x => x.TrackingUsers)
                .Include(x => x.Tags)
                .Include(x => x.Comments),
                x => x.OrderByDescending(x => x.TrackingUsers.Count));

            var dtos = _mapper.Map<List<IdeaDto>>(ideas);

            if(name is not null)
            {
                var user = await _userService.GetUserAsync(name);

                if(user is not null)
                {
                    SetTrackedIdeas(dtos, user);
                }
            }

            return dtos;
        }

        public async Task TrackIdeaAsync(string hash, string name)
        {
            var idea = await _ideaRepository.GetOneAsync(x => x.Hash == hash);
            var user = await _userService.GetUserAsync(name);

            _ = idea ?? throw new NotFoundException();
            _ = user ?? throw new NotFoundException();

            var userProfile = await _userProfileRepository.GetOneAsync(x => x.Id == user.UserProfile.Id, x => x.Include(x => x.TrackedIdeas));

            userProfile.TrackedIdeas.Add(idea);

            await _userProfileRepository.AddOrUpdateAsync(userProfile);
        }

        public async Task UnTrackIdeaAsync(string hash, string name)
        {
            var idea = await _ideaRepository.GetOneAsync(x => x.Hash == hash);
            var user = await _userService.GetUserAsync(name);

            _ = idea ?? throw new NotFoundException();
            _ = user ?? throw new NotFoundException();

            var userProfile = await _userProfileRepository.GetOneAsync(x => x.Id == user.UserProfile.Id, x => x.Include(x => x.TrackedIdeas));

            userProfile.TrackedIdeas.Remove(idea);

            await _userProfileRepository.AddOrUpdateAsync(userProfile);
        }

        public async Task VerifiedIdea(string hash)
        {
            var idea = await _ideaRepository.GetOneAsync(x => x.Hash == hash);
            idea.IsVerifed = true;
            await _ideaRepository.AddOrUpdateAsync(idea);
        }

        private static void SetTrackedIdeas(List<IdeaDto> dtos, ApplicationUser user)
        {
            var trackedIds = user.UserProfile.TrackedIdeas.Select(x => x.Id);
            dtos.ForEach(x =>
            {
                if(trackedIds.Contains(x.Id))
                    x.IsTracked = true;
            });
        }

        private static void SetTrackedIdeas(IdeaDetailedDto dto, ApplicationUser user)
        {
            var trackedIds = user.UserProfile.TrackedIdeas.Select(x => x.Id);
            if(trackedIds.Contains(dto.Id))
                dto.IsTracked = true;
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
                .Include(x => x.Comments)
                .Include(x => x.Author),
                x => x.OrderByDescending(x => x.Created));
        }

        private async Task<IEnumerable<Idea>> GetPopularIdeas()
        {
            return await _ideaRepository.GetAsync(x => x.IsVerifed && !x.IsPrivate,
                x => x.Include(x => x.TrackingUsers)
                .Include(x => x.Tags)
                .Include(x => x.Comments)
                .Include(x => x.Author),
                x => x.OrderByDescending(x => x.TrackingUsers.Count));
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
                            Name = tag.Trim("#$%^&*()+=-[]';,./{}|:<>?~@".ToCharArray()),
                        });
                    }
                }
            }

            var dbTags = await _tagRepository.GetAsync(x => tagIds.Contains(x.Id));
            tags.AddRange(dbTags.ToList());
            tags = tags.DistinctBy(x => x.Name).ToList();

            return tags;
        }

        private async Task UpdateIdeaAsync(CreateIdeaDto ideaDto, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserAsync(ideaDto.UserName);

            _ = user ?? throw new NotFoundException();
            _ = user.UserProfile.Author ?? throw new NotFoundException();

            var ideaDb = await _ideaRepository.GetOneAsync(x => x.Hash == ideaDto.HashId, x => x.Include(x => x.Tags)
                                                                                                .Include(x => x.Author));

            _ = ideaDb ?? throw new NotFoundException();

            if(user.UserProfile.Author.Id != ideaDb.Author.Id)
                throw new NotHaveRightsException();

            ideaDb.Tags = await GetTagsAsync(ideaDto.NewTags, ideaDto.TagIds);
            ideaDb.FullDesctiption = ideaDto.Text;
            ideaDb.Title = ideaDto.Title;
            ideaDb.ShortDescription = ideaDto.SubTitle;
            ideaDb.IsVerifed = false;
            ideaDb.MainImagePath = ideaDto.MainImagePath ?? ideaDb.MainImagePath;

            await _ideaRepository.AddOrUpdateAsync(ideaDb);
        }

        public async Task<IdeaDetailedDto> GetIdeaForCheck(string hash)
        {
            var idea = await _ideaRepository.GetOneAsync(
                x => x.Hash == hash,
                x =>
                    x.Include(x => x.Author)
                    .Include(x => x.Tags)
                );

            return _mapper.Map<IdeaDetailedDto>(idea);
        }
    }
}