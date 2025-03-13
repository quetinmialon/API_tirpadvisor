using tripAdvisorAPI.Models;

namespace tripAdvisorAPI.Initializer;

public class DbInitializer
{
    public static void initialize(AppDbContext context)
    {
        
        context.Database.EnsureCreated();
        
        //dummy data

        if (!context.Users.Any())
        {
            var user = new User[]
            {
                new() { FirebaseUid = "uuid", Email = "admin@admin.com", FirstName = "Quentin Mialon"}
            };
            context.Users.AddRange(user);
            context.SaveChanges();
        }
    }
}