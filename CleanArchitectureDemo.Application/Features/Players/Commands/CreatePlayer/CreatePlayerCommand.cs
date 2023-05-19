using CleanArchitectureDemo.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDemo.Application.Features.Players.Commands.CreatePlayer
{
    public record class CreatePlayerCommand : IRequest<Results<int>>, IMapFrom<Player> { }
}
