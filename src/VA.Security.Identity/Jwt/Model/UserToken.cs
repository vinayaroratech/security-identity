﻿using System.Collections.Generic;

namespace VA.Security.Identity.Jwt.Model
{
    public class UserToken<T>
    {
        public T Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaim> Claims { get; set; }
    }

    public class UserToken : UserToken<string>
    {

    }
}
