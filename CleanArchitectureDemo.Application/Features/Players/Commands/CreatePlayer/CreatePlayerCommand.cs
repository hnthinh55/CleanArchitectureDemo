using AutoMapper;
using CleanArchitectureDemo.Application.Common.Mappings;
using CleanArchitectureDemo.Application.Interfaces.Repositories;
using CleanArchitectureDemo.Domain.Entities;
using CleanArchitectureDemo.Shared;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.Features.Players.Commands.CreatePlayer
{
    public record CreatePlayerCommand : IRequest<Result<int>>, IMapFrom<Player>
    {
        public string Name { get; set; }
        public int ShirtNo { get; set; }
        public string PhotoUrl { get; set; }
        public DateTime BirthDate { get; set; }
    }

    internal class CreatePlayerCommandHandler : IRequestHandler<CreatePlayerCommand, Result<int>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public CreatePlayerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<Result<int>> Handle(
            CreatePlayerCommand command,
            CancellationToken cancellationToken
        )
        {
            var player = new Player()
            {
                Name = command.Name,
                ShirtNo = command.ShirtNo,
                PhotoUrl = command.PhotoUrl,
                BirthDate = command.BirthDate,
            };
            await unitOfWork.Repository<Player>().AddAsync(player);
            player.AddDomainEvent(new PlayerCreatedEvent(player));
            await unitOfWork.Save(cancellationToken);
            return await Result<int>.SuccessAsync(player.Id, "Player Created");
        }
    }
}
