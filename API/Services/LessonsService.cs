using API.DTOs.Profile;
using API.Models;
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
    
    public async Task<List<LessonShortDTO>> GetLessonsForCreatorAsync(int moduleId)
    {
        return await _repository.GetMyLessonsAsync(moduleId);
    }
    
    public async Task<LessonShortDTO> GetLessonAsync(int lessonId)
    {
        var lesson = await _repository.GetByIdAsync(lessonId);
        
        var lessonDto = new LessonShortDTO()
        {
            Id = lesson.Id,
            Title = lesson.Title,
            Content = lesson.Content,
            OrderIndex = lesson.OrderIndex,
        };
        return lessonDto;
    }
    
    public async Task<Lesson> CreateLessonAsync(CreateUpdateLessonDTO dto, int moduleId)
    {
        var orderIndex = await _repository.GetMaxOrderIndex(moduleId);
        var lesson = new Lesson
        {
            Title = dto.Title,
            Content = dto.Content,
            OrderIndex = orderIndex,
            ModuleId = moduleId,
        };

        return await _repository.CreateAsync(lesson);
    }

    public async Task<bool> UpdateLessonAsync(int lessonId, CreateUpdateLessonDTO dto)
    {
        var lesson = await _repository.GetByIdAsync(lessonId);

        if (lesson == null)
            return false;

        lesson.Title = dto.Title;
        lesson.Content = dto.Content;

        await _repository.UpdateAsync(lesson);

        return true;
    }

    public async Task<bool> IsOwnerAsync(int lessonId, int userId)
    {
        return await _repository.IsOwnerAsync(lessonId, userId);
    }

    public async Task<bool> DeleteAsync(int lessonId)
    {
        var lesson = await _repository.GetByIdAsync(lessonId);
        
        if (lesson == null)
            return false;
        
        await _repository.DeleteAsync(lessonId);
        return true;
    }
}