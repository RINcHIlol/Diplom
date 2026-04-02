using API.Data;
using API.DTOs.Profile;
using API.Models;
using API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace API.Repositories;

public class ModulesRepository : IModulesRepository
{
    private readonly AppDbContext _context;
    public ModulesRepository(AppDbContext context) => _context = context;

    public async Task<List<ModuleProgressDto>> GetModelsForCourseAsync(int? courseId)
    {
        return null;
    }
}