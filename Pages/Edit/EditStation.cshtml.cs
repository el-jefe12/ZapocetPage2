using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TexasGuyContractIdentity.Generic;

namespace TexasGuyContract.Pages.Edit
{
    [Authorize]
    public class EditStationModel : GenericPageModel
    {


        public void OnGet()
        {

        }
    }
}
