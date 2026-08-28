using MediatR;
using UverTeaServerApp.Shared.Common;
using UverTeaServerApp.src.Feature.UserModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.UserModule.Queries.GetUsers;

public record GetUsersQuery(PaginationParams? Params = null) : IRequest<PagedResult<UserDetailResponseDto>>;
