using API.DTOs.Profile;

namespace API.Repositories.Interfaces;

public interface ILessonsRepository
{
    Task<List<LessonProgressDto>> GetLessonsForModuleAsync(int moduleId, int? userId);
}