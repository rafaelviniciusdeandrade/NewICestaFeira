using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CestaFeira.Domain.Command.Base
{
    public class BaseCommand : IRequest<CommandBaseResult>
    {

    }
}
