﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using server_api;
using server_api.Models;

namespace server_api
{
    public class UserRepository : IDisposable
    {
        private ApplicationContext _ctx;

        private UserManager<User> _userManager;
        private UserStore<User> _userStore;

        public UserRepository()
        {
            _ctx = new ApplicationContext();
            _userStore = new UserStore<User>(_ctx);
            _userManager = new UserManager<User>(_userStore);
        }

        public UserRepository(ApplicationContext ctx)
        {
            _userStore = new UserStore<User>(ctx);
            _userManager = new UserManager<User>(_userStore);
        }

        public async Task<IdentityResult> RegisterUser(RegisterUser registration)
        {
            User user = new User
            {
                UserName = registration.Email,
                Email = registration.Email,

            };
            var result = await _userManager.CreateAsync(user, registration.Password);

            return result;
        }

        public async Task<User> FindUserById(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            return user;
        }

        public async Task<User> FindUser(string userName, string password)
        {
            User user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public User UpdateUser(string id, UserProfile user)
        {
            User existing =  _userManager.FindById(id);
            if (existing == null)
            {
                return null;
            }

            existing.FirstName = user.FirstName;
            existing.LastName = user.LastName;
            existing.Email = user.Email;
            existing.UserName = user.Email;
            IdentityResult result = _userManager.Update(existing);
            if (result.Succeeded)
            {
                return existing;
            }
            return null;

        }

        public async Task<bool> UpdateUserPassword(string id, string password)
        {
            User existing = await _userManager.FindByIdAsync(id);
            if (existing == null)
            {
                return false;
            }
            string hash = _userManager.PasswordHasher.HashPassword(password);
            await _userStore.SetPasswordHashAsync(existing,hash);
            IdentityResult result = await _userManager.UpdateAsync(existing);
            if (result.Succeeded)
            {
                return true;
            }
            return false;
            

        }

        public Boolean IsValidPreferences(String mapMode, String downloadFormat)
        {
            switch (mapMode.ToUpper())
            {
                case "LIGHT":
                    break;
                case "DARK":
                    break;
                case "SATELLITE":
                    break;
                default:
                    return false;
            }

            switch (downloadFormat.ToUpper())
            {
                case "CSV":
                    break;
                case "JSON":
                    break;
                default:
                    return false;
            }

            return true;
        }

        public UserPreferences GetUserPreferences(string id)
        {
            UserPreferences data = _ctx.UserPreferences
                                        .Where(u => id.Equals(u.User_Id))
                                        .FirstOrDefault();

            return data;
        }

        public UserPreferences UpdateUserPreferences(String id, String mapMode, String downloadFormat, String stationId, String[] parameters)
        {
            var existingPreferences = _ctx.UserPreferences.Include("DefaultParameters")
                                            .Single(u => id == u.User_Id);

            existingPreferences.DefaultMapMode = mapMode;
            existingPreferences.DefaultDownloadFormat = downloadFormat;
            existingPreferences.DefaultStationId = stationId;

            _ctx.Configuration.AutoDetectChangesEnabled = false;
            existingPreferences.DefaultParameters.Clear();

            // Find the existing parameters in station
            Dictionary<string, Parameter> existingParameters = new Dictionary<string, Parameter>();
            foreach (Parameter p in _ctx.Parameters.ToList())
            {
                existingParameters.Add(p.Name + " " + p.Unit, p);
            }

            // Expects the Parameter List to be SPACE separated string "NAME UNIT" eg. "PM2.5 UG/M3"
            foreach (var p in parameters)
            {
                Parameter newParameter = null;
                existingParameters.TryGetValue(p, out newParameter);
                existingPreferences.DefaultParameters.Add(newParameter);
            }

            _ctx.Configuration.AutoDetectChangesEnabled = true;
            _ctx.SaveChanges();

            return existingPreferences;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}
