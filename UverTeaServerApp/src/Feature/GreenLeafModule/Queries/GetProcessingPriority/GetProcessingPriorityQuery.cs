using MediatR;
using UverTeaServerApp.src.Feature.GreenLeafModule.Domain.Priority;

namespace UverTeaServerApp.src.Feature.GreenLeafModule.Queries.GetProcessingPriority;

public record GetProcessingPriorityQuery
    : IRequest<IReadOnlyList<ProcessingPriorityResult>>;