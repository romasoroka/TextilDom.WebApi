using Luzanov.Application.Products.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luzanov.Application.Products.Commands
{
    public class UpdateProductCommand : CreateProductCommand
    {
        public int Id { get; set; }
    }
}
