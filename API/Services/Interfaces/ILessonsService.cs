using API.DTOs.Profile;

namespace API.Services.Interfaces;

public interface ILessonsService
{
    Task<List<LessonProgressDto>> GetLessonsAsync(int moduleId, int? userId);
}