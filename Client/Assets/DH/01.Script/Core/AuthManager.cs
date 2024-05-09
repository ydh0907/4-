using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance;

    public async Task Authorization()
    {
        await UnityServices.InitializeAsync();

        for (int i = 0; i < 5; i++)
        {
            if (await TryAuth()) break;
        }
    }

    private async Task<bool> TryAuth()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            if (AuthenticationService.Instance.IsAuthorized && AuthenticationService.Instance.IsSignedIn)
            {
                return true;
            }
        }
        catch (AuthenticationException e)
        {
            Debug.LogException(e);
            return false;
        }
        catch (RequestFailedException e)
        {
            Debug.LogException(e);
            return false;
        }
        return false;
    }
}
