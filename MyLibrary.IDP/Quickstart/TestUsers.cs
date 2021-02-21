// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using IdentityServer4;

namespace IdentityServerHost.Quickstart.UI
{
    public class TestUsers
    {
        public static List<TestUser> Users
        {
            get
            {                
                return new List<TestUser>
                {
                    new TestUser
                    {
                        SubjectId = "1",
                        Username = "Wanda",
                        Password = "password",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.GivenName, "Wanda"),
                            new Claim(JwtClaimTypes.FamilyName, "Maximoff"),
                        }
                    },
                    new TestUser
                    {
                        SubjectId = "2",
                        Username = "Vision",
                        Password = "password",
                        Claims =
                        {
                            new Claim(JwtClaimTypes.GivenName, "Vision"),
                            new Claim(JwtClaimTypes.FamilyName, "Jarvis"),
                        }
                    }
                };
            }
        }
    }
}