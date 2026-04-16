using API.DTOs.Profile;
using API.Models;
using Task = System.Threading.Tasks.Task;

namespace API.Repositories.Interfaces;

public interface ILessonsRepository
{
    Task<List<LessonProgressDto>> GetLessonsForModuleAsync(int moduleId, int? userId);
    Task<List<LessonShortDTO>> GetMyLessonsAsync(int moduleId);
    Task<Lesson> CreateAsync(Lesson lesson);
    Task<int> GetMaxOrderIndex(int courseId);
    Task<Lesson?> GetByIdAsync(int id);
    Task UpdateAsync(Lesson lesson);
    Task<bool> IsOwnerAsync(int moduleId, int userId);
    Task DeleteAsync(int lessonId);
}