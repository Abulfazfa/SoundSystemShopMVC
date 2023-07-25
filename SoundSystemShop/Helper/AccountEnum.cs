namespace SoundSystemShop.Helper
{
    public enum UserRegistrationResult
    {
        Success,       // User registration was successful
        InvalidInput,  // The input data provided for registration is invalid
        Failed,        // User registration failed due to an unexpected error
    }
    public enum LoginResult
    {
        Success,
        UserNotFound,
        UserLockedOut,
        InvalidCredentials
    }
}
