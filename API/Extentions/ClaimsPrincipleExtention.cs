using System;
using System.Security.Authentication;
using System.Security.Claims;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions;

public static class ClaimsPrincipleExtention
{
   public static async Task<AppUser> GetUserByEmail(
    this UserManager<AppUser> userManager, 
     ClaimsPrincipal user)
     {
        var userToReturn = await userManager.Users.FirstOrDefaultAsync(x =>
        x.Email == user.GetEmail())?? throw new AuthenticationException("user not found");
        return userToReturn;
     }

     public static async Task<AppUser> GetUserByEmailWithAddress(
     this UserManager<AppUser> userManager, 
     ClaimsPrincipal user)
     {
        var userToReturn = await userManager.Users.Include(x =>x.Address).FirstOrDefaultAsync(x =>
        x.Email == user.GetEmail())?? throw new AuthenticationException("user not found");
        return userToReturn;
     }

     public static string GetEmail (this ClaimsPrincipal user){
        var email= user.FindFirstValue(ClaimTypes.Email)??
          throw new AuthenticationException("email claim not found");
       
          return email;
     }
}
