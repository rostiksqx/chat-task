namespace ChatTask.Data;

public static class DbContextHelperExtension
{
    public static async Task<Exception?> SafeSave(this ChatDbContext context)
    {
        try
        {
            await context.SaveChangesAsync();
            return null; // No exception, operation successful
        }
        catch (Exception ex)
        {
            return ex; // Return the caught exception
        }
    }
}