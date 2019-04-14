﻿using System;
using System.Collections.Generic;
using System.Text;
using DbContract.Entities;

namespace DbContract.RepositoryContract
{
    public interface IRepository : IDisposable
    {
        string AddUser(User user);
        IEnumerable<User> GetAllUsers();
        User GetUserByEmail(string email);
        string AddPhoto(Photo photo);
        string GetPasswordHash(string email);
        void RemoveLoggedUser(string email);
        void AddLoggedUser(LoggedUser user);
        string GetActiveToken(string email);
        IEnumerable<Photo> GetUserPhotos(User user);
    }
}