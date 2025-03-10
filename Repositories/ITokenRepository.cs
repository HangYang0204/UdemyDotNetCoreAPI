﻿using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ITokenRepository
    {
        string CreatJWTToken(IdentityUser user, List<string> roles);
    }
}
