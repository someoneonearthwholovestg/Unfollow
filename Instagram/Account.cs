using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

using Madamin.Unfollow.Instagram.API;

namespace Madamin.Unfollow.Instagram
{
    public class Account
    {
        internal Account()
        {
        }

        internal async Task LoginAsync(string username, string password)
        {
            // no need to authenticate again
            if (_api.IsUserAuthenticated) return;

            await _api.LoginAsync(username, password);
        }

        internal async Task LogoutAsync()
        {
            if (!_api.IsUserAuthenticated)
                throw new UserNotAuthenticatedException();

            await _api.LogoutAsync();
        }

        internal void SaveState(string path)
        {
            if (!_api.IsUserAuthenticated)
                throw new UserNotAuthenticatedException();

            using (var file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                new BinaryFormatter().Serialize(file, _api.State);
            }
        }

        internal void LoadState(string path)
        {
            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                _api.State = new BinaryFormatter().Deserialize(file);
            }
        }

        internal void SaveCache(string path)
        {
            if (Data == null)
                throw new AccountDataNotAvailableException();

            using (var file = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                new BinaryFormatter().Serialize(file, Data);
            }
        }

        internal void LoadCache(string path)
        {
            if (!_api.IsUserAuthenticated)
                throw new UserNotAuthenticatedException();

            using (var file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                Data = (DataCache)new BinaryFormatter().Deserialize(file);
            }
        }

        public async Task RefreshAsync()
        {
            if (!_api.IsUserAuthenticated)
                throw new UserNotAuthenticatedException();

            var followings = await _api.GetFollowingsAsync(User.Id);
            var followers = await _api.GetFollowersAsync(User.Id);

            Data = new DataCache(followers, followings);
        }

        public async Task UnfollowAsync(User user)
        {
            await _api.UnfollowAsync(user.Id);
            Data.Followings.Remove(user);
        }

        public override bool Equals(object obj)
        {
            return obj is Account account &&
                   EqualityComparer<User>.Default.Equals(User, account.User);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(User);
        }

        public User User => _api.User;

        public DataCache Data { get; private set; }

        private InstaApi _api = new InstaApi();

        [Serializable]
        public class DataCache
        {
            public DataCache(
                IEnumerable<User> followers,
                IEnumerable<User> followings)
            {
                Followers = followers.ToList();
                Followings = followings.ToList();
            }

            public List<User> Followers { get; }
            public List<User> Followings { get; }

            public IEnumerable<User> Unfollowers => Followings.Except(Followers);
        }
    }

    public class AccountDataNotAvailableException : Exception { }
}