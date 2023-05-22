using AutoMapper;
using AutoMapper.QueryableExtensions;

using CleanArchitectureDemo.Application.Extensions;
using CleanArchitectureDemo.Application.Interfaces.Repositories;
using CleanArchitectureDemo.Domain.Entities;
using CleanArchitectureDemo.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.Features.Players.Queries.GetPlayersWithPagination
{
    public record GetPlayersWithPaginationQuery
        : IRequest<PaginatedResult<GetPlayersWithPaginationDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetPlayersWithPaginationQuery() { }

        public GetPlayersWithPaginationQuery(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }
    }

    internal class GetPlayerWithPaginationQueyHandler
        : IRequestHandler<
            GetPlayersWithPaginationQuery,
            PaginatedResult<GetPlayersWithPaginationDto>
        >
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetPlayerWithPaginationQueyHandler(IUnitOfWork unitOfWork, IMapper maper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = maper;
        }

        public async Task<PaginatedResult<GetPlayersWithPaginationDto>> Handle(
            GetPlayersWithPaginationQuery query,
            CancellationToken cancellationToken
        )
        {
            return await unitOfWork
                .Repository<Player>()
                .Entities.OrderBy(x => x.Name)
                .ProjectTo<GetPlayersWithPaginationDto>(mapper.ConfigurationProvider)
                .ToPaginatedListAsync(query.PageNumber, query.PageSize, cancellationToken);
        }
    }
}
