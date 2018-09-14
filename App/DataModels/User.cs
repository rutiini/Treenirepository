// <copyright file="User.cs" company="rutiini">
// Created by Esa Ruissalo
// </copyright>
namespace Treenirepository.DataModels
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  /// <summary>
  /// A potential Database table for platform users.
  /// Currently unused, using a third party login should be the correct approach.
  /// </summary>
  [Table("User", Schema = "dbo")]
  public class User
  {
    /// <summary>
    /// Gets or sets the Id of the User.
    /// </summary>
    /// <value>int property: (database) Id.</value>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the UserName of the User.
    /// </summary>
    /// <value>string property: UserName.</value>
    [Required]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the FullName of the User.
    /// </summary>
    /// <value>string property: FullName.</value>
    [Required]
    public string FullName { get; set; }

    /// <summary>
    /// Gets or sets the Password of the User.
    /// </summary>
    /// <value>string property: Password.</value>
    [Required]
    public string Password { get; set; }
  }
}