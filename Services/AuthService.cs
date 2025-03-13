using AutoMapper;
using FirebaseAdmin.Auth;
using tripAdvisorAPI.DTO;
using tripAdvisorAPI.Models;

namespace tripAdvisorAPI.Services;

public class AuthService(IMapper mapper, AppDbContext context)
{
    private readonly IMapper _mapper = mapper;
    private readonly AppDbContext _context = context;
    private readonly FirebaseAuth _authInterface = FirebaseAuth.DefaultInstance;

    public async Task CreateUser(CreateUserDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var user = _mapper.Map<User>(dto);
        string firebaseUid = string.Empty; // Stocker l'UID pour rollback en cas d'erreur

        try
        {
            // Créer l'utilisateur dans Firebase
            var userRecordArgs = new UserRecordArgs
            {
                Email = user.Email,
                Password = dto.Password,
                DisplayName = $"{dto.FirstName}"
            };

            var firebaseUser = await _authInterface.CreateUserAsync(userRecordArgs);
            firebaseUid = firebaseUser.Uid;
            user.FirebaseUid = firebaseUid;

            // Ajouter l'utilisateur dans la base de données locale
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        catch (FirebaseAuthException ex)
        {
            throw new InvalidOperationException($"Error creating Firebase user: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Si Firebase a créé l'utilisateur mais que la BDD échoue, rollback Firebase
            if (!string.IsNullOrEmpty(firebaseUid))
            {
                await _authInterface.DeleteUserAsync(firebaseUid);
            }

            throw new InvalidOperationException($"An error occurred while creating a user: {ex.Message}");
        }
    }
}
