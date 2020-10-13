using System;

namespace Madamin.Unfollow.Instagram
{
    public class InstagramException : Exception
    {
        public InstagramException(string message) : base(message) { }
    }

    public class InvalidUserException : Exception { }
    public class BadPasswordException : Exception { }
    public class TwoFactorAuthException : Exception { }
    public class ChallengeException : Exception { }
    public class LimitErrorException : Exception { }
    public class InactiveUserException : Exception { }
    public class UserNotAuthenticatedException : Exception { }
}