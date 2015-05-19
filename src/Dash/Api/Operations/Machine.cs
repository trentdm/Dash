using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.FluentValidation;

namespace Dash.Api.Operations
{
    [Route("/api/machine", "GET POST")]
    [Route("/api/machine/{id}", "GET PUT")]
    [Authenticate(ApplyTo.Post | ApplyTo.Put | ApplyTo.Delete)] 
    public class Machine
    {
        [PrimaryKey]
        public int Id { get; set; }
        [Index(Unique = true)]
        public string Name { get; set; }
    }

    public class MachineValidator : AbstractValidator<Machine>
    {
        public MachineValidator()
        {
            RuleSet(ApplyTo.Post | ApplyTo.Put, () =>
            {
                RuleFor(r => r.Name).NotEmpty();
            });
        }
    }
    
    public class MachineResponse : ResponseStatus
    {
        public int Total { get; set; }
        public List<Machine> Results { get; set; }
    }
}
