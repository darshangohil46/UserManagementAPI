using UserManagementAPI.Models;

namespace UserManagementAPI.Services
{
    public class UserService
    {
        private readonly List<User> _users = new();
        private readonly Dictionary<int, User> _userById = new();
        private readonly object _sync = new();
        private int _nextId = 1;

        public IReadOnlyList<User> GetAll()
        {
            lock (_sync)
            {
                return _users
                    .Select(u => new User { Id = u.Id, Name = u.Name, Email = u.Email })
                    .ToList();
            }
        }

        public User? GetById(int id)
        {
            lock (_sync)
            {
                if (!_userById.TryGetValue(id, out var user)) return null;

                return new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                };
            }
        }

        public User Create(User user)
        {
            lock (_sync)
            {
                if (_users.Any(u => string.Equals(u.Email, user.Email, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException("A user with this email already exists.");
                }

                var newUser = new User
                {
                    Id = _nextId++,
                    Name = user.Name.Trim(),
                    Email = user.Email.Trim()
                };

                _users.Add(newUser);
                _userById[newUser.Id] = newUser;

                return new User
                {
                    Id = newUser.Id,
                    Name = newUser.Name,
                    Email = newUser.Email
                };
            }
        }

        public bool Update(int id, User updatedUser)
        {
            lock (_sync)
            {
                if (!_userById.TryGetValue(id, out var user)) return false;

                var normalizedEmail = updatedUser.Email.Trim();
                if (_users.Any(u => u.Id != id && string.Equals(u.Email, normalizedEmail, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new InvalidOperationException("A user with this email already exists.");
                }

                user.Name = updatedUser.Name.Trim();
                user.Email = normalizedEmail;
                return true;
            }
        }

        public bool Delete(int id)
        {
            lock (_sync)
            {
                if (!_userById.TryGetValue(id, out var user)) return false;

                _userById.Remove(id);
                _users.Remove(user);
                return true;
            }
        }
    }
}
