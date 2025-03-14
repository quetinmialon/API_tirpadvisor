using AutoMapper;
using Microsoft.EntityFrameworkCore;
using tripAdvisorAPI.DTO;

namespace tripAdvisorAPI.Services;

public class UserService(IMapper mapper, AppDbContext context)
{
    private readonly IMapper _mapper = mapper;
    private readonly AppDbContext _context = context;

    public async Task<UserDTO?> GetUserById(int userId)
    {
        var user = await _context.Users
            .AsNoTracking() // Optimisation : Empêche le tracking inutile
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return null; // Retourne null si aucun utilisateur n'est trouvé
        }

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<List<UserDTO>> GetAllUsers()
    {
        var users = await _context.Users
            .AsNoTracking()
            .ToListAsync();

        return _mapper.Map<List<UserDTO>>(users);
    }

}