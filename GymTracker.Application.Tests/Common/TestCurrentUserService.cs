using GymTracker.Application.Common.Interfaces;

namespace GymTracker.Application.Tests.Common;

public sealed class TestCurrentUserService : ICurrentUserService
{
    public TestCurrentUserService(Guid? userId)
    {
        UserId = userId;
    }

    public Guid? UserId { get; }
}