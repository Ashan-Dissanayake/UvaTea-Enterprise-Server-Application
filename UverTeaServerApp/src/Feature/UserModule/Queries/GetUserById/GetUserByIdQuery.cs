using MediatR;
using UverTeaServerApp.src.Feature.UserModule.Models.Dtos;

namespace UverTeaServerApp.src.Feature.UserModule.Queries.GetUserById;

public record GetUserByIdQuery(int Id) : IRequest<UserDetailResponseDto>;
