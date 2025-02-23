using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.UseCases.DTOs
{
    public class CustomerDTO
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public InfoContactDTO Identification { get; set; }
        


        public void setIdContext(Guid id)
        {
            Id = id;
        }
    }
}
