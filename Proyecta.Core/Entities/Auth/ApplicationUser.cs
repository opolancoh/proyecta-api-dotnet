using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Proyecta.Core.Contracts;

namespace Proyecta.Core.Entities.Auth;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DisplayName { get; set; }
    
    // Auditable implementation
    public DateTime CreatedAt { get; set; }
    public string CreatedById { get; set; }
    public ApplicationUser? CreatedBy { get; init; }  // Navigation property for the creator
    
    public DateTime UpdatedAt { get; set; }
    public string UpdatedById { get; set; }
    public ApplicationUser? UpdatedBy { get; init; }  // Navigation property for the updater
}