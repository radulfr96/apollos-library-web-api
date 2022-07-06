using ApollosLibrary.Application.Interfaces;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApollosLibrary.Infrastructure.Services
{
    public class DateTimeService : IDateTimeService
    {
        private IClock _clock;

        public DateTimeService(IClock clock)
        {
            _clock = clock;
        }

        public LocalDateTime Now
        {
            get
            {
                var instant = _clock.GetCurrentInstant();
                return instant.InUtc().LocalDateTime;
            }
        }
    }
}
