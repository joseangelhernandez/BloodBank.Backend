using System.Net.Http.Json;
using BloodBank.Core.Exceptions;
using BloodBank.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace BloodBank.Infrastructure.Firebase;

/// <summary>
/// Llama a la API REST de Firebase Authentication (identitytoolkit.googleapis.com)
/// Documentación: https://firebase.google.com/docs/reference/rest/auth
/// </summary>
public class FirebaseAuthClient(HttpClient http, IConfiguration configuration) : IFirebaseAuthClient
{
    private const string BaseUrl = "https://identitytoolkit.googleapis.com/v1/accounts";

    // La Web API key está en appsettings.json  "Firebase:WebApiKey"
    private readonly string _apiKey = configuration["Firebase:WebApiKey"]
        ?? throw new InvalidOperationException("Missing setting Firebase:WebApiKey.");

    public async Task<FirebaseSession> SignUpAsync(string email, string password, CancellationToken ct = default)
    {
        var body = new { email, password, returnSecureToken = true };
        return await PostAndReadSessionAsync("signUp", body, ct);
    }

    public async Task<FirebaseSession> SignInAsync(string email, string password, CancellationToken ct = default)
    {
        var body = new { email, password, returnSecureToken = true };
        return await PostAndReadSessionAsync("signInWithPassword", body, ct);
    }

    public async Task SendPasswordResetEmailAsync(string email, CancellationToken ct = default)
    {
        var body = new { requestType = "PASSWORD_RESET", email };
        var response = await http.PostAsJsonAsync($"{BaseUrl}:sendOobCode?key={_apiKey}", body, ct);

        if (!response.IsSuccessStatusCode)
        {
            var code = await ReadErrorCodeAsync(response, ct);

            // Si el email no existe no lo decimos, para no revelar qué cuentas hay
            if (code == "EMAIL_NOT_FOUND") return;

            throw ToBusinessException(code);
        }
    }

    public async Task UpdateProfileAsync(string idToken, string displayName, string? photoUrl, CancellationToken ct = default)
    {
        object body = new { idToken, displayName, returnSecureToken = false };

        var response = await http.PostAsJsonAsync($"{BaseUrl}:update?key={_apiKey}", body, ct);

        if (!response.IsSuccessStatusCode)
            throw ToBusinessException(await ReadErrorCodeAsync(response, ct));
    }
    

    /// <summary>Hace el POST a Firebase y convierte la respuesta en un FirebaseSession.</summary>
    private async Task<FirebaseSession> PostAndReadSessionAsync(string action, object body, CancellationToken ct)
    {
        var response = await http.PostAsJsonAsync($"{BaseUrl}:{action}?key={_apiKey}", body, ct);

        if (!response.IsSuccessStatusCode)
            throw ToBusinessException(await ReadErrorCodeAsync(response, ct));

        var data = await response.Content.ReadFromJsonAsync<FirebaseAuthResponse>(ct)
            ?? throw new InvalidOperationException("Empty response from Firebase.");

        return new FirebaseSession(data.LocalId, data.Email, data.IdToken, int.Parse(data.ExpiresIn));
    }

    /// <summary>
    /// Firebase responde errores así: { "error": { "message": "EMAIL_EXISTS" } }.
    /// A veces trae detalle: "WEAK_PASSWORD : Password should be at least 6 characters".
    /// </summary>
    private static async Task<string> ReadErrorCodeAsync(HttpResponseMessage response, CancellationToken ct)
    {
        var error = await response.Content.ReadFromJsonAsync<FirebaseErrorResponse>(ct);
        return error?.Error?.Message ?? "UNKNOWN_ERROR";
    }

    /// <summary>Traduce el código de Firebase al mensaje y código HTTP que ve la app.</summary>
    private static BusinessException ToBusinessException(string firebaseMessage)
    {
        if (firebaseMessage == "EMAIL_EXISTS")
            return new BusinessException("The account already exists for that email.", 409);

        if (firebaseMessage is "INVALID_LOGIN_CREDENTIALS" or "EMAIL_NOT_FOUND" or "INVALID_PASSWORD")
            return new BusinessException("Invalid email or password.", 401);

        if (firebaseMessage.StartsWith("WEAK_PASSWORD"))
            return new BusinessException("The password provided is too weak.", 400);

        if (firebaseMessage is "INVALID_ID_TOKEN" or "TOKEN_EXPIRED" or "USER_NOT_FOUND")
            return new BusinessException("Your session expired. Please log in again.", 401);

        return new BusinessException(firebaseMessage, 400);
    }

    // Forma del JSON que devuelve Firebase (solo los campos que usamos).
    private record FirebaseAuthResponse(string LocalId, string Email, string IdToken, string ExpiresIn);
    private record FirebaseErrorResponse(FirebaseError? Error);
    private record FirebaseError(int Code, string Message);
}
