using API.DTOs.Profile;
using API.Repositories.Interfaces;
using API.Services.Interfaces;

namespace API.Services;

public class LessonsService : ILessonsService
{
    private readonly ILessonsRepository _repository;
    public LessonsService(ILessonsRepository repository) => _repository = repository;

    public async Task<List<LessonProgressDto>> GetLessonsAsync(int moduleId, int? userId)
    {
        return await _repository.GetLessonsForModuleAsync(moduleId, userId);
    }
}