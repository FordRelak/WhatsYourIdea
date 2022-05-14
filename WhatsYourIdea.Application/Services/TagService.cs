using AutoMapper;
using Domain;
using Domain.Entities;
using WhatsYourIdea.Applications.DTO;

namespace WhatsYourIdea.Applications.Services
{
    public class TagService : ITagService
    {
        private readonly IRepository<Tag> _tagRepository;
        private readonly IMapper _mapper;

        public TagService(IRepository<Tag> tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public IEnumerable<TagDto> GetTags()
        {
            var tags = _tagRepository.Get();
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }

        public async Task<IEnumerable<TagDto>> GetTagsExceptIdsAsync(int[] ids)
        {
            var tags = await _tagRepository.GetAsync(x => !ids.Contains(x.Id));
            return _mapper.Map<IEnumerable<TagDto>>(tags);
        }
    }
}