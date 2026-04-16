using API.DTOs.Profile;
using API.Models;

namespace API.Services.Interfaces;

public interface ILessonsService
{
    Task<List<LessonProgressDto>> GetLessonsAsync(int moduleId, int? userId);
    Task<List<LessonShortDTO>> GetLessonsForCreatorAsync(int moduleId);
    Task<LessonShortDTO> GetLessonAsync(int lessonId);
    Task<Lesson> CreateLessonAsync(CreateUpdateLessonDTO dto, int moduleId);
    Task<bool> UpdateLessonAsync(int lessonId, CreateUpdateLessonDTO dto);
    Task<bool> IsOwnerAsync(int lessonId, int userId);
    Task<bool> DeleteAsync(int lessonId);
}