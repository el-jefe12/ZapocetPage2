using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.CodeAnalysis;
using TexasGuyContractIdentity.Models;
using TexasGuyContractIdentity.Data;

namespace TexasGuyContractIdentity.Generic
{
    public abstract class GenericPageModel : PageModel
    {
        [AllowNull]
        internal ApplicationDbContext _context;

        public string Message { get; set; } = String.Empty;

    }
}
